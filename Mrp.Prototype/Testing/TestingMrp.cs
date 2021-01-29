using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using Mrp.Prototype.MrpClasses;
using NUnit.Framework;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace Mrp.Prototype.Testing
{
    [TestFixture()]
    public class TestingMrp
    {
        public Shop Shop;
        public List<Planner> Planners { get; set; } = new List<Planner>();
        public List<ShopScheduler> Schedulers { get; set; } = new List<ShopScheduler>();
        public WorkPlan WorkPlan { get; set; }
        public WorkOrder WorkOrder { get; set; }
        public List<Job> Jobs { get; set; } = new List<Job>();
        private RepositoryBase<CrmQuote> _quoteRep; 

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
            _quoteRep = new RepositoryBase<CrmQuote>();
            InitializeShop();
            InitializePlanners();
            InitializeSchedulers();
            InitializeWorkPlan();
            InitializeWorkOrder();
            ScheduleJobsWorkOrder();
        }

        [Test]
        public void EvauluateInitialization()
        {
            Assert.IsTrue(WorkOrder.WorkOrderItems.Count > 0);
            foreach (var workOrderItem in WorkOrder.WorkOrderItems)
            {
                foreach (var job in workOrderItem.Jobs)
                {
                    Console.WriteLine($"Total Pieces: {job.RequiredPieces} Resource: {job.Resource.Name} DurationPerPiece (minutes): {job.WorkOrderItem.Duration.TotalMinutes}");

                }
                //Assert.IsTrue(workOrderItem.EntireWorkOrderTotalDurationEstimate.TotalMinutes > 0);
            }
        }

        private void ScheduleJobsWorkOrder()
        {
            // Bob the scheduler Logs in
            var scheduler = Schedulers.FirstOrDefault(x => x.GetPinCode == "0100");
            Assert.IsNotNull(scheduler);

            foreach (var workOrderItem in WorkOrder.WorkOrderItems)
            {
                var resource = workOrderItem.ResourceType;
                workOrderItem.JobScheduler.ScheduleJobAutomatically(scheduler,workOrderItem,workOrderItem.RequiredQuantity.Pieces);
            }
        }

        

        public void InitializeWorkOrder()
        {
            var quote = _quoteRep.AsQueryable().Single(x => x.QuoteId == 14589);

            WorkOrder = new WorkOrder(Shop, WorkPlan, quote, quote.Items.First().AsQuoteItem());
        }

        public void InitializeWorkPlan()
        {
            var startingProduct = new ProductMaster("INC", "4140 12 ANN");
            var finishedProduct = new ProductMaster("INC", "4140 12.5-9.75 80L");

            Assert.IsNotNull(startingProduct);
            Assert.IsNotNull(finishedProduct);

            WorkPlan = new WorkPlan(Planners.First(), startingProduct,finishedProduct);

            // WorkPlan - needs to have Cutting, Boring and HeatTreat then Ship
            var workPlanItemForSaw = new WorkPlanItem()
            {
                ResourceType = ResourceType.Saw,
                TransportTime = TimeSpan.FromMinutes(30),
                PrepTime = TimeSpan.FromMinutes(15),
                CleanupTime = TimeSpan.FromMinutes(5),
                DurationPerPiece = TimeSpan.FromMinutes(60),
            };
            Assert.AreEqual(workPlanItemForSaw.GetTotalDuration(1),TimeSpan.FromMinutes(110));
            WorkPlan.WorkPlanItems.Add(workPlanItemForSaw);

            var workPlanItemForBore = new WorkPlanItem()
            {
                ResourceType = ResourceType.Bore,
                TransportTime = TimeSpan.FromMinutes(15),
                PrepTime = TimeSpan.FromMinutes(10),
                CleanupTime = TimeSpan.FromMinutes(10),
                DurationPerPiece = TimeSpan.FromMinutes(240),
            };
            Assert.AreEqual(workPlanItemForBore.GetTotalDuration(1), TimeSpan.FromMinutes(275));
            WorkPlan.WorkPlanItems.Add(workPlanItemForBore);

            var workPlanItemForHeatTreat = new WorkPlanItem()
            {
                ResourceType = ResourceType.HeatTreat,
                TransportTime = TimeSpan.FromMinutes(15),
                PrepTime = TimeSpan.FromMinutes(60),
                CleanupTime = TimeSpan.FromMinutes(30),
                DurationPerPiece = TimeSpan.FromMinutes(480),
            };
            Assert.AreEqual(workPlanItemForHeatTreat.GetTotalDuration(1), TimeSpan.FromMinutes(585));
            WorkPlan.WorkPlanItems.Add(workPlanItemForHeatTreat);

            var workPlanItemForShip = new WorkPlanItem()
            {
                ResourceType = ResourceType.Ship,
                TransportTime = TimeSpan.FromMinutes(0),
                PrepTime = TimeSpan.FromMinutes(10),
                CleanupTime = TimeSpan.FromMinutes(10),
                DurationPerPiece = TimeSpan.FromMinutes(15),
            };
            Assert.AreEqual(workPlanItemForShip.GetTotalDuration(1), TimeSpan.FromMinutes(35));
            WorkPlan.WorkPlanItems.Add(workPlanItemForShip);

        }

        private void InitializeSchedulers()
        {
            Schedulers.Add(new ShopScheduler()
            {
                Name = "Bob",
                Pin = 100
            });
        }

        private void InitializePlanners()
        {
            Planners.Add(new Planner() {Name = "Marc"});
            Planners.Add(new Planner() { Name = "Isidro" });
        }



        private void InitializeShop()
        {
            Shop = new Shop() {Name = "Telge",
                Resources = new List<Resource>()
                {
                    new Resource(Shop) {Name = "Saw 1", ResourceType = DAL.Vulcan.Mongo.DocClass.Quotes.ResourceType.Saw},
                    new Resource(Shop) {Name = "Bore #1", ResourceType = DAL.Vulcan.Mongo.DocClass.Quotes.ResourceType.Bore},
                    new Resource(Shop) {Name = "HeatTreat #1", ResourceType = DAL.Vulcan.Mongo.DocClass.Quotes.ResourceType.HeatTreat},
                    new Resource(Shop) {Name = "Shipping #1", ResourceType = DAL.Vulcan.Mongo.DocClass.Quotes.ResourceType.Ship},
                },
                ShopWorkers = new List<ShopWorker>()
                {
                    new ShopWorker() {Name = "Marc Pike", Pin = 1014},
                    new ShopWorker() {Name = "Isidro", Pin = 1000}
                },
                ShopManagers = new List<ShopManager>()
                {
                    new ShopManager() {Name = "Stanton", Pin = 0}
                }
            };
        }

        [Test]
        public void PinCodeTest()
        {
            Assert.IsTrue(Shop.GetShopWorker("1014").Name == "Marc Pike");
            Assert.IsTrue(Shop.GetShopManager("0000").Name == "Stanton");
            Assert.IsNull(Shop.GetShopWorker("9999"));
            Assert.IsNull(Shop.GetShopManager("9999"));
        }

    }
}
