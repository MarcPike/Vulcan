using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterProductGrade : EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetStockItemsContextForCoid(coid);
            return context.StockGrade.OrderBy(x => x.Description).Select(x => x.Description).Distinct().ToList();
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