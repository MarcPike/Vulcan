namespace DAL.iMetal.Core.Queries
{
    public class PurchaseOrderItemUnitValues
    {
        public PurchaseOrderItemUnit Ordered { get; set; } = new PurchaseOrderItemUnit();
        public PurchaseOrderItemUnit Allocated { get; set; } = new PurchaseOrderItemUnit();
        public PurchaseOrderItemUnit Delivered { get; set; } = new PurchaseOrderItemUnit();
        public PurchaseOrderItemUnit Balance { get; set; } = new PurchaseOrderItemUnit();
    }
}