using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterAvailableWeight : EnhancedRangeFilterBase<decimal, Context.StockItems.StockItem>
    {
        public FilterAvailableWeight()
        {
            MinExpression = items => (items.PhysicalWeight ?? 0) - (items.AllocatedWeight ?? 0) >= MinValue;
            MaxExpression = items => (items.PhysicalWeight ?? 0) - (items.AllocatedWeight ?? 0) <= MaxValue;
            EqualToExpression = items => (items.PhysicalWeight ?? 0) - (items.AllocatedWeight ?? 0) == EqualToValue;
        }
    }
}