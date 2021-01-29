using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterLength : EnhancedRangeFilterBase<decimal, PurchaseOrderItem>
    {
        public FilterLength()
        {
            MinExpression = items => items.PC_Length >= MinValue;
            MaxExpression = items => items.PC_Length <= MaxValue;
            EqualToExpression = items => items.PC_Length == EqualToValue;
        }
    }
}