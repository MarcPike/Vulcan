using Vulcan.IMetal.Context.Orders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Orders
{
    public class FilterOrderNumber : EnhancedStringFilterBase<QueryOrderCapture>
    {


        public FilterOrderNumber()
        {
            HasSuggestions = false;

            ContainsExpression =
                order =>
                    order.SalesHeader.Number.ToString().Contains(
                        Value);
            InListExpression =
                order =>
                    Values.Contains(order.SalesHeader.Number.ToString());
            EqualToExpression =
                order => order.SalesHeader.Number.ToString() == Value;
            StartsWithExpression =
                order =>
                    order.SalesHeader.Number.ToString()
                        .StartsWith(Value);

        }

    }
}