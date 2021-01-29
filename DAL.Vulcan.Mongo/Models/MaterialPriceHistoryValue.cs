using System;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Models
{
    public class MaterialPriceHistoryValue
    {
        public string Id;
        public int QuoteId;
        public CompanyRef Company;
        public ProspectRef Prospect;
        public PipelineStatus Status;

        public decimal CostPerKilogram;
        public decimal CostPerPound;
        public decimal CostPerInch;
        public decimal CostPerFoot;


        public decimal PricePerKilogram;
        public decimal PricePerPound;
        public decimal PricePerInch;
        public decimal PricePerFoot;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ReportDate;

        public MaterialPriceHistoryValue(CrmQuote quote, CrmQuoteItem item, PipelineStatus status, DateTime reportDate, decimal exchangeRate)
        {
            Id = quote.Id.ToString();
            QuoteId = quote.QuoteId;
            Company = quote.Company;
            Prospect = quote.Prospect;
            Status = status;

            if (Status == PipelineStatus.Won && item.IsLost)
            {
                Status = PipelineStatus.Loss;
            }

            CostPerKilogram = item.QuotePrice.MaterialCostValue.MaterialCostPerKg * exchangeRate;
            CostPerPound = item.QuotePrice.MaterialCostValue.MaterialCostPerPound * exchangeRate;
            CostPerFoot = item.QuotePrice.MaterialCostValue.MaterialCostPerFoot * exchangeRate;
            CostPerInch = item.QuotePrice.MaterialCostValue.MaterialCostPerInch * exchangeRate;

            PricePerKilogram = item.QuotePrice.MaterialPriceValue.PricePerKilogram * exchangeRate;
            PricePerPound = item.QuotePrice.MaterialPriceValue.PricePerPound * exchangeRate;
            PricePerFoot = item.QuotePrice.MaterialPriceValue.PricePerFoot * exchangeRate;
            PricePerInch = item.QuotePrice.MaterialPriceValue.PricePerInch * exchangeRate;

            ReportDate = reportDate;

        }


    }
}