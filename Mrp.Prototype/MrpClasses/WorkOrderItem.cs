using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;

namespace Mrp.Prototype.MrpClasses
{
    public class WorkOrderItem : MrpObject
    {
        public Shop Shop { get; }
        public WorkOrder WorkOrder { get; }
        public ProductMaster StartingProduct { get;  } 
        public ProductMaster FinishedProduct { get;  } 
        public WorkPlan WorkPlan { get; set; }
        public WorkPlanItem WorkPlanItem { get;  }
        public RequiredQuantity RequiredQuantity { get; }
        public ResourceType ResourceType { get; }
        public List<Job> Jobs { get; set; } = new List<Job>();
        public WorkOrderItemStatus Status { get; set; } = WorkOrderItemStatus.NotScheduled;
        public JobScheduler JobScheduler { get; }

        public TimeSpan Duration
        {
            get { return WorkPlanItem.GetTotalDuration(Jobs.Sum(x => x.RequiredPieces)); }
        }

        public TimeSpan EntireWorkOrderTotalDurationEstimate
        {
            get
            {
                var result = new TimeSpan(0);
                foreach (var workItem in WorkPlan.WorkPlanItems)
                {
                    result += workItem.GetTotalDuration(RequiredQuantity.Pieces);
                }

                return result;
            }
        }

        public WorkOrderItem(Shop shop, WorkPlan workPlan, WorkPlanItem workPlanItem, WorkOrder workOrder, ProductMaster startingProduct, ProductMaster finishedProduct, RequiredQuantity requiredQuantity, ResourceType resourceType)
        {
            WorkOrder = workOrder;
            StartingProduct = startingProduct;
            FinishedProduct = finishedProduct;
            RequiredQuantity = (RequiredQuantity) requiredQuantity.Clone();
            WorkPlan = workPlan;
            WorkPlanItem = workPlanItem;
            ResourceType = resourceType;
            Shop = shop;
            JobScheduler = new JobScheduler(this);
        }

    }
}