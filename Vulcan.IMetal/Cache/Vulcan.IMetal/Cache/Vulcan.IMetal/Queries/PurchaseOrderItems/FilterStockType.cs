using System.Linq;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterStockType : EnhancedStringFilterBase<PurchaseOrderItem>
    {
        public FilterStockType()
        {
            ContainsExpression =
                items =>
                    items.Product.ProductCategory.ProductControl.Description.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Product.ProductCategory.ProductControl.Description);
            EqualToExpression =
                items => items.Product.ProductCategory.ProductControl.Description.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Product.ProductCategory.ProductControl.Description.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}
