using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Companies
{
    public class CustomersCode: EnhancedStringFilterBase<Company>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetCompanyContextForCoid(coid);
            return context.Company.Select(x => x.Code).Distinct().ToList();

        }

        public CustomersCode()
        {
            HasSuggestions = true;

            ContainsExpression = items =>
                items.Code.ToLower().Contains(
                    Value.ToLower());

            InListExpression =
                items =>
                    Values.Contains(items.Code);
            EqualToExpression =
                items => items.Code.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Code.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}