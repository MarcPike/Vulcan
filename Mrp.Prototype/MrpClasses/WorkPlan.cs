using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace Mrp.Prototype.MrpClasses
{
    public class WorkPlan : MrpObject
    {
        public List<WorkPlanItem> WorkPlanItems { get; set; } = new List<WorkPlanItem>();
        public ProductMaster StartingProduct { get; set; } 
        public ProductMaster FinishedProduct { get; set; } 
        public Planner Planner { get; set; }

        public WorkPlan(Planner planner, ProductMaster startingProduct, ProductMaster finishedProduct)
        {
            Planner = planner;
            StartingProduct = startingProduct;
            FinishedProduct = finishedProduct;
        }
    }
}