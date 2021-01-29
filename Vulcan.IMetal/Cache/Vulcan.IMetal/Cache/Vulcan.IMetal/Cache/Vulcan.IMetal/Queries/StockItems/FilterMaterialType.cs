using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterMaterialType : EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public FilterMaterialType()
        {
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