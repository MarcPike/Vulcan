using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Helpers
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
