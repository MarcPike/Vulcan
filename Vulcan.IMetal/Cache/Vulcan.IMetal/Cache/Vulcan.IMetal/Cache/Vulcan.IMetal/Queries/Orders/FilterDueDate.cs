using System;
using Vulcan.IMetal.Context.Orders;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Orders
{
    public class FilterDueDate : EnhancedRangeFilterBase<DateTime, QueryOrderCapture>
    {
        public FilterDueDate()
        {
            MinExpression = orders => orders.SalesHeader.DueDate.Value >= MinValue;
            MaxExpression = orders => orders.SalesHeader.DueDate.Value <= MaxValue;
            EqualToExpression = orders => orders.SalesHeader.DueDate.Value == EqualToValue;
        }
    }
}