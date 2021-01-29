using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterMaterialType : EnhancedStringFilterBase<PurchaseOrderItem>
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