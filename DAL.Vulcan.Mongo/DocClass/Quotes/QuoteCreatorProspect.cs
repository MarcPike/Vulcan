using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class QuoteCreatorProspect : QuoteCreator
    {
        public QuoteCreatorProspect(CrmUser crmUser, ProspectRef prospectRef) : base(crmUser)
        {
            CrmQuote.Prospect = prospectRef;
        }
    }
}