using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class FilterAvailablePieces : EnhancedRangeFilterBase<int, PurchaseOrderItem>
    {
        public FilterAvailablePieces()
        {
            MinExpression = items => (items.OrderedPiece ?? 0) - (items.AllocatedPiece ?? 0) >= MinValue;
            MaxExpression = items => (items.OrderedPiece ?? 0) - (items.AllocatedPiece ?? 0) <= MaxValue;
            EqualToExpression = items => (items.OrderedPiece ?? 0) - (items.AllocatedPiece ?? 0) == EqualToValue;
        }
    }
}