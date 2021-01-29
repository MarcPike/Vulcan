using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace Mrp.Prototype.MrpClasses
{
    public class Resource : MrpObject
    {
        public Shop Shop { get; }
        public ResourceType ResourceType { get; set; }
        public string Name { get; set; }
        public List<Job> Jobs { get; set; } = new List<Job>();
        public Job CurrentJob { get; set; }
        public ShopWorker CurrentWorker { get; set; }
        public ResourceStatus ResourceStatus { get; set; } = ResourceStatus.Idle;
        public bool Available { get; set; } = true;

        public void StartWork(ShopWorker worker, Guid jobId)
        {
            CurrentJob = Jobs.SingleOrDefault(x => x.Id == jobId);
            if (CurrentJob != null)
            {
                CurrentWorker = worker;
                CurrentJob.StartWork(worker);
                ResourceStatus = ResourceStatus.Busy;
            }
        }

        public void SuspendWork(ShopWorker worker, Guid jobId, int completedPieces)
        {
            if (ResourceStatus != ResourceStatus.Busy)
            {
                throw new Exception("No Job is currently being worked on");
            }

            if (worker.Id != CurrentWorker.Id)
            {
                throw new ArgumentException("Invalid worker");
            }

            if (jobId != CurrentJob.Id)
            {
                throw new ArgumentException("Invalid job");
            }

            CurrentJob.Suspend(completedPieces);
            ResourceStatus = ResourceStatus.Suspended;
        }

        public void CompleteWork(ShopWorker worker, Guid jobId, int completedPieces)
        {
            if (ResourceStatus != ResourceStatus.Busy)
            {
                throw new Exception("No Job is currently being worked on");
            }

            if (worker.Id != CurrentWorker.Id)
            {
                throw new ArgumentException("Invalid worker");
            }

            if (jobId != CurrentJob.Id)
            {
                throw new ArgumentException("Invalid job");
            }

            CurrentJob.Complete(completedPieces);
            ResourceStatus = ResourceStatus.WaitingOnTransport;
        }

        public void AddJob(WorkOrderItem workOrderItem, ShopScheduler shopScheduler, int pieces)
        {
            Jobs.Add(new Job(this,workOrderItem,shopScheduler,pieces));
        }

        public int BookedMinutes()
        {
            var result = 0;
            foreach (var job in Jobs)
            {
                result += job.WorkOrderItem.WorkPlan.WorkPlanItems.Sum(x => x.GetTotalDuration(job.RequiredPieces).Minutes);
            }

            return result;
        }

        public Resource(Shop shop)
        {
            Shop = shop;
        }
    }
}