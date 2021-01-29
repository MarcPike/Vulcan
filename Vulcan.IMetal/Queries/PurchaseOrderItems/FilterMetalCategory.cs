using System.Collections.Generic;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterMetalCategory : EnhancedStringFilterBase<PurchaseOrderItem>
    {
        public override List<string> GetSuggestions(string coid)
        {
            return new List<string>()
            {
                "LOW ALLOY",
                "NICKEL",
                "OTHER",
                "STAINLESS"
            };
        }

        public FilterMetalCategory()
        {
            HasSuggestions = true;

            ContainsExpression =
                items =>
                    items.Product.ProductCategory.StockAnalysisCode_Analysis1Id.Description.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Product.ProductCategory.StockAnalysisCode_Analysis1Id.Description);
            EqualToExpression =
                items => items.Product.ProductCategory.StockAnalysisCode_Analysis1Id.Description.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Product.ProductCategory.StockAnalysisCode_Analysis1Id.Description.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}