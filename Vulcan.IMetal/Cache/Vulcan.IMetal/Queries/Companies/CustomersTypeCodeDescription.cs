using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Companies
{
    public class CustomersTypeCodeDescription : EnhancedStringFilterBase<Company>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetCompanyContextForCoid(coid);
            return context.CompanyTypeCode.Select(x => x.Description).Distinct().ToList();
        }

        public CustomersTypeCodeDescription()
        {
            HasSuggestions = true;

            ContainsExpression = items =>
                items.CompanyTypeCode.Description.ToLower().Contains(
                    Value.ToLower());

            InListExpression =
                items =>
                    Values.Contains(items.CompanyTypeCode.Description);
            EqualToExpression =
                items => items.CompanyTypeCode.Description.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.CompanyTypeCode.Description.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}