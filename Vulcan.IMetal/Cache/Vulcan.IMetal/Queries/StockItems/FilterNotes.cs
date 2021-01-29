using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class FilterNotes: EnhancedStringFilterBase<Context.StockItems.StockItem>
    {
        public FilterNotes()
        {
            ContainsExpression =
                items =>
                    items.Note.ToLower().Contains(
                        Value.ToLower());
            InListExpression =
                items =>
                    Values.Contains(items.Note);
            EqualToExpression =
                items => items.Note.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Note.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}
