using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace Vulcan.SVC.WebApi.Core.Models
{
    public class QuoteExportAttemptResultsModel
    {
        public List<QuoteExportAttemptDetails> ExportAttempts { get; set; } = new List<QuoteExportAttemptDetails>();

        public QuoteExportAttemptResultsModel()
        {
        }

        public QuoteExportAttemptResultsModel(CrmQuote quote)
        {
            foreach (var exportAttempt in quote.ExportAttempts.OrderByDescending(x=>x.ExecutionDate).ToList())
            {
                ExportAttempts.Add(new QuoteExportAttemptDetails(exportAttempt));
            }
        }
    }
}
