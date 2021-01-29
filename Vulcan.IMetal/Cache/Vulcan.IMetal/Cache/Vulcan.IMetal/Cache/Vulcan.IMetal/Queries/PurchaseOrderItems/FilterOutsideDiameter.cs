using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterOutsideDiameter : EnhancedRangeFilterBase<decimal, PurchaseOrderItem>
    {
        public FilterOutsideDiameter()
        {
            MinExpression = items => items.PC_OutsideDiameter >= MinValue;
            MaxExpression = items => items.PC_OutsideDiameter <= MaxValue;
            EqualToExpression = items => items.PC_OutsideDiameter == EqualToValue;
        }
    }
}