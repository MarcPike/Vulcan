using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Models
{
    public class QuoteMiniModel
    {
        public string Id { get; set; }
        public int QuoteId { get; set; }
        public string Status { get; set; }
        public CompanyRef Company { get; set; }
        public ProspectRef Prospect { get; set; }
        public ContactRef Contact { get; set; }
        public Address ShipToAddress { get; set; }
        public string SalesPersonId { get; set; }
        public string PoNumber { get; set; }
        public string SalesPerson { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? SubmitDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? WonDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? LostDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ReportDate { get; set; }

        public QuoteTotal QuoteTotal { get; set; }
        //public QuoteTotal QuoteTotalFast { get; set; }

        public string ExportStatus { get; set; }

        public List<ExportAttempt> ExportAttempts { get; set; }

        public string SalesOrderId { get; set; }

        public string DisplayCurrency { get; set; }

        public bool Bid { get; set; }

        public int Star { get; set; }

        public string ExportErrors
        {
            get
            {
                var lastExportAttempt = ExportAttempts.LastOrDefault();
                if (lastExportAttempt == null) return "";
                return lastExportAttempt.Errors;
            }
        }

        public string CompanyName
        {
            get
            {
                if (IsProspect) return Prospect.Name;
                return Company.Name;
            }
        }

        public bool IsProspect => (Company == null) && (Prospect != null);

        public string RfqNumber { get; set; }

        public List<QuoteMiniItemModel> Items { get; set; } = new List<QuoteMiniItemModel>();

        public QuoteMiniModel()
        {
        }

        public QuoteMiniModel(CrmQuote quote, PipelineStatus status)
        {
            //quote.SetReportDate();

            Id = quote.Id.ToString();
            Company = quote.Company;
            QuoteId = quote.QuoteId;
            SalesPersonId = quote.SalesPerson.Id;
            SalesPerson = quote.SalesPerson.FullName;
            CreateDateTime = quote.CreateDateTime;
            SubmitDate = quote.SubmitDate;
            WonDate = quote.WonDate;
            LostDate = quote.LostDate;
            Contact = quote.Contact;
            Status = quote.Status.ToString();
            RfqNumber = quote.RfqNumber;
            Contact = quote.Contact;
            Prospect = quote.Prospect;
            ExportStatus = quote.ExportStatus.ToString();
            ExportAttempts = quote.ExportAttempts.OrderByDescending(x => x.ExecutionDate).ToList();
            SalesOrderId = quote.ExternalOrderId;
            ReportDate = quote.ReportDate;
            DisplayCurrency = quote.DisplayCurrency;
            ShipToAddress = quote.ShipToAddress;
            PoNumber = quote.PoNumber;
            Bid = quote.Bid;
            Star = quote.Star;

            //QuoteTotal = new QuoteTotal(quote.Items.Select(x => new QuoteItemModel(x.AsQuoteItem())).ToList(), quote.QuickQuoteItems, true);
            var includeLostInTotal = (status == PipelineStatus.Loss);

            var quoteItems = quote.Items.Select(x => x.AsQuoteItem()).ToList();

            QuoteTotal = new QuoteTotal(quoteItems, includeLostInTotal);

            //if (quote.QuoteId == 13784)
            //{
            //}

            foreach (var crmQuoteItem in quoteItems)
            {
                if (crmQuoteItem != null)
                {
                    Items.Add(new QuoteMiniItemModel(crmQuoteItem));
                }
            }

            //foreach (var quickQuoteItem in quote.QuickQuoteItems.Select(x => x.AsQuickQuoteItem()))
            //{
            //    Items.Add(new QuoteMiniItemModel()
            //    {
            //        Id = quickQuoteItem.Id.ToString(),
            //        FinishProduct = quickQuoteItem.FinishedProduct,
            //        FinishQuantity = quickQuoteItem.RequiredQuantity,
            //        IsLost = quickQuoteItem.IsLost,
            //        Regret = quickQuoteItem.Regret
            //    });
            //}

        }

    }
}
