using DAL.Vulcan.Mongo.DocClass.Quotes;
using System.Collections.Generic;
using System.Linq;

namespace Vulcan.iMetal.Quote.Export.Model
{
    public class QuoteExportAttempResultsModel
    {
        public List<QuoteExportAttemptDetails> ExportAttempts { get; set; } = new List<QuoteExportAttemptDetails>();

        public QuoteExportAttempResultsModel()
        {
        }

        public QuoteExportAttempResultsModel(CrmQuote quote)
        {
            foreach (var exportAttempt in quote.ExportAttempts.OrderByDescending(x=>x.ExecutionDate).ToList())
            {
                ExportAttempts.Add(new QuoteExportAttemptDetails(exportAttempt));
            }
        }
    }
}
