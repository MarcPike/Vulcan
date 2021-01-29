using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class QuoteCreatorProspect : QuoteCreator
    {
        public QuoteCreatorProspect(CrmUser crmUser, ProspectRef prospectRef) : base(crmUser)
        {
            CrmQuote.Prospect = prospectRef;
        }
    }
}