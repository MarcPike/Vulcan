using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterThick : EnhancedRangeFilterBase<decimal, Context.StockItems.StockItem>
    {
        public FilterThick()
        {
            MinExpression = items => items.PC_Thick >= MinValue;
            MaxExpression = items => items.PC_Thick <= MaxValue;
            EqualToExpression = items => items.PC_Thick == EqualToValue;
        }
    }
}