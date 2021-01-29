using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterLocation: EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public FilterLocation()
        {
            ContainsExpression =
                items =>
                    items.Location.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Location);
            EqualToExpression =
                items => items.Location.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Location.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}
