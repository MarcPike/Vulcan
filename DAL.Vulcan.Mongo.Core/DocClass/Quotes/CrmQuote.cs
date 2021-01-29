using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Queries;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Address = DAL.Vulcan.Mongo.Core.DocClass.Locations.Address;


namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class CrmQuote: BaseDocument
    {
        public static MongoRawQueryHelper<CrmQuote> Helper = new MongoRawQueryHelper<CrmQuote>();
        public string Coid { get; set; }
        public int QuoteId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ReceivedRFQ { get; set; } 

        public Guid QuoteLinkId { get; set; } = Guid.Empty;

        public QuoteLinkType QuoteLinkType { get; set; } = QuoteLinkType.None;

        public List<Revision> Revisions { get; set; } = new List<Revision>();

        public Revision CurrentRevision => Revisions.OrderByDescending(x => x.Id).FirstOrDefault();

        public TeamRef Team { get; set; } = null;

        public int RevisionNumber => CurrentRevision?.Id ?? 0;

        public bool IsProspect => ((Company == null) && (Prospect != null));

        public PipelineStatus Status { get; set; } = PipelineStatus.Draft;
        public ExportStatus ExportStatus { get; set; } = ExportStatus.NotExported;
        public CrmUserRef ExportRequestedBy { get; set; } = null;
        public List<ExportAttempt> ExportAttempts { get; set; } = new List<ExportAttempt>();
        public ExternalHeaderCrmData ExternalHeaderCrmData { get; set; } = new ExternalHeaderCrmData();
        public string ExternalOrderId { get; set; } = string.Empty;

        public string DisplayCurrency { get; set; } = string.Empty;

        public bool Bid { get; set; } = false;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExpireDate { get; set; } = DateTime.Now.Date.AddDays(30);

        //public Note SpecialInstructions { get; set; } = new Note();
        //public Note ExternalInstructions { get; set; } = new Note();
        //public Note InternalInstructions { get; set; } = new Note();
        //public Note GeneralNotes { get; set; } = new Note();

        public string SalesPersonNotes { get; set; }
        public string CustomerNotes { get; set; }

        public string PoNumber { get; set; } = String.Empty;

        public int PdfRowsPerPage { get; set; } = 6;

        public CompanyRef Company { get; set; }
        public ProspectRef Prospect { get; set; }
        public Address ShipToAddress { get; set; }
        public List<Address> Addresses { get; set; }
        public CrmUserRef SalesPerson { get; set; }
        public ContactRef Contact { get; set; }

        public CompetitorRef LostTo { get; set; } = null;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? LostDate { get; set; } = null;

        public string LostReasonId { get; set; } = string.Empty;

        public string LostComments { get; set; } = string.Empty;

        public bool IsLost => (LostDate != null);

        public List<CrmQuoteItemRef> Items { get; set; } = new List<CrmQuoteItemRef>();

        public List<QuickQuoteItemRef> QuickQuoteItems { get; set; } = new List<QuickQuoteItemRef>();

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? SubmitDate { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //public DateTime DeliveryDate { get; set; }

        public string PaymentTerm { get; set; } = "30 Days From Invoice Date";
        public string FreightTerm { get; set; } = "FCA Free Carrier";
        public string Validity { get; set; } = "7 Days";
        public int ValidityDays { get; set; } = 7;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ValidityDate
        {
            get
            {
                return (SubmitDate != null) ? (DateTime?)SubmitDate.Value.AddDays(ValidityDays) : null;
            }
        }

        public string RfqNumber { get; set; } = string.Empty;

        public string SalesGroupCode { get; set; } = string.Empty;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? WonDate { get; set; } = null;

        public int Star { get; set; } = 0;

        public QuoteRef AsQuoteRef()
        {
            return new QuoteRef(this);
        }

        private int NextItemIndex()
        {
            if (Items.Count == 0) return 1;

            return Items.Max(x => x.Index) + 1;
        }

        public static List<CrmQuote> CheckForExpiredQuotes(List<CrmQuote> quotes)
        {
            var quoteRep = new RepositoryBase<CrmQuote>();
            foreach (var crmQuote in quotes)
            {
                if (crmQuote.Status == PipelineStatus.Submitted && (crmQuote.SubmitDate != null))
                {
                    if (crmQuote.SubmitDate.Value.AddDays(crmQuote.ValidityDays+1).Date < DateTime.Now.Date)
                    {
                        crmQuote.Status = PipelineStatus.Expired;
                        crmQuote.ExpireDate = DateTime.Now.Date;
                        quoteRep.Upsert(crmQuote);
                    }
                }
            }

            return quotes;
        }

        public void CheckIfExpired()
        {
            if (SubmitDate == null) return;
            if (SubmitDate.Value.AddDays(ValidityDays + 1).Date < DateTime.Now.Date)
            {
                Status = PipelineStatus.Expired;
                ExpireDate = DateTime.Now.Date;
            }
        }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ReportDate { get; set; }

        public void SetReportDate()

        {
            var reportDate = ModifiedDateTime;
            if ((CurrentRevision != null) && (Status != PipelineStatus.Draft))
            {
                reportDate = CurrentRevision.RevisionDate;
            }
            else if (Status == PipelineStatus.Draft)
            {
                reportDate = CreateDateTime;
            }
            else if (Status == PipelineStatus.Submitted)
            {
                reportDate = SubmitDate ?? ModifiedDateTime;
            }
            else if (Status == PipelineStatus.Won)
            {
                reportDate = WonDate ?? ModifiedDateTime;
            }

            if (Status == PipelineStatus.Loss)
            {
                reportDate = LostDate ?? ModifiedDateTime;
            }

            if (Status == PipelineStatus.Expired)
            {
                reportDate = ExpireDate;
            }

            ReportDate = reportDate;
        }

        public decimal GetAverageMargin()
        {
            var items = Items.Select(x => x.AsQuoteItem());

            items = items.Where(x=>x.CalculateQuotePriceModel != null);

            if (!items.Any()) return 0;

            return items.Average(x=>x.CalculateQuotePriceModel.MaterialPriceValue?.Margin ?? 0) * 100;
        }

        public static CrmQuoteItem GetQuoteItemFor(int quoteId, int index)
        {
            try
            {
                return Helper.Find(x => x.QuoteId == quoteId).FirstOrDefault()?.Items[index]?.AsQuoteItem();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    //[BsonIgnoreExtraElements]
    //public class CrmQuoteRef : ReferenceObject<CrmQuote>
    //{
    //    public int QuoteId { get; set; }
    //    public CompanyRef Company { get; set; }
    //    public PipelineStatus Status { get; set; }

    //    public CrmQuote AsQuote()
    //    {
    //        return ToBaseDocument();
    //    }
    //}

}
