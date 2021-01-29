using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.Queries.Companies;

namespace Vulcan.IMetal.Helpers
{
    public class HelperCompanyPaymentTerms : BaseHelper, IHelperCompanyPaymentTerms
    {

        public PaymentTermModel GetPaymentTermsForCompany(string coid, int companyId)
        {
            var query = new QueryCompany(coid);
            var term = query.GetTermForId(companyId);

            if (term == null) return null;

            return new PaymentTermModel()
            {
                Id = term.Id,
                Code = term.Code,
                Description = term.Description,
                Status = term.Status,
                DueDays = term.DueDay ?? 0
            };

        }

    }
}
