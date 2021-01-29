using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterDensity : EnhancedRangeFilterBase<decimal, Context.StockItems.StockItem>
    {
        public FilterDensity()
        {
            MinExpression = items => items.PC_Density >= MinValue;
            MaxExpression = items => items.PC_Density <= MaxValue;
            EqualToExpression = items => items.PC_Density == EqualToValue;
        }
    }
}