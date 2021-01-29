using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterProductCondition : EnhancedStringFilterBase<PurchaseOrderItem>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetPurchaseOrdersContextForCoid(coid);
            return context.Product.OrderBy(x => x.SpecificationValue2).Select(x => x.SpecificationValue2).Distinct().ToList();
        }

        public FilterProductCondition()
        {
            HasSuggestions = true;

            ContainsExpression =
                items =>
                    items.Product.SpecificationValue2.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Product.SpecificationValue2);
            EqualToExpression =
                items => items.Product.SpecificationValue2.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Product.SpecificationValue2.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}