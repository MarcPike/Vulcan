using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class QuoteQueryCompanyOptions
    {
        public List<CompanyRef> Companies { get; set; } = new List<CompanyRef>();
        public List<ContactRef> Contacts { get; set; } = new List<ContactRef>();

        public bool IsUsed => (Companies.Count > 0) || (Contacts.Count > 0);


    }
}