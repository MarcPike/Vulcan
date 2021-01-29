using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterOutsideDiameter : EnhancedRangeFilterBase<decimal, Context.StockItems.StockItem>
    {
        public FilterOutsideDiameter()
        {
            MinExpression = items => items.PC_OutsideDiameter >= MinValue;
            MaxExpression = items => items.PC_OutsideDiameter <= MaxValue;
            EqualToExpression = items => items.PC_OutsideDiameter == EqualToValue;
        }
    }
}