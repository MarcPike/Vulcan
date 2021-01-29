using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Companies
{
    public class CustomersStatusDescription : EnhancedStringFilterBase<Company>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetCompanyContextForCoid(coid);
            return context.Company.Select(x => x.CompanyStatusCode.Description).Distinct().ToList();

        }

        public CustomersStatusDescription()
        {
            HasSuggestions = true;

            ContainsExpression = items =>
                items.CompanyStatusCode.Description.ToLower().Contains(
                    Value.ToLower());

            InListExpression =
                items =>
                    Values.Contains(items.CompanyStatusCode.Description);
            EqualToExpression =
                items => items.CompanyStatusCode.Description.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.CompanyStatusCode.Description.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}