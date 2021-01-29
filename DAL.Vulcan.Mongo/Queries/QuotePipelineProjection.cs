using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Queries
{
    public class QuotePipelineProjection
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
        public DateTime? ExpireDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ReportDate { get; set; }

        public QuoteTotal QuoteTotal { get; set; }

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

        public List<QuotePipelineItemsProjection> Items = new List<QuotePipelineItemsProjection>();
        public int ValidityDays { get; set; }

        public QuotePipelineProjection(
            ObjectId id,
            int quoteId,
            PipelineStatus status,
            CompanyRef company,
            ProspectRef prospect,
            ContactRef contact,
            Address shipToAddress,
            string salesPersonId,
            string poNumber,
            string salesPerson,
            DateTime createDateTime,
            DateTime? submitDate,
            DateTime? wonDate,
            DateTime? lostDate,
            DateTime? expireDate,
            DateTime? reportDate,
            List<CrmQuoteItemRef> quoteItemRefs,
            string exportStatus,
            List<ExportAttempt> exportAttempts,
            string salesOrderId,
            string displayCurrency,
            bool bid,
            string rfqNumber,
            int validityDays,
            int star
        )
        {
            Id = id.ToString();
            QuoteId = quoteId;
            Status = status.ToString();
            Company = company;
            Prospect = prospect;
            Contact = contact;
            ShipToAddress = shipToAddress;
            SalesPersonId = salesPersonId;
            SalesPerson = salesPerson;
            SalesOrderId = salesOrderId;
            PoNumber = poNumber;
            CreateDateTime = createDateTime;
            SubmitDate = submitDate;
            WonDate = wonDate;
            LostDate = lostDate;
            ExpireDate = expireDate;
            ReportDate = reportDate;
            ExportStatus = exportStatus;
            ExportAttempts = exportAttempts;
            DisplayCurrency = displayCurrency;
            Bid = bid;
            RfqNumber = rfqNumber;
            ValidityDays = validityDays;
            Star = star;


            if (!quoteItemRefs.Any())
            {
                QuoteTotal = new QuoteTotal();
                return;
            }

            var quoteItems = quoteItemRefs.Select(x => x.AsQuoteItem()).ToList();
            foreach (var item in quoteItems.Where(x => x == null).ToList())
            {
                quoteItems.Remove(item);
            }

            var includeLostInTotal = (status == PipelineStatus.Loss);

            if (quoteItems.Count == 0)
            {
                QuoteTotal = new QuoteTotal();
            }
            else
            {
                QuoteTotal = new QuoteTotal(quoteItems, includeLostInTotal);
            }

            foreach (var crmQuoteItem in quoteItems)
            {
                if (crmQuoteItem != null)
                {
                    Items.Add(new QuotePipelineItemsProjection(crmQuoteItem));
                }
            }
        }

    }
}