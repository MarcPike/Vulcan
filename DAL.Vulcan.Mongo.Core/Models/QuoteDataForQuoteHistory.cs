using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class QuoteDataForQuoteHistory
    {
        public int QuoteId { get; set; }
        public DateTime? ReportDate { get; set; }
        public PipelineStatus Status { get; set; }
        public List<CrmQuoteItemRef> Items { get; set; }
        public List<QuickQuoteItemRef> QuickQuoteItems { get; set; }
        public CrmUserRef SalesPerson { get; set; }
        public CompanyRef Company { get; set; }

        public QuoteDataForQuoteHistory(int quoteId, DateTime? reportDate, PipelineStatus status,
            List<CrmQuoteItemRef> items, List<QuickQuoteItemRef> quickQuoteItems, CrmUserRef salesPerson, CompanyRef company)
        {
            QuoteId = quoteId;
            ReportDate = reportDate;
            Status = status;
            Items = items;
            QuickQuoteItems = quickQuoteItems;
            SalesPerson = salesPerson;
            Company = company;
        }

    }
}