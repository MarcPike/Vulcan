using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Companies
{
    public class CustomersShortName : EnhancedStringFilterBase<Company>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetCompanyContextForCoid(coid);
            return context.Company.Select(x => x.ShortName).Distinct().ToList();
        }

        public CustomersShortName()
        {
            HasSuggestions = true;

            ContainsExpression = items =>
                items.ShortName.ToLower().Contains(
                    Value.ToLower());

            InListExpression =
                items =>
                    Values.Contains(items.ShortName);
            EqualToExpression =
                items => items.ShortName.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.ShortName.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}
