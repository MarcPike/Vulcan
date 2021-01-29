using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterCategoryCodeDescription : EnhancedStringFilterBase<PurchaseOrderItem>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetPurchaseOrdersContextForCoid(coid);
            return context.ProductCategory.Select(x => x.Description).ToList();
        }

        //public FilterCategoryCodeDescription()
        //{
        //    ContainsExpression =
        //        items =>
        //            items.PurchaseOrderHeader_PurchaseHeaderId.CategoryId..ToLower().Contains(
        //                Value.ToLower());
        //    InListExpression =
        //        items =>
        //            StockItems.Contains(items.PurchaseOrderHeader_PurchaseHeaderId.CategoryId.);
        //    EqualToExpression =
        //        items => items.PurchaseOrderHeader_PurchaseHeaderId.PurchaseCategoryCode.Description.ToLower() == Value.ToLower();
        //    StartsWithExpression =
        //        items =>
        //            items.PurchaseOrderHeader_PurchaseHeaderId.PurchaseCategoryCode.Description.ToLower()
        //                .StartsWith(Value.ToLower());

        //}
    }
}