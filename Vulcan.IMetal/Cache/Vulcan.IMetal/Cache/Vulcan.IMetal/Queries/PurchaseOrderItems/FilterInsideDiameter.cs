using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterInsideDiameter : EnhancedRangeFilterBase<decimal, PurchaseOrderItem>
    {
        public FilterInsideDiameter()
        {
            MinExpression = items => items.PC_InsideDiameter >= MinValue;
            MaxExpression = items => items.PC_InsideDiameter <= MaxValue;
            EqualToExpression = items => items.PC_InsideDiameter == EqualToValue;
        }
    }
}