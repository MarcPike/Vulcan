using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterWidth : EnhancedRangeFilterBase<decimal, PurchaseOrderItem>
    {
        public FilterWidth()
        {
            MinExpression = items => items.PC_Width >= MinValue;
            MaxExpression = items => items.PC_Width <= MaxValue;
            EqualToExpression = items => items.PC_Width == EqualToValue;
        }
    }
}