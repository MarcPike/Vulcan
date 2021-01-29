using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterStockType: EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetStockItemsContextForCoid(coid);
            return context.ProductControl.OrderBy(x => x.Description).Select(x => x.Description).Distinct().ToList();
        }

        public FilterStockType()
        {
            HasSuggestions = true;

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
