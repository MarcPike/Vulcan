using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterWidth : EnhancedRangeFilterBase<decimal, Context.StockItems.StockItem>
    {
        public FilterWidth()
        {
            MinExpression = items => items.PC_Width >= MinValue;
            MaxExpression = items => items.PC_Width <= MaxValue;
            EqualToExpression = items => items.PC_Width == EqualToValue;
        }
    }
}