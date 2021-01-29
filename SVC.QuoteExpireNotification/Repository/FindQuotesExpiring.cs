using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;

namespace SVC.QuoteExpireNotification.Repository
{
    public class FindQuotesExpiring
    {
        public DateTime ExecutedOn { get; set; } 
        public List<CrmQuote> ExpiringQuotes { get; set; } = new List<CrmQuote>();

        public FindQuotesExpiring()
        {
            ExecutedOn = DateTime.Now.Date;
        }

        public FindQuotesExpiring(DateTime dateOf)
        {
            ExecutedOn = dateOf;
        }

        public void Execute()
        {

            var rep = new RepositoryBase<CrmQuote>();
            var submittedQuotes = rep.AsQueryable().Where(x => x.Status == PipelineStatus.Submitted && x.SubmitDate != null).ToList();
            foreach (var quote in submittedQuotes)
            {
                var submitDate = quote.SubmitDate ?? DateTime.Now;
                if (submitDate.AddDays(quote.ValidityDays).Date == ExecutedOn.Date.AddDays(1))
                {
                    ExpiringQuotes.Add(quote);
                }
            }
        }

    }
}
