using System.Collections.Generic;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterMetalType : EnhancedStringFilterBase<PurchaseOrderItem>
    {
        public override List<string> GetSuggestions(string coid)
        {
            return new List<string>()
            {
                "ALUMINUM TUBE",
                "ALUMINUM BAR",
                "CARBON & ALLOY STEEL M/C PARTS",
                "CARBON & ALLOY STEEL PLATE",
                "CARBON & ALLOY STEEL FORGINGS",
                "CARBON & ALLOY STEEL TUBE",
                "CARBON & ALLOY STEEL FLAT BAR",
                "CARBON & ALLOY STEEL ROUND BAR",
                "COBALT",
                "MACHINED COMPONENT",
                "NICKEL ALLOY MACHINED PARTS",
                "NICKEL ALLOY TUBE",
                "NICKEL ALLOY PIPE",
                "NICKEL ALLOY FLAT BAR",
                "NICKEL ALLOY BILLET",
                "NICKEL ALLOY INGOT",
                "NICKEL ALLOY SCRAP",
                "NICKEL ALLOY ROUND BAR",
                "STAINLESS STEEL FLAT BAR",
                "STAINLESS WIRE",
                "STAINLESS STEEL BILLET",
                "STAINLESS STEEL TUBE",
                "STAINLESS STEEL ROUND BAR",
                "ST. ST. MACHINED PARTS",
                "TOOL STEEL BAR",
                "YELLOW METALS",
                "YELLOW METAL BAR",
                "YELLOW METAL TUBE"
            };
        }

        public FilterMetalType()
        {
            HasSuggestions = true;

            ContainsExpression =
                items =>
                    items.Product.ProductCategory.StockAnalysisCode_Analysis2Id.Description.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Product.ProductCategory.StockAnalysisCode_Analysis2Id.Description);
            EqualToExpression =
                items => items.Product.ProductCategory.StockAnalysisCode_Analysis2Id.Description.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Product.ProductCategory.StockAnalysisCode_Analysis2Id.Description.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}