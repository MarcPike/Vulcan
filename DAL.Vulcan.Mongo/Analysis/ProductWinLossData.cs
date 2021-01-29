using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Analysis
{
    public class ProductWinLossData : BaseDocument
    {
        public static MongoRawQueryHelper<ProductWinLossData> Helper = new MongoRawQueryHelper<ProductWinLossData>();
        public string ProductCode { get; set; }
        public ProductMaster ProductInfo { get; set; }
        public List<ProductWinLossHistory> History { get; set; } = new List<ProductWinLossHistory>();

        public void AddNewWinLossHistory(CrmQuote quote, CrmQuoteItem quoteItem)
        {

            RemoveExistingInfo(quote.Id, quoteItem.Id);

            var coid = quote.Coid;
            var quoteId = quote.QuoteId;
            var quoteObjectId = quote.Id;
            var quoteItemObjectId = quoteItem.Id;
            var company = quote.Company;
            var prospect = quote.Prospect;
            var team = quote.Team;
            var salesPerson = quote.SalesPerson;
            var submitDate = quote.SubmitDate;
            var wonLossDate = quote.WonDate ?? quote.LostDate;
            var expired = quote.Status == PipelineStatus.Expired;
            var expiredDate = quote.ExpireDate;
            var displayCurrency = quote.DisplayCurrency;
            var pricePerInch = quoteItem.QuotePrice.MaterialPriceValue.PricePerInch;
            var pricePerPound = quoteItem.QuotePrice.MaterialPriceValue.PricePerPound;
            var pricePerKilogram= quoteItem.QuotePrice.MaterialPriceValue.PricePerKilogram;
            var pricePerFoot = quoteItem.QuotePrice.MaterialPriceValue.PricePerFoot;
            var pricePerEach = quoteItem.QuotePrice.PricePerEach;
            var requiredQuantity = quoteItem.QuotePrice.FinishQuantity;
            var totalPounds = quoteItem.QuotePrice.FinishPounds;
            var totalInches = quoteItem.QuotePrice.TotalInches;
            var totalKilograms = quoteItem.QuotePrice.FinishKilograms;
            var status = quote.Status;
            var loss = quoteItem.IsLost;
            var win = ((!quoteItem.IsLost) && (status == PipelineStatus.Won));
            var oemType = quoteItem.OemType;
            var totalCost = quoteItem.TotalCost;
            var totalPrice = quoteItem.TotalPrice;


            var isStartingProduct = ProductCode == quoteItem.QuotePrice.StartingProduct.ProductCode;
            var isFinishedProduct = ProductCode == quoteItem.QuotePrice.FinishedProduct.ProductCode;

            if (ProductInfo == null)
            {
                ProductInfo = (isStartingProduct)
                    ? quoteItem.QuotePrice.StartingProduct
                    : quoteItem.QuotePrice.FinishedProduct;
            }

            History.Add(new ProductWinLossHistory()
            {
                Coid = coid,
                QuoteId = quoteId,
                Company = company,
                Prospect = prospect,
                DisplayCurrency = displayCurrency,
                IsFinishedProduct = isFinishedProduct,
                IsStartingProduct = isStartingProduct,
                Loss = loss,
                Win = win,
                Status = status,
                Expired = expired,
                OemType = oemType,
                ExpiredDate = expiredDate,
                PricePerEach = pricePerEach,
                PricePerInch = pricePerInch,
                PricePerPound = pricePerPound,
                PricePerKilogram = pricePerKilogram,
                QuoteObjectId = quoteObjectId,
                QuoteItemObjectId = quoteItemObjectId,
                RequiredQuantity = requiredQuantity,
                SalesPerson = salesPerson,
                SubmitDate = submitDate ?? DateTime.MinValue,
                WonLossDate = wonLossDate ?? DateTime.MinValue,
                Team = team,
                TotalInches = totalInches,
                TotalKilograms = totalKilograms,
                TotalPounds = totalPounds,
                TotalCost = totalCost,
                TotalPrice = totalPrice,
            });


            Helper.Upsert(this);

        }


        private void RemoveExistingInfo(ObjectId quoteId, ObjectId quoteItemId)
        {
            var removeThese = History.Where(x => x.QuoteObjectId == quoteId && x.QuoteItemObjectId == quoteItemId)
                .ToList();
            if (removeThese == null) return;

            foreach (var productWinLossHistory in removeThese)
            {
                History.Remove(productWinLossHistory);
            }

            Helper.Upsert(this);
        }

    }
}
