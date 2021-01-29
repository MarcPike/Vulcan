using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Quotes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vulcan.IMetal.Queries.StockItems;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class QuotePrice
    {

        public BaseCost BaseCost { get; set; }

        public List<QuoteTestPiece> TestPieces { get; set; } = new List<QuoteTestPiece>();

        public MaterialCostValue MaterialCostValue { get; set; }

        public MaterialPriceValue MaterialPriceValue { get; set; } 

        public List<CostOverride> CostOverrides { get; set; } = new List<CostOverride>();

        public RequiredQuantity RequiredQuantity { get; set; }

        public ProductMaster StartingProduct { get; set; }
        public ProductMaster FinishedProduct { get; set; }

        public List<ProductionStepCostBase> ProductionCosts { get; set; } = new List<ProductionStepCostBase>();

        public decimal TotalMaterialCost => MaterialCostValue.MaterialTotalCost;

        public decimal TotalMaterialPrice => MaterialPriceValue.MaterialTotalPrice;

        public decimal ExchangeRateUsd { get; set; }
        public decimal ExchangeRateGbp { get; set; }
        public decimal ExchangeRateCad { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        [BsonRepresentation(BsonType.String)]         // Mongo
        public CustomerUom CustomerUom { get; set; }

        public QuoteSource QuoteSource { get; set; } = QuoteSource.StockItem;

        public void SetStockGradeForProducts()
        {
            StartingProduct.SetStockGrade();
            FinishedProduct.SetStockGrade();
        }

        public decimal ProductionPriceTotal
        {
            get
            {
                decimal result = 0;
                if (ProductionCosts.Any())
                {

                    var firstStepId = ProductionCosts.First().Id;
                    ProductionStepCostBase previousStep = null;
                    foreach (var cost in ProductionCosts)
                    {
                        if (previousStep == null)
                        {
                            previousStep = cost;
                        }
                        else
                        {
                            cost.RequiredQuantity = previousStep.FinishQuantity;
                            previousStep = cost;
                        }
                        cost.CalculateMarginTotals();
                        result += cost.ProductionPrice;
                    }
                }
                return result;

            }
        }

        public decimal ProductionCostTotal
        {
            get
            {
                decimal result = 0;
                if (ProductionCosts.Any())
                {

                    var firstStepId = ProductionCosts.First().Id;
                    ProductionStepCostBase previousStep = null;
                    foreach (var cost in ProductionCosts)
                    {
                        if (previousStep == null)
                        {
                            previousStep = cost;
                        }
                        else
                        {
                            cost.RequiredQuantity = previousStep.FinishQuantity;
                            previousStep = cost;
                        }
                        cost.CalculateMarginTotals();
                        result += cost.ProductionCost;
                    }
                }
                return result;

            }
        }



        public decimal KerfTotalCost => MaterialCostValue.KerfTotalCost;
        public decimal KerfTotalPrice => MaterialPriceValue.KerfTotalPrice;

        public decimal TotalCutCost => MaterialCostValue.TotalCutCost;

        //public decimal QuickQuoteItemTotalCost => QuickQuoteItems.Sum(x => (x.Cost == 0) ? x.Price : x.Cost);

        //public decimal QuickQuoteItemTotalPrice => QuickQuoteItems.Sum(x => x.Price);

        public decimal MaterialOnlyPrice => TotalPrice - KerfTotalPrice - MaterialPriceValue.TestPiecesTotalPrice - MaterialCostValue.TotalCutCost - ProductionPriceTotal;

        public decimal TotalCost => MaterialCostValue.TotalCost + ProductionCostTotal + MaterialCostValue.TestPiecesTotalCost + MaterialCostValue.KerfTotalCost;// + QuickQuoteItemTotalCost;

        public decimal TotalPrice =>
            MaterialPriceValue.TotalPrice + ProductionPriceTotal + TotalCutCost + MaterialPriceValue.KerfTotalPrice + MaterialPriceValue.TestPiecesTotalPrice;// + QuickQuoteItemTotalPrice;

        public decimal TestPiecesInches => TestPieces.Sum(x => x.RequiredQuantity.TotalInches());

        public decimal TotalInches => MaterialCostValue.TotalInches;

        public decimal StartingPounds => TotalInches * StartingProduct.TheoWeight * StartingProduct.FactorForLbs;
        public decimal FinishPounds => TotalInches * FinishedProduct.TheoWeight * FinishedProduct.FactorForLbs;

        public decimal StartingKilograms => StartingPounds * (decimal) 0.453592;
        public decimal FinishKilograms => FinishPounds * (decimal)0.453592;

        public decimal PricePerEach => (RequiredQuantity.Pieces > 0 ) ? TotalPrice / RequiredQuantity.Pieces : 0;

        public decimal PriceOverride { get; set; } = 0;
        public decimal PricePerEachOverride { get; set; } = 0;

        public RequiredQuantity FinishQuantity =>
            RequiredQuantity.CalculateNewQuantityForNewProduct(RequiredQuantity, FinishedProduct);

        public FinalPriceOverride FinalPriceOverride { get; set; }

        public decimal FinalPrice => FinalPriceOverride?.FinalPrice ?? TotalPrice;

        public decimal Margin => QuoteCalculations.GetMargin(TotalCost, FinalPrice);
        public decimal MaterialOnlyCost => MaterialCostValue.MaterialOnlyCost;
    }

    //public class QuickQuoteItemModel
    //{
    //    public Guid Id { get; set; }
    //    public string Coid { get; set; }
    //    public string BaseCurrency { get; set; }
    //    public OrderQuantity OrderQuantity { get; set; }
    //    public string Label { get; set; }
    //    public decimal Cost { get; set; }
    //    public decimal Price { get; set; }

    //    public QuickQuoteItemModel()
    //    {

    //    }

    //    public QuickQuoteItemModel(QuickQuoteItem item)
    //    {
    //        Id = item.Id;
    //        Coid = item.Coid;
    //        BaseCurrency = item.BaseCurrency;
    //        OrderQuantity = item.OrderQuantity;
    //        Label = item.Label;
    //        Cost = item.Cost;
    //        Price = item.Price;
    //    }
    //}
}