using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public class QuoteQueryCompanyOptionsHelper
    {
        public List<CompanyRef> Companies = new List<CompanyRef>();
        public List<ContactRef> Contacts = new List<ContactRef>();

        public QuoteQueryCompanyOptionsHelper(TeamRef teamRef)
        {
            var repQuotes = new RepositoryBase<CrmQuote>();

            var quotes = repQuotes.AsQueryable().Where(x => x.Team.Id == teamRef.Id).ToList();
            foreach (var company in quotes.Select(x => x.Company).OrderBy(x => x.Name))
            {
                if (Companies.All(x => x.Id != company.Id))
                {
                    Companies.Add(company);
                }
            }

            foreach (var contact in quotes.Where(x=>x.Contact != null).Select(x => x.Contact).OrderBy(x => x.LastName))
            {
                if (Contacts.All(x => x.Id != contact.Id))
                {
                    Contacts.Add(contact);
                }
            }
        }

    }
}
