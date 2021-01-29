using System;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterCreateDate: EnhancedRangeFilterBase<DateTime, Context.StockItems.StockItem>
    {
        public FilterCreateDate()
        {
            MinExpression = items => items.Cdate >= MinValue;
            MaxExpression = items => items.Cdate <= MaxValue;
            EqualToExpression = items => items.Cdate == EqualToValue;
        }
    }
}
