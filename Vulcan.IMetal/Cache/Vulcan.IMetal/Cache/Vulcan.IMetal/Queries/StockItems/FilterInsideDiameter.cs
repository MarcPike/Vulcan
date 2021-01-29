using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterInsideDiameter : EnhancedRangeFilterBase<decimal, Context.StockItems.StockItem>
    {
        public FilterInsideDiameter()
        {
            MinExpression = items => items.PC_Dimensions.InsideDiameter >= MinValue;
            MaxExpression = items => items.PC_Dimensions.InsideDiameter <= MaxValue;
            EqualToExpression = items => items.PC_Dimensions.InsideDiameter == EqualToValue;
        }
    }
}