using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterDensity : EnhancedRangeFilterBase<decimal, PurchaseOrderItem>
    {
        public FilterDensity()
        {
            MinExpression = items => items.PC_Density >= MinValue;
            MaxExpression = items => items.PC_Density <= MaxValue;
            EqualToExpression = items => items.PC_Density == EqualToValue;
        }
    }
}