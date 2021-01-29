using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterAvailableWeight : EnhancedRangeFilterBase<decimal, PurchaseOrderItem>
    {
        public FilterAvailableWeight()
        {
            MinExpression = items => (items.OrderedWeight ?? 0) - (items.AllocatedWeight ?? 0) >= MinValue;
            MaxExpression = items => (items.OrderedWeight ?? 0) - (items.AllocatedWeight ?? 0) <= MaxValue;
            EqualToExpression = items => (items.OrderedWeight ?? 0) - (items.AllocatedWeight ?? 0) == EqualToValue;
        }
    }
}