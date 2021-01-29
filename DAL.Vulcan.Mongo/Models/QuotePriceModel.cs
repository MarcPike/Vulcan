using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vulcan.IMetal.Models;
using Vulcan.IMetal.Queries.StockItems;

namespace DAL.Vulcan.Mongo.Models
{

    public class QuotePriceModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }

        public MaterialCostValue MaterialCostValue { get; set; }

        public MaterialPriceValue MaterialPriceValue { get; set; }

        //public MaterialPriceHistory MaterialPriceHistoryForStartingProduct => new MaterialPriceHistory(StartingProduct);
        //public MaterialPriceHistory MaterialPriceHistoryForFinishedProduct => new MaterialPriceHistory(FinishedProduct);

        public List<CostOverride> CostOverrides { get; set; } = new List<CostOverride>();

        public RequiredQuantity RequiredQuantity { get; set; }


        public ProductMaster StartingProduct { get; set; }
        public ProductMaster FinishedProduct { get; set; }

        public List<ProductionStepCostBase> ProductionCosts { get; set; } = new List<ProductionStepCostBase>();

        public decimal TotalMaterialCost;
        public decimal TotalMaterialPrice;
        public decimal ProductionPriceTotal;
        public decimal ProductionCostTotal;
        public decimal ProductionTestCostTotal;
        public decimal ProductionTestPriceTotal;
        public decimal KerfTotalCost;
        public decimal TotalCutCost;
        public decimal TotalInches;
        public decimal StartingPounds;
        public decimal FinishPounds;
        public decimal TestPiecesInches;
        public decimal StartingKilograms;
        public decimal FinishKilograms;

        public decimal ExchangeRateUsd;
        public decimal ExchangeRateGbp;
        public decimal ExchangeRateCad;

        public decimal PricePerEach;
        public decimal PriceOverride;
        public decimal PricePerEachOverride;
        public decimal FinalPrice;

        public decimal MaterialOnlyPrice;
        public decimal KerfTotalPrice;

        public decimal MaterialOnlyCost;

        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        [BsonRepresentation(BsonType.String)]         // Mongo
        public CustomerUom CustomerUom;

        public List<ExchangeRate> ExchangeRates { get; set; } = ExchangeRate.GetRateList();

        public decimal TotalCost { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal Margin { get; set; }

        public FinalPriceOverride FinalPriceOverride { get; set; }

        public List<string> FinalPriceOverrideTypes => Enum.GetNames(typeof(FinalPriceOverrideType)).ToList();

        public List<CalculationValue> CalculationValues = new List<CalculationValue>();

        public QuoteSource QuoteSource { get; set; } = QuoteSource.StockItem;

        public QuotePriceModel()
        {
        }

        public QuotePriceModel(string application, string userId, QuotePrice quotePrice)
        {

            try
            {
                Application = application;
                UserId = userId;
                ProductionCosts = quotePrice.ProductionCosts;
                CostOverrides = quotePrice.CostOverrides;
                RequiredQuantity = quotePrice.RequiredQuantity;
                StartingProduct = quotePrice.StartingProduct;
                FinishedProduct = quotePrice.FinishedProduct;
                RequiredQuantity = quotePrice.RequiredQuantity;
                MaterialCostValue = quotePrice.MaterialCostValue;
                MaterialPriceValue = quotePrice.MaterialPriceValue;
                TotalCost = quotePrice.TotalCost;
                TotalPrice = quotePrice.TotalPrice;
                Margin = quotePrice.Margin;
                TotalMaterialCost = quotePrice.TotalMaterialCost;
                TotalMaterialPrice = quotePrice.TotalMaterialPrice;
                ProductionPriceTotal = quotePrice.ProductionPriceTotal;
                ProductionCostTotal = quotePrice.ProductionCostTotal;
                KerfTotalCost = quotePrice.KerfTotalCost;
                TotalCutCost = quotePrice.TotalCutCost;
                TotalInches = quotePrice.TotalInches;
                StartingPounds = quotePrice.StartingPounds;
                FinishPounds = quotePrice.FinishPounds;
                TestPiecesInches = quotePrice.TestPiecesInches;
                StartingKilograms = quotePrice.StartingKilograms;
                FinishKilograms = quotePrice.FinishKilograms;
                ExchangeRateCad = quotePrice.ExchangeRateCad;
                ExchangeRateGbp = quotePrice.ExchangeRateGbp;
                ExchangeRateUsd = quotePrice.ExchangeRateUsd;
                PricePerEach = quotePrice.PricePerEach;
                PriceOverride = quotePrice.PriceOverride;
                PricePerEachOverride = quotePrice.PricePerEachOverride;
                FinalPrice = quotePrice.FinalPrice;
                FinalPriceOverride = quotePrice.FinalPriceOverride;
                CustomerUom = quotePrice.CustomerUom;
                MaterialOnlyPrice = quotePrice.MaterialOnlyPrice;
                KerfTotalPrice = quotePrice.KerfTotalPrice;
                MaterialOnlyCost = quotePrice.MaterialOnlyCost;
                QuoteSource = quotePrice.QuoteSource;
                BuildCalculationDescriptors();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void BuildCalculationDescriptors()
        {
            //CalculationDescriptors.Add("MaterialOnlyCost",
            //    $"[Piece weight] {RequiredQuantity.PieceWeight.Pounds} * [Required Pieces] {RequiredQuantity.Pieces} * [Cost Per Pound] {MaterialCostValue.MaterialCostPerPound} = {MaterialOnlyCost}");
            //CalculationDescriptors.Add("MaterialOnlyPrice",
            //    $"[Piece weight] {RequiredQuantity.PieceWeight.Pounds} * [Required Pieces] {RequiredQuantity.Pieces} * [Cost Per Pound] {MaterialPriceValue.PricePerPound} = {MaterialOnlyPrice}");
            //CalculationDescriptors.Add("Kerf",
            //    $"[Total Cuts including Test Pieces] {MaterialCostValue.TotalPieces} * [Kerf Inches per Cut] {MaterialCostValue.KurfInchesPerCut} = [Kerf Total Inches] {MaterialCostValue.KerfTotalInches} * [Price Per Inch] {MaterialPriceValue.PricePerInch} = {MaterialPriceValue.KerfTotalPrice}");
            //CalculationDescriptors.Add("Sawing",
            //    $"[Total Cuts including Test Pieces] {MaterialCostValue.TotalPieces} * [Cut Cost] {MaterialCostValue.CutCostPerPiece} = {MaterialCostValue.TotalCutCost}");
            //CalculationDescriptors.Add("TestPieces",
            //    $"[Total Test Inches] {MaterialCostValue.TestPieceInches} * [Price Per Inch] {MaterialPriceValue.PricePerInch} = {MaterialPriceValue.TestPiecesTotalPrice}");

            CalculationValues.Add(new CalculationValue("PiecePounds", RequiredQuantity.PieceWeight.Pounds));

            CalculationValues.Add(new CalculationValue("PieceKilograms", RequiredQuantity.PieceWeight.Kilograms));

            CalculationValues.Add(new CalculationValue("RequiredPieces", RequiredQuantity.Pieces));

            CalculationValues.Add(new CalculationValue("CostPerPound", MaterialCostValue.MaterialCostPerPound));
            CalculationValues.Add(new CalculationValue("PricePerPound", MaterialPriceValue.PricePerPound));

            CalculationValues.Add(new CalculationValue("CostPerKilogram", MaterialCostValue.MaterialCostPerKg));
            CalculationValues.Add(new CalculationValue("PricePerKilogram", MaterialPriceValue.PricePerKilogram));

            CalculationValues.Add(new CalculationValue("MaterialOnlyCost", MaterialOnlyCost));
            CalculationValues.Add(new CalculationValue("MaterialOnlyPrice", MaterialOnlyPrice));
            CalculationValues.Add(new CalculationValue("TotalPieces", MaterialCostValue.TotalPieces));
            CalculationValues.Add(new CalculationValue("KerfInchesPerCut", MaterialCostValue.KurfInchesPerCut));

            CalculationValues.Add(new CalculationValue("PricePerInch", MaterialPriceValue.PricePerInch));
            CalculationValues.Add(new CalculationValue("KerfTotalPrice", MaterialPriceValue.KerfTotalPrice));

            CalculationValues.Add(new CalculationValue("CutCostPerPiece", MaterialCostValue.CutCostPerPiece));
            CalculationValues.Add(new CalculationValue("TotalCutCost", MaterialCostValue.TotalCutCost));

            CalculationValues.Add(new CalculationValue("TestPieceInches", MaterialCostValue.TestPieceInches));
            CalculationValues.Add(new CalculationValue("TestPiecesTotalPrice", MaterialPriceValue.TestPiecesTotalPrice));

        }

        public QuotePrice AsQuotePrice()
        {
            var result = new QuotePrice()
            {
                ProductionCosts = this.ProductionCosts,
                CostOverrides = this.CostOverrides,
                RequiredQuantity = this.RequiredQuantity,
                StartingProduct = this.StartingProduct,
                FinishedProduct = this.FinishedProduct,
                MaterialCostValue = this.MaterialCostValue,
                MaterialPriceValue = this.MaterialPriceValue,
                PriceOverride = this.PriceOverride,
                PricePerEachOverride = this.PricePerEachOverride,
                FinalPriceOverride = this.FinalPriceOverride,
                CustomerUom = this.CustomerUom
            };

            return result;
        }
    }
}