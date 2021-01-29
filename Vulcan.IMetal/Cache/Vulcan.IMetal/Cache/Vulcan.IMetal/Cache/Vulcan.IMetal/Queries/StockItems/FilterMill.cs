using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterMill : EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public FilterMill()
        {
            ContainsExpression =
                items =>
                    items.StockCast.Mill.Code.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.StockCast.Mill.Code);
            EqualToExpression =
                items => items.StockCast.Mill.Code.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.StockCast.Mill.Code.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}