using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class QuoteModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Coid { get; set; }
        public string Id { get; set; }
        public int QuoteId { get; set; }
        public string QuoteLinkId { get; set; }

        public int RevisionNumber { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ReceivedRFQ { get; set; }


        public List<Revision> Revisions { get; set; } = new List<Revision>();
        public Revision CurrentRevision => Revisions.OrderByDescending(x => x.Id).FirstOrDefault();

        public List<string> StatusList = Enum.GetNames(typeof(PipelineStatus)).ToList();

        public string ExportStatus { get; set; }

        public int PdfRowsPerPage { get; set; }

        public string Status { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExpireDate { get; set; }

        //public Note SpecialInstructions { get; set; }
        //public Note ExternalInstructions { get; set; }
        //public Note InternalInstructions { get; set; }
        //public Note GeneralNotes { get; set; }
        public string PoNumber { get; set; }

        public bool IsProspect => ((Company == null) && (Prospect != null));
        public bool IsCompany => (Company != null);

        public string OrderClassificationCode { get; set; } = string.Empty;
        public string OrderClassificationDescription { get; set; } = string.Empty;

        public string SalesPersonNotes { get; set; } = string.Empty;
        public string CustomerNotes { get; set; } = string.Empty;

        public CompanyRef Company { get; set; }
        public ProspectRef Prospect { get; set; }
        public Address ShipToAddress { get; set; }
        public List<Address> Addresses { get; set; }

        public bool Bid { get; set; }
        public int Star { get; set; }

        public string DisplayCurrency { get; set; } 
        public string OldCurrency { get; set; }


        public CrmUserRef SalesPerson { get; set; }
        public ContactRef Contact { get; set; }

        public List<ContactRef> Contacts { get; set; } = new List<ContactRef>();

        public CompetitorRef LostTo { get; set; }
        public string LostReasonId { get; set; } = string.Empty;

        public string LostReason
        {
            get
            {
                if (LostReasonId != String.Empty)
                {
                    return DocClass.CRM.LostReason.Helper.FindById(LostReasonId).Reason ?? string.Empty;
                }
                else
                {
                    return string.Empty;
                }
                
            }
        }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? LostDate { get; set; }
        public string LostComments { get; set; } = string.Empty;

        public List<string> SearchTags { get; set; }

        public List<QuoteItemModel> Items { get; set; }

        //public List<QuickQuoteItemModel> QuickQuoteItems { get; set; }

        public QuoteTotal QuoteTotal { get; set; }

        //public List<ExchangeRate> ExchangeRates { get; set; } = ExchangeRate.GetRateList();

        public string SalesGroupCode { get; set; }

        //public DateTime DeliveryDate { get; set; }

        public string PaymentTerm { get; set; } 
        public string FreightTerm { get; set; } 

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? SubmitDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedDateTime { get; set; }

        public string Validity { get; set; }
        public int ValidityDays { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ValidityDate { get; set; }
        public string RfqNumber { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? WonDate { get; set; } = null;

        public List<FileAttachmentModel> FileAttachments { get; set; } = new List<FileAttachmentModel>();

        public bool IsDirty { get; set; } = true;

        public List<QuoteRef> LinkedQuotes
        {
            get
            {
                var result = new List<QuoteRef>();

                if (QuoteLinkId != Guid.Empty.ToString())
                {
                    var quoteLinkId = Guid.Parse(QuoteLinkId);
                    var otherQuotes = new RepositoryBase<CrmQuote>().AsQueryable()
                        .Where(x => x.QuoteLinkId == quoteLinkId).ToList();
                    otherQuotes = otherQuotes.Where(x => x.Id.ToString() != this.Id).ToList();
                    result.AddRange(otherQuotes.Select(x=>x.AsQuoteRef()));
                }

                return result;
            }
        }

        public QuoteModel()
        {
        }

        public QuoteModel(string application, string userId, CrmQuote quote)
        {
            try
            {
                if (quote.Company != null)
                {

                    var company = quote.Company.AsCompany();
                    //CompanyResolver.CompanyRefresher.RefreshCompanyOrderClassification(company);
                    //quote.ShipToAddress = CompanyResolver.SaveNewAddressIfNecessary(company, quote.ShipToAddress);

                    Addresses = company.Addresses;
                    Contacts = company.Contacts;
                    OrderClassificationCode = company.OrderClassificationCode;
                    OrderClassificationDescription = company.OrderClassificationDescription;
                }
                else
                {
                    Addresses = quote.Addresses;
                    var prospect = quote.Prospect?.AsProspect();
                    if (prospect != null)
                    {
                        Contacts = prospect.Contacts;
                    }

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Problem occurred updating information from iMetal: {e.Message}");
            }


            Id = quote.Id.ToString();
            Application = application;
            UserId = userId;
            Coid = quote.Coid;
            QuoteId = quote.QuoteId;
            QuoteLinkId = quote.QuoteLinkId.ToString();
            Revisions = quote.Revisions;
            RevisionNumber = quote.RevisionNumber;
            Company = quote.Company;
            Prospect = quote.Prospect;
            ShipToAddress = quote.ShipToAddress;
            SalesPerson = quote.SalesPerson;
            Contact = quote.Contact;
            LostTo = quote.LostTo;
            LostDate = quote.LostDate;
            LostReasonId = quote.LostReasonId;
            SearchTags = quote.SearchTags;
            Bid = quote.Bid;
            Star = quote.Star;
            Items = quote.Items.Select(x => new QuoteItemModel(application, userId, x.AsQuoteItem())).OrderBy(x=>x.Index).ToList();
            //SpecialInstructions = quote.SpecialInstructions;
            //ExternalInstructions = quote.ExternalInstructions;
            //InternalInstructions = quote.InternalInstructions;
            //GeneralNotes = quote.GeneralNotes;
            PdfRowsPerPage = quote.PdfRowsPerPage;

            SalesPersonNotes = quote.SalesPersonNotes;
            CustomerNotes = quote.CustomerNotes;
            LostComments = quote.LostComments;
            Status = quote.Status.ToString();
            PoNumber = quote.PoNumber ?? string.Empty;
            SubmitDate = quote.SubmitDate;
            //DeliveryDate = quote.DeliveryDate;
            PaymentTerm = quote.PaymentTerm;
            FreightTerm = quote.FreightTerm;
            Validity = quote.Validity;
            ValidityDays = quote.ValidityDays;
            ValidityDate = quote.ValidityDate;
            RfqNumber = quote.RfqNumber;
            ReceivedRFQ = quote.ReceivedRFQ;
            SalesGroupCode = quote.SalesGroupCode;

            WonDate = quote.WonDate;

            CreateDateTime = quote.CreateDateTime;
            ModifiedDateTime = quote.ModifiedDateTime;

            DisplayCurrency = quote.DisplayCurrency;
            OldCurrency = quote.DisplayCurrency;

            ExportStatus = quote.ExportStatus.ToString();

            ExpireDate = quote.ExpireDate;

            QuoteTotal = new QuoteTotal(quote.Items.Select(x => x.AsQuoteItem()).ToList(), false);

            FileAttachments = Base.Core.FileAttachment.FileAttachmentsVulcan.GetAllAttachmentsForDocument(quote).Select(x=> new FileAttachmentModel(x)).ToList() ?? new List<FileAttachmentModel>();
            IsDirty = !(new RepositoryBase<CrmQuote>().AsQueryable().Any(x=>x.Id == quote.Id));

        }

    }
}