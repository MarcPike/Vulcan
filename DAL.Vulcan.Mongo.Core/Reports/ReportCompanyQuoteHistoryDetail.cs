using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Helpers;

namespace DAL.Vulcan.Mongo.Core.Reports
{
    public class ReportCompanyQuoteHistoryDetail
    {
        public string CompanyName { get; set; }
        public int QuoteId { get; set; }
        public int ItemNumber { get; set; }
        public string Status { get; set; }
        public DateTime QuoteDate { get; set; }
        public string StartingProductCode { get; set; }
        public string FinishProductCode { get; set; }
        public string PartSpecification { get; set; }
        public bool IsQuickQuoteItem { get; set; } = false;
        public bool IsRegret { get; set; } = false;
        public int Pieces { get; set; }
        public decimal TotalInches { get; set; }
        public string CustomerUom { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPounds { get; set; }
        public decimal SellPricePerPound { get; set; }

        private const string UNKNOWN = "<unknown>";

        public static List<ReportCompanyQuoteHistoryDetail> ScrapeFromQuote(CrmQuote quote, string displayCurrency)
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            var result = new List<ReportCompanyQuoteHistoryDetail>();
            
            foreach (var crmQuoteItemRef in quote.Items.ToList().OrderBy(x=>x.Index))
            {
                if (crmQuoteItemRef.IsQuickQuoteItem) continue;
                var quoteCurrency = quote.DisplayCurrency;

                var onItem = new ReportCompanyQuoteHistoryDetail();
                var item = crmQuoteItemRef.AsQuoteItem();
                onItem.CompanyName = quote.Company.Name;
                onItem.QuoteId = quote.QuoteId;
                onItem.ItemNumber = item.Index;
                onItem.Status = quote.Status.ToString();
                onItem.QuoteDate = quote.ReportDate ?? quote.CreateDateTime;

                //if (item.IsQuickQuoteItem)
                //{

                //    //onItem.StartingProductCode = UNKNOWN;
                //    //onItem.FinishProductCode = item.QuickQuoteData.FinishedProduct?.ProductCode ?? UNKNOWN;
                //    //onItem.PartSpecification = item.PartSpecification ?? UNKNOWN;
                //    //onItem.Pieces = item.QuickQuoteData.RequiredQuantity?.Pieces ?? 0;
                //    //onItem.TotalInches = item.QuickQuoteData.RequiredQuantity?.PieceLength.Inches ?? 0;
                //    //onItem.TotalPounds = item.QuickQuoteData.RequiredQuantity?.PieceWeight.Pounds ?? 0;
                //    //onItem.CustomerUom = item.QuickQuoteData.UomValueForPdf();
                //    //onItem.UnitPrice = ConvertCurrency(item.QuickQuoteData.Price);
                //    //onItem.IsRegret = item.QuickQuoteData.Regret;
                //    //onItem.IsQuickQuoteItem = true;
                //    continue;
                //}
                //else
                {
                    onItem.StartingProductCode = item.QuotePrice.StartingProduct.ProductCode;
                    onItem.FinishProductCode = item.QuotePrice.FinishedProduct.ProductCode;
                    onItem.PartSpecification = item.PartSpecification;
                    onItem.Pieces = item.QuotePrice.RequiredQuantity.Pieces;
                    onItem.TotalInches = item.QuotePrice.RequiredQuantity.PieceLength.Inches.RoundAndNormalize(2);
                    onItem.TotalPounds = item.QuotePrice.RequiredQuantity.PieceWeight.Pounds.RoundAndNormalize(4);
                    onItem.CustomerUom = item.QuotePrice.CustomerUom.ToString();
                    onItem.UnitPrice = ConvertCurrency(item.QuotePrice.FinalPriceOverride.FinalPricePerEach).RoundAndNormalize(2); 
                    onItem.SellPricePerPound = ConvertCurrency(item.QuotePrice.FinalPriceOverride.FinalPricePerPound).RoundAndNormalize(2);
                    onItem.IsRegret = false;
                    onItem.IsQuickQuoteItem = false;
                }

                result.Add(onItem);

                decimal ConvertCurrency(decimal value)
                {
                    return helperCurrency.ConvertValueFromCurrencyToCurrency(value, quoteCurrency, displayCurrency);
                }

            }
            return result;
        }
    }
}
