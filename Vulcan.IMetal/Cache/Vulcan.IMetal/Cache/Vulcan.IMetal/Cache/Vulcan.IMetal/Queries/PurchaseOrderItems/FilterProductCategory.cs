using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterProductCategory : EnhancedStringFilterBase<PurchaseOrderItem>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetPurchaseOrdersContextForCoid(coid);
            return context.ProductCategory.OrderBy(x => x.Category).Select(x => x.Category).Distinct().ToList();
        }

        public FilterProductCategory()
        {
            HasSuggestions = true;

            ContainsExpression =
                items =>
                    items.Product.ProductCategory.Category.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Product.ProductCategory.Category);
            EqualToExpression =
                items => items.Product.ProductCategory.Category.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Product.ProductCategory.Category.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}