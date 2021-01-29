using System;
using Vulcan.IMetal.Context.Orders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Orders
{
    public class FilterSaleDate : EnhancedRangeFilterBase<DateTime, QueryOrderCapture>
    {
        public FilterSaleDate()
        {
            MinExpression = orders => orders.SalesHeader.SaleDate.Value >= MinValue;
            MaxExpression = orders => orders.SalesHeader.SaleDate.Value <= MaxValue;
            EqualToExpression = orders => orders.SalesHeader.SaleDate.Value == EqualToValue;
        }
    }
}