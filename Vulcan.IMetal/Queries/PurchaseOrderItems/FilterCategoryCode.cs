using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterCategoryCode : EnhancedStringFilterBase<PurchaseOrderItem>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetPurchaseOrdersContextForCoid(coid);
            return context.PurchaseCategoryCode.Select(x => x.Code).ToList();
        }

        public FilterCategoryCode()
        {
            HasSuggestions = true;

            ContainsExpression =
                items =>
                    items.PurchaseOrderHeader_PurchaseHeaderId.PurchaseCategoryCode.Code.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.PurchaseOrderHeader_PurchaseHeaderId.PurchaseCategoryCode.Code);
            EqualToExpression =
                items => items.PurchaseOrderHeader_PurchaseHeaderId.PurchaseCategoryCode.Code.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.PurchaseOrderHeader_PurchaseHeaderId.PurchaseCategoryCode.Code.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}