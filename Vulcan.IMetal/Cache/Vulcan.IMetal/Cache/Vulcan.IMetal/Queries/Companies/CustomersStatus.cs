using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Companies
{
    public class CustomersStatus : EnhancedStringFilterBase<Company>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetCompanyContextForCoid(coid);
            return context.Company.Select(x => x.Status).Distinct().ToList();

        }

        public CustomersStatus()
        {
            HasSuggestions = true;

            ContainsExpression = items =>
                items.Status.ToLower().Contains(
                    Value.ToLower());

            InListExpression =
                items =>
                    Values.Contains(items.Status);
            EqualToExpression =
                items => items.Status.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Status.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}