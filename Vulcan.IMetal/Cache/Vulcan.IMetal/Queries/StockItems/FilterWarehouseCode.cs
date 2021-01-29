using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterWarehouseCode : EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public FilterWarehouseCode()
        {
            ContainsExpression =
                items =>
                    items.Warehouse.Code.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Warehouse.Code);
            EqualToExpression =
                items => items.Warehouse.Code.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Warehouse.Code.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}