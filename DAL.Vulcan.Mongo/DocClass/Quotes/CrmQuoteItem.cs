using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class CrmQuoteItem: BaseDocument
    {
        public static MongoRawQueryHelper<CrmQuoteItem> Helper = new MongoRawQueryHelper<CrmQuoteItem>();
        public int Index { get; set; }
        public string Coid { get; set; }

        public QuotePrice QuotePrice { get; set; }
        public CalculateQuotePriceModel CalculateQuotePriceModel { get; set; }

        public string OemType { get; set; } = string.Empty;
        public string PartSpecification { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string PoNumber { get; set; } = string.Empty;
        public string LeadTime { get; set; } = string.Empty;

        public bool ShowProductCodeOnQuote = true;

        public string SalesPersonNotes { get; set; } = string.Empty;
        public string CustomerNotes { get; set; } = string.Empty;

        public CompetitorRef LostTo { get; set; } = null;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? LostDate { get; set; } = null;

        public string LostReasonId { get; set; } = string.Empty;
        public string LostProductCode { get; set; } = string.Empty;
        public string LostComments { get; set; } = string.Empty;
        public bool IsLost => (LostDate != null);

        [JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)] // Mongo
        public CustomerUom CustomerUom
        {
            get
            {
                var result = CustomerUom.Inches;
                if (CalculateQuotePriceModel != null)
                {
                    result = CalculateQuotePriceModel.CustomerUom;
                }

                return result;
            }
        }

        public CrmUserRef SalesPerson { get; set; }
        public QuoteSource QuoteSource { get; set; } = QuoteSource.StockItem;

        public bool IsQuickQuoteItem => QuoteSource == QuoteSource.QuickQuoteItem;

        public bool IsMachinedPart => QuoteSource == QuoteSource.MachinedPart;
        public bool IsCrozCalc => QuoteSource == QuoteSource.CrozCalcItem;

        public QuickQuoteData QuickQuoteData { get; set; } = null;
        public QuoteMachinedPartModel MachinedPartModel { get; set; }
        public CrozCalcItem CrozCalcItem { get; set; }


        public ExternalItemCrmData ExternalItemCrmData { get; set; } = new ExternalItemCrmData();

        public string RequestedProductCode { get; set; } = string.Empty;

        public string BaseCurrency
        {
            get
            {
                var currency = "USD";
                if (String.IsNullOrEmpty(Coid))
                {
                    return currency;
                }
                if (Coid == "EUR")
                    currency = "GBP";
                if (Coid == "CAN")
                    currency = "CAD";
                return currency;
            }
        }


        public CrmQuoteItemRef AsQuoteItemRef()
        {
            return new CrmQuoteItemRef(this);
        }

        public void SetStockGradeForProducts()
        {

            QuotePrice.SetStockGradeForProducts();
            CalculateQuotePriceModel.SetStockGradeForProducts();

        }

        public CrmQuote GetQuote()
        {
            var rep = new RepositoryBase<CrmQuote>();
            return rep.AsQueryable().FirstOrDefault(x => x.Items.Any(i => i.Id == Id.ToString()));
        }

        public decimal TotalCost
        {
            get
            {
                if (IsQuickQuoteItem) return QuickQuoteData?.Cost ?? 0;
                if (IsMachinedPart) return MachinedPartModel?.TotalCost ?? 0;
                if (IsCrozCalc) return CrozCalcItem.TotalCost;
                return QuotePrice?.TotalCost ?? 0;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                if (IsQuickQuoteItem) return QuickQuoteData?.Price ?? 0;
                if (IsMachinedPart) return MachinedPartModel?.TotalPrice ?? 0;
                if (IsCrozCalc) return CrozCalcItem.TotalPrice;
                return QuotePrice?.FinalPrice ?? 0;
            }
        }

        public decimal AdditionalServiceCost
        {
            get
            {
                if ((QuotePrice == null) || (!QuotePrice.ProductionCosts.Any())) return 0;
                var additionalCostTotal = (decimal)0;

                foreach (var additionalCost in QuotePrice.ProductionCosts.Where(x => x.IsPriceBlended == false)
                    .ToList())
                {
                    additionalCostTotal += additionalCost.CostValues.Sum(x => x.InternalCost);
                }

                return additionalCostTotal;
            }
        }

        public decimal AdditionServicePrice
        {
            get
            {
                if ((QuotePrice == null) || (!QuotePrice.ProductionCosts.Any())) return 0;
                var additionalPriceTotal = (decimal)0;

                foreach (var additionalCost in QuotePrice.ProductionCosts.Where(x => x.IsPriceBlended == false)
                    .ToList())
                {
                    additionalPriceTotal += additionalCost.CostValues.Sum(x => x.ProductionCost);
                }

                return additionalPriceTotal;
            }
        }

        public decimal TotalKerfCost
        {
            get
            {
                if (QuotePrice == null && IsLost) return 0;
                return QuotePrice?.MaterialCostValue?.KerfTotalCost ?? 0;
            }
        }
    }
}