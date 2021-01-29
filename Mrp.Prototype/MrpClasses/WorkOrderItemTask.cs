using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;

namespace Mrp.Prototype.MrpClasses
{
    public class WorkOrderItemTask : MrpObject
    {
        public ResourceType ResourceType { get; set; }
        public RequiredQuantity RequiredQuantity { get; set; }
        public List<Job> Jobs { get; set; } = new List<Job>();

        public void AddJob(WorkOrderItem workOrderItem, int pieces, Resource resource, ShopScheduler scheduler)
        {
            if (pieces > RequiredQuantity.Pieces) throw new ArgumentException($"Pieces required is less than {pieces}");

            if (ResourceType != resource.ResourceType) throw new ArgumentException($"Resource ({resource}) is for the wrong ResourceType. Should be {ResourceType} but it is {resource.ResourceType}");

            Jobs.Add(new Job(resource, workOrderItem, scheduler, pieces)
            {
                Resource = resource,
                ReceivedPieces = 0,
                RequiredPieces = pieces,
                CompletedPieces = 0,
                TransportPieces = 0,
                ShopScheduler = scheduler
            });
        }
    }
}