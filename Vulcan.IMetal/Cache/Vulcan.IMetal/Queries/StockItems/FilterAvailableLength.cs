using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterAvailableLength : EnhancedRangeFilterBase<decimal, Context.StockItems.StockItem>
    {
        public FilterAvailableLength()
        {
            MinExpression = items => (items.PhysicalQuantity ?? 0) - (items.AllocatedQuantity ?? 0) >= MinValue;
            MaxExpression = items => (items.PhysicalQuantity ?? 0) - (items.AllocatedQuantity ?? 0) <= MaxValue;
            EqualToExpression = items => (items.PhysicalQuantity ?? 0) - (items.AllocatedQuantity ?? 0) == EqualToValue;
        }
    }
}