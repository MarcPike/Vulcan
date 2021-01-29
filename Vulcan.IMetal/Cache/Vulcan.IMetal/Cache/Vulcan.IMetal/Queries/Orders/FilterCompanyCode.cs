using Vulcan.IMetal.Context.Orders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Orders
{
    public class FilterCompanyCode : EnhancedStringFilterBase<QueryOrderCapture>
    {
        public FilterCompanyCode()
        {
            HasSuggestions = true;

            ContainsExpression =
                order =>
                    order.Company.Code.Contains(
                        Value);
            InListExpression =
                order =>
                    Values.Contains(order.Company.Code);
            EqualToExpression =
                order => order.Company.Code == Value;
            StartsWithExpression =
                order =>
                    order.Company.Code
                        .StartsWith(Value);

        }
    }
}