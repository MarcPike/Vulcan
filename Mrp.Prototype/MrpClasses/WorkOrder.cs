using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace Mrp.Prototype.MrpClasses
{
    public class WorkOrder : MrpObject
    {
        public CrmQuote CrmQuote { get; set; }
        public CrmQuoteItem CrmQuoteItem { get; set; }
        public WorkPlan WorkPlan { get; set; }
        public List<WorkOrderItem> WorkOrderItems { get; set; } = new List<WorkOrderItem>();
        public WorkOrder(Shop shop, WorkPlan workPlan, CrmQuote quote, CrmQuoteItem quoteItem)
        {
            CrmQuote = quote;
            CrmQuoteItem = quoteItem;
            WorkPlan = workPlan;
            foreach (var workPlanItem in workPlan.WorkPlanItems)
            {
                WorkOrderItems.Add(new WorkOrderItem(shop, workPlan, workPlanItem, this, quoteItem.QuotePrice.StartingProduct, quoteItem.QuotePrice.FinishedProduct, quoteItem.QuotePrice.RequiredQuantity, workPlanItem.ResourceType));
            }
        }

        //public ScheduleWorkOrderItem()

    }
}