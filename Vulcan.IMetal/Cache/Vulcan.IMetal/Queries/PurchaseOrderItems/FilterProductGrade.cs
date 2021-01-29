using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterProductGrade : EnhancedStringFilterBase<PurchaseOrderItem>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetPurchaseOrdersContextForCoid(coid);
            return context.ProductCategory.OrderBy(x => x.Category).Select(x => x.Category).Distinct().ToList();
        }

        public FilterProductGrade()
        {
            HasSuggestions = true;

            ContainsExpression =
                items =>
                    items.Product.StockGrade_GradeId.Code.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Product.StockGrade_GradeId.Code);
            EqualToExpression =
                items => items.Product.StockGrade_GradeId.Code.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Product.StockGrade_GradeId.Code.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}