using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterAvailablePieces : EnhancedRangeFilterBase<int, Context.StockItems.StockItem>
    {
        public FilterAvailablePieces()
        {
            MinExpression = items => (items.PhysicalPiece?? 0) - (items.AllocatedPiece ?? 0) >= MinValue;
            MaxExpression = items => (items.PhysicalPiece?? 0) - (items.AllocatedPiece ?? 0) <= MaxValue;
            EqualToExpression = items => (items.PhysicalPiece?? 0) - (items.AllocatedPiece ?? 0) == EqualToValue;
        }
    }
}