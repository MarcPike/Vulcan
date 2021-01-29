using System;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.Vulcan.Mongo.Models
{
    public class QuoteItemModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public int Index { get; set; }
        public string Coid { get; set; }
        public CrmUserRef SalesPerson { get; set; }
        public string OemType { get; set; }
        public string PartSpecification { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string PoNumber { get; set; } = string.Empty;
        public string LeadTime { get; set; }

        public bool ShowProductCodeOnQuote { get; set; } = true;

        public string SalesPersonNotes { get; set; } = string.Empty;
        public string CustomerNotes { get; set; } = string.Empty;

        public List<string> SearchTags { get; set; } = new List<string>();

        public GuidList<Note> Notes { get; set; } = new GuidList<Note>();

        [JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)] // Mongo
        public QuoteSource QuoteSource { get; set; }

        public QuotePriceModel QuotePriceModel { get; set; }
        public CalculateQuotePriceModel CalculateQuotePriceModel { get; set; }

        public QuoteMachinedPartModel MachinedPartModel { get; set; }

        public QuickQuoteData QuickQuoteData { get; set; }

        public CrozCalcItemModel CrozCalcItem { get; set; }

        [JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)] // Mongo
        public CustomerUom CustomerUom { get; set; }

        public string LostReasonId { get; set; } = string.Empty;
        public CompetitorRef LostTo { get; set; }
        public DateTime? LostDate { get; set; }
        public string LostProductCode { get; set; } = string.Empty;
        public string LostComments { get; set; } = string.Empty;

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


        public bool IsLost { get; set; }

        public bool IsQuickQuoteItem { get; set; }

        public bool IsMachinedPart { get; set; } 

        public bool IsCrozCalc { get; set; }

        public List<FileAttachmentModel> FileAttachments { get; set; } = new List<FileAttachmentModel>();

        public ItemSummaryViewModel ItemSummaryViewModel { get; set; }

        public string RequestedProductCode { get; set; } = string.Empty;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedDateTime { get; set; }

        public bool IsDirty { get; set; } = true;

        public QuoteItemModel()
        {
        }

        public QuoteItemModel(string application, string userId, CrmQuoteItem item)
        {
            Application = application;
            UserId = userId;
            Id = item.Id.ToString();
            Index = item.Index;
            Coid = item.Coid;
            PartNumber = item.PartNumber;
            PartSpecification = item.PartSpecification;
            PoNumber = item.PoNumber;
            SearchTags = item.SearchTags;
            QuotePriceModel = (item.QuotePrice == null) ? null : new QuotePriceModel(item.CalculateQuotePriceModel.Application, item.CalculateQuotePriceModel.UserId, item.QuotePrice);
            CalculateQuotePriceModel = item.CalculateQuotePriceModel;
            MachinedPartModel = item.MachinedPartModel;
            SalesPerson = item.SalesPerson;
            SalesPersonNotes = item.SalesPersonNotes;
            CustomerNotes = item.CustomerNotes;
            LeadTime = item.LeadTime;
            CustomerUom = item.CustomerUom;
            LostReasonId = item.LostReasonId;
            LostDate = item.LostDate;
            LostTo = item.LostTo;
            IsLost = item.IsLost;
            
            OemType = (item.OemType == string.Empty) ? "Unknown" : item.OemType;
            CrozCalcItem = (item.CrozCalcItem != null) ? new CrozCalcItemModel(application, userId, item.CrozCalcItem): null;
            
            IsQuickQuoteItem = item.IsQuickQuoteItem;
            IsMachinedPart = item.IsMachinedPart;
            IsCrozCalc = item.IsCrozCalc;

            QuoteSource = item.QuoteSource;

            LostComments = item.LostComments;
            if (IsMachinedPart)
            {
                CustomerUom = CustomerUom.PerPiece;
            }

            FileAttachments = Base.FileAttachment.FileAttachmentsVulcan.GetAllAttachmentsForDocument(item).Select(x => new FileAttachmentModel(x)).ToList() ?? new List<FileAttachmentModel>();
            QuickQuoteData = item.QuickQuoteData;
            CreateDateTime = item.CreateDateTime;
            ModifiedDateTime = item.ModifiedDateTime;

            RequestedProductCode = item.RequestedProductCode;

            ShowProductCodeOnQuote = item.ShowProductCodeOnQuote;

            ItemSummaryViewModel = new ItemSummaryViewModel(item);
            IsDirty = false;

            if (QuotePriceModel != null && QuotePriceModel.UserId != userId)
            {
                QuotePriceModel.UserId = userId;
            }

            if (CalculateQuotePriceModel != null && CalculateQuotePriceModel.UserId != userId)
            {
                CalculateQuotePriceModel.UserId = userId;
            }

        }

    }
}
