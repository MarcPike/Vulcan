using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterAvailableLength : EnhancedRangeFilterBase<decimal, PurchaseOrderItem>
    {
        public FilterAvailableLength()
        {
            MinExpression = items => (items.OrderedQuantity ?? 0) - (items.AllocatedQuantity ?? 0) >= MinValue;
            MaxExpression = items => (items.OrderedQuantity ?? 0) - (items.AllocatedQuantity ?? 0) <= MaxValue;
            EqualToExpression = items => (items.OrderedQuantity ?? 0) - (items.AllocatedQuantity ?? 0) == EqualToValue;
        }
    }
}