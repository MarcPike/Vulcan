using System;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterDueDate : EnhancedRangeFilterBase<DateTime, PurchaseOrderItem>
    {
        public FilterDueDate()
        {
            MinExpression = items => items.DueDate.Value >= MinValue;
            MaxExpression = items => items.DueDate.Value <= MaxValue;
            EqualToExpression = items => items.DueDate.Value == EqualToValue;
        }
    }
}