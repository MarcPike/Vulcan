using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Marketing.ChartModels
{
    public class TestGetQuoteModel
    {
        public int QuoteId { get; set; }
        public PipelineStatus Status { get; set; }
        public DateTime? ReportDate { get; set; }
        public TestGetQuoteModel()
        {
        }

        public TestGetQuoteModel(CrmQuote quote)
        {
            QuoteId = quote.QuoteId;
            Status = quote.Status;
            ReportDate = quote.ReportDate;
        }
    }
}
