using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterProductCode : EnhancedStringFilterBase<PurchaseOrderItem>
    {
        public FilterProductCode()
        {
            ContainsExpression =
                items =>
                    items.Product.Code.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Product.Code);
            EqualToExpression =
                items => items.Product.Code.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Product.Code.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}