using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Companies
{
    public class CustomersName : EnhancedStringFilterBase<Company>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetCompanyContextForCoid(coid);
            return context.Company.Select(x=>x.Name).Distinct().ToList();

        }

        public CustomersName()
        {
            HasSuggestions = true;

            ContainsExpression = items =>
                items.Name.ToLower().Contains(
                    Value.ToLower());

            InListExpression =
                items =>
                    Values.Contains(items.Name);
            EqualToExpression =
                items => items.Name.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.Name.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}