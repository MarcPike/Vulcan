using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterProductDescription : EnhancedStringFilterBase<PurchaseOrderItem>
    {
        public FilterProductDescription()
        {
            ContainsExpression =
                items =>
                    items.Product.Description.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Product.Description);
            EqualToExpression =
                items => items.Product.Description.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Product.Description.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}