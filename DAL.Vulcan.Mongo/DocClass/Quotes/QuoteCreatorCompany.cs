using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class QuoteCreatorCompany : QuoteCreator
    {
        public QuoteCreatorCompany(string coid, CrmUser crmUser, CompanyRef companyRef) : base(crmUser)
        {

            CrmQuote.Team = crmUser.ViewConfig.Team;

            CrmQuote.Coid = coid;
            var company = companyRef.AsCompany();

            CompanyResolver.Execute(company);

            CrmQuote.Company = company.AsCompanyRef();

            CrmQuote.Addresses = company.Addresses.ToList();

            CrmQuote.ShipToAddress = company.Addresses.Last();

            DefaultShippingToLastQuote(company);

            CompanyDefaults.ApplyCompanyDefaultsToQuote(CrmQuote);


        }

        private void DefaultShippingToLastQuote(Company company)
        {
            var lastQuoteMadeWithThisCompany = new RepositoryBase<CrmQuote>().AsQueryable()
                .Where(x => x.Company.Id == CrmQuote.Company.Id).OrderByDescending(x => x.CreateDateTime)
                .FirstOrDefault();
            if ((lastQuoteMadeWithThisCompany != null) && (lastQuoteMadeWithThisCompany.ShipToAddress != null))
            {
                var lastShippingAddress = CrmQuote.Addresses.FirstOrDefault(x =>
                    x.Id == lastQuoteMadeWithThisCompany.ShipToAddress.Id);

                if (lastShippingAddress != null)
                {
                    CrmQuote.ShipToAddress = lastShippingAddress;
                }
            }
        }
    }
}