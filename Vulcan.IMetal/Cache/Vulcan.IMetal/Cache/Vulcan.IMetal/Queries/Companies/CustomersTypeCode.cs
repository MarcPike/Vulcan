using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Companies
{
    public class CustomersTypeCode : EnhancedStringFilterBase<Company>
    {
        public override List<string> GetSuggestions(string coid)
        {
            var context = ContextFactory.GetCompanyContextForCoid(coid);
            return context.CompanyTypeCode.Select(x => x.Code).Distinct().ToList();
        }

        public CustomersTypeCode()
        {
            HasSuggestions = true;

            ContainsExpression = items =>
                items.CompanyTypeCode.Code.ToLower().Contains(
                    Value.ToLower());

            InListExpression =
                items =>
                    Values.Contains(items.CompanyTypeCode.Code);
            EqualToExpression =
                items => items.CompanyTypeCode.Code.ToLower() == Value.ToLower();
            StartsWithExpression =
                items =>
                    items.CompanyTypeCode.Code.ToLower()
                        .StartsWith(Value.ToLower());

        }
    }
}