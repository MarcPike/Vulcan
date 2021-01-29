using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Orders;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Orders
{
    public partial class FilterOrderTypeCode : EnhancedStringFilterBase<QueryOrderCapture>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetOrdersContextForCoid(coid);
            return context.SalesType.OrderBy(x => x.Code).Select(x => x.Code).Distinct().ToList();
        }

        public Dictionary<int, string> GetCodeDictionary(string coid)
        {
            var context = ContextFactory.GetOrdersContextForCoid(coid);
            return context.SalesType
                .ToDictionary(t => t.Id, t => t.Code);
        }

        public FilterOrderTypeCode()
        {
            HasSuggestions = true;

            ContainsExpression =
                order =>
                    order.SalesType.Code.Contains(
                        Value);
            InListExpression =
                order =>
                    Values.Contains(order.SalesType.Code);
            EqualToExpression =
                order => order.SalesType.Code == Value;
            StartsWithExpression =
                order =>
                    order.SalesType.Code
                        .StartsWith(Value);

        }
    }
}