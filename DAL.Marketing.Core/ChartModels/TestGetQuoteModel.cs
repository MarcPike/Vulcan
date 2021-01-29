using System;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Marketing.Core.ChartModels
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
