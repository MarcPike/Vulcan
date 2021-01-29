using System;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterCreateDate: EnhancedRangeFilterBase<DateTime,PurchaseOrderItem>
    {
        public FilterCreateDate()
        {
            MinExpression = items => items.Cdate >= MinValue;
            MaxExpression = items => items.Cdate <= MaxValue;
            EqualToExpression = items => items.Cdate == EqualToValue;
        }
    }
}
