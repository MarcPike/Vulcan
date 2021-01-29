using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.StockItems;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterWarehouseName: EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public FilterWarehouseName()
        {
            ContainsExpression =
                items =>
                    items.Warehouse.Name.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Warehouse.Name);
            EqualToExpression =
                items => items.Warehouse.Name.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Warehouse.Name.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}
