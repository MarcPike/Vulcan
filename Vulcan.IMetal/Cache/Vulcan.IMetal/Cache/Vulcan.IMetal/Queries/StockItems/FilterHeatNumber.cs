using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterHeatNumber : EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public FilterHeatNumber()
        {
            ContainsExpression =
                items =>
                    items.StockCast.CastNumber.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.StockCast.CastNumber);
            EqualToExpression =
                items => items.StockCast.CastNumber.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.StockCast.CastNumber.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}