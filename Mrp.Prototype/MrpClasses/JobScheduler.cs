using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;

namespace Mrp.Prototype.MrpClasses
{

    public class JobScheduler
    {
        public ResourceType ResourceType { get; }
        public List<Resource> Resources { get; }
        public RequiredQuantity RequiredQuantity { get; }
        public bool ScheduleComplete => (RequiredQuantity.Pieces == 0);
        public List<Job> ScheduledJobs { get; set; } = new List<Job>();

        public Resource FindResourceWithLightestWorkload()
        {
            return Resources.Where(x=>x.ResourceType == ResourceType && x.Available).OrderBy(x => x.BookedMinutes()).FirstOrDefault();
        }

        public void ScheduleJobAutomatically(ShopScheduler scheduler, WorkOrderItem workOrderItem, int requiredPieces)
        {
            if (requiredPieces > RequiredQuantity.Pieces) throw new ArgumentException("requiredPieces value is greater than RequiredQuantity.Pieces");
            var resource = FindResourceWithLightestWorkload();

            CreateJob(scheduler, workOrderItem, requiredPieces, resource);
        }

        public void ScheduleJob(ShopScheduler scheduler, Resource resource, WorkOrderItem workOrderItem, int requiredPieces)
        {
            if (requiredPieces > RequiredQuantity.Pieces) throw new ArgumentException("requiredPieces value is greater than RequiredQuantity.Pieces");
            if (resource.ResourceType != ResourceType) throw new ArgumentException($"Wrong resource type. Expected {ResourceType} but this Resource is {resource.ResourceType}");

            CreateJob(scheduler, workOrderItem, requiredPieces, resource);
        }

        private void CreateJob(ShopScheduler scheduler, WorkOrderItem workOrderItem, int requiredPieces, Resource resource)
        {
            var job = new Job(resource, workOrderItem, scheduler, requiredPieces);
            RequiredQuantity.Pieces -= requiredPieces;
            workOrderItem.Jobs.Add(job);
        }

        public void RemoveJob(ShopScheduler scheduler, Job job)
        {
            if (ScheduledJobs.All(x => x.Id != job.Id))
            {
                throw new ArgumentException("Job not found");
            }

            RequiredQuantity.Pieces += job.RequiredPieces;
            var removeJob = ScheduledJobs.Single(x => x.Id == job.Id);
            ScheduledJobs.Remove(removeJob);
        }

        public JobScheduler(WorkOrderItem workOrderItem)
        {
            ResourceType = workOrderItem.ResourceType;
            RequiredQuantity = workOrderItem.RequiredQuantity;
            Resources = workOrderItem.Shop.Resources.Where(x=>x.ResourceType == ResourceType && x.Available).ToList();
        }
    }
}
