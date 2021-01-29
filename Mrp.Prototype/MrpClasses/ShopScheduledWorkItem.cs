using DAL.Vulcan.Mongo.Quotes;

namespace Mrp.Prototype.MrpClasses
{
    public class ShopScheduledWorkItem : MrpObject
    {
        public Resource Resource { get; set; }
        public WorkOrderItem WorkOrderItem { get; set; }
        public RequiredQuantity RequiredQuantity { get; set; }

    }
}