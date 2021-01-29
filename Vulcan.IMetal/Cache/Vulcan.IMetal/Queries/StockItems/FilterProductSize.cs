using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterProductSize : EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public FilterProductSize()
        {
            ContainsExpression =
                items =>
                    items.Product.SizeDescription.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Product.SizeDescription);
            EqualToExpression =
                items => items.Product.SizeDescription.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Product.SizeDescription.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}