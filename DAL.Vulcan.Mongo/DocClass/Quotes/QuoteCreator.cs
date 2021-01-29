using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class QuoteCreator
    {
        protected readonly CrmQuote CrmQuote;

        public QuoteCreator(CrmUser crmUser)
        {
            
            CrmQuote = new CrmQuote
            {
                QuoteId = KeyValues.GetNextQuoteId(),
                CreatedByUserId = crmUser.Id.ToString(),
                SalesPerson = crmUser.AsCrmUserRef()
            };
        }
        public CrmQuote GetQuote()
        {
            return CrmQuote;
        }
    }
}