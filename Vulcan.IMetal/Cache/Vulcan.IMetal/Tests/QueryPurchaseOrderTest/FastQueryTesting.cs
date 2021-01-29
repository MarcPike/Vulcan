using System;
using System.Diagnostics;
using System.Linq;
using Devart.Data.Linq.Monitoring;
using NUnit.Framework;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.Queries.PurchaseOrderItems;

namespace Vulcan.IMetal.Tests.QueryPurchaseOrderTest
{
    [TestFixture]
    public class FastQueryTesting
    {
        private readonly LinqMonitor _monitor = new LinqMonitor();

        public string Coid { get; set; }
        public PurchaseOrdersContext Context;

        [SetUp]
        public void Setup()
        {
            Coid = "CAN";
            Context = ContextFactory.GetPurchaseOrdersContextForCoid(Coid);
            Context.Connection.Open();
        }

        [TearDown]
        public void TearDown()
        {
            Context.Connection.Close();
        }

        [Test]
        public void RawQueryTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Context.Connection.Open();
            var query = PurchaseOrderItemsAdvancedQuery.AsQueryable(Coid, Context);

            //query = query.Where(x => x.InsideDiameter <= 4).AsQueryable();
            //query = query.Where(x => x.OutsideDiameter >= 6).AsQueryable();
            //query = query.Where(x => x.UnitValues.Balance.Length > 0);
            query = query.Where(x => x.StockGrade == "4130M");
            //query = query.Where(x => x.PoNumber == 26509);
            //query = query.Where(x => x.ItemNumber == 1);


            sw.Stop();
            Console.WriteLine("Query construction time: "+sw.Elapsed);
            sw.Start();
            _monitor.IsActive = true;
            var result = query.ToList();
            foreach (var purchaseOrderItem in result.ToList())
            {
                Console.WriteLine(ObjectDumper.Dump(purchaseOrderItem));
                //Console.WriteLine($"PurchaseOrderId: {purchaseOrderItem.PurchaseOrderHeaderId} Code: {purchaseOrderItem.ProductCode} Available Length {purchaseOrderItem.UnitValues.Balance.Length} Cost/Lb: {purchaseOrderItem.CostPerLb} Condition: {purchaseOrderItem.ProductCondition}");
            }
            sw.Stop();
            Console.WriteLine($"Query execution time: {sw.Elapsed} rows: {result.Count}");
            _monitor.IsActive = false;

        }
    }
}
