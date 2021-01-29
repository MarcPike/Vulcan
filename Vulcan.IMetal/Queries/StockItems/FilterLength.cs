using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterLength : EnhancedRangeFilterBase<decimal, Context.StockItems.StockItem>
    {
        public FilterLength()
        {
            MinExpression = items => items.PC_Length >= MinValue;
            MaxExpression = items => items.PC_Length <= MaxValue;
            EqualToExpression = items => items.PC_Length == EqualToValue;
        }
    }
}