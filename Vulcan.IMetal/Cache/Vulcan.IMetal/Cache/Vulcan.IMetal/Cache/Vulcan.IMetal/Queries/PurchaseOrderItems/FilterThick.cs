using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterThick : EnhancedRangeFilterBase<decimal, PurchaseOrderItem>
    {
        public FilterThick()
        {
            MinExpression = items => items.PC_Thick >= MinValue;
            MaxExpression = items => items.PC_Thick <= MaxValue;
            EqualToExpression = items => items.PC_Thick == EqualToValue;
        }
    }
}