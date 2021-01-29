using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterTagNumber : EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public FilterTagNumber()
        {
            ContainsExpression =
                items =>
                    items.Number.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Number);
            EqualToExpression =
                items => items.Number.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Number.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}