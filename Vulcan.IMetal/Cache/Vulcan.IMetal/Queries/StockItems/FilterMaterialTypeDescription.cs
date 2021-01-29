using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterMaterialTypeDescription : EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public FilterMaterialTypeDescription()
        {
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