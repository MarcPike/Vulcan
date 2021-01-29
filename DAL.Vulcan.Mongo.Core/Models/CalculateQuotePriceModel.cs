using DAL.iMetal.Core.Helpers;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Quotes;
using DAL.Vulcan.Mongo.Core.Team_Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public enum QuoteProductSourceType
    {
        StockItem,
        PurchaseOrder,
        MadeUpCost
    }
    // A QuotePrice can be recreated over and over
    public class CalculateQuotePriceModel 
    {
        public string Application { get; set; }
        public string UserId { get; set; }

        public QuoteProductSourceType QuoteProductSourceType { get; set; } = QuoteProductSourceType.StockItem;

        public BaseCost BaseCostStart { get; set; }
        public ProductMaster StartingProduct { get; set; }
        public ProductMaster FinishedProduct { get; set; }
        public RequiredQuantity RequiredQuantity { get; set; }
        public List<ProductionStepCostBase> ProductionCosts { get; set; } = new List<ProductionStepCostBase>();
        public List<CostOverrideModel> CostOverrides { get; set; } = new List<CostOverrideModel>();
        public MaterialCostValue MaterialCostValue { get; set; }
        public MaterialPriceValue MaterialPriceValue { get; set; }
        public decimal KurfInchesPerCut { get; set; } = (decimal) 0.25;
        public string Status { get; set; } = string.Empty;
        public OrderQuantity OrderQuantity { get; set; }
        public decimal CutCostPerPiece { get; set; } = 0;
        public decimal? CutCostPerPieceOverride { get; set; } = null;

        public List<QuoteTestPiece> TestPieces { get; set; } = new List<QuoteTestPiece>();

        public RequiredQuantity FinishedQuantity =>
            RequiredQuantity.CalculateNewQuantityForNewProduct(RequiredQuantity, FinishedProduct);

        [JsonConverter(typeof(JsonStringEnumConverter))]  // JSON.Net
        [BsonRepresentation(BsonType.String)]         // Mongo
        public FinalPriceOverrideType FinalPriceOverrideType { get; set; } = FinalPriceOverrideType.PerInch;
        public decimal FinalPriceOverrideValue { get; set; } = 0;

        public List<string> FinalPriceOverrideTypeList => Enum.GetNames(typeof(FinalPriceOverrideType)).ToList();

        public List<string> CustomerUomList => Enum.GetNames(typeof(CustomerUom)).ToList();

        [JsonConverter(typeof(JsonStringEnumConverter))]  // JSON.Net
        [BsonRepresentation(BsonType.String)]         // Mongo
        public CustomerUom CustomerUom { get; set; } = CustomerUom.PerPiece;

        public int ChangeStartingProductToThisStockItemId { get; set; } = 0;

        public string DisplayCurrency { get; set; } = string.Empty;

        public string OldCurrency { get; set; } = string.Empty;

        public QuoteSource QuoteSource { get; set; } = QuoteSource.StockItem;

        private HelperCurrencyForIMetal _helperCurrency = new HelperCurrencyForIMetal();
        private CrmUser _user;
        private Team _team;


        public static List<string> GetQuoteCalcOverridesSupported()
        {
            var quoteCalcOverridesSupported = new List<string>();
            foreach (MaterialPriceOverrideType overrideType in Enum.GetValues(typeof(MaterialPriceOverrideType)))
            {
                quoteCalcOverridesSupported.Add(overrideType.ToString());
            }
            return quoteCalcOverridesSupported;
        }

        public void CheckForChangeCurrencyExchangeRate()
        {

            var startingMaterialCoid = StartingProduct.Coid;
            var defaultCurrencyForCoid = _helperCurrency.GetDefaultCurrencyForCoid(startingMaterialCoid);

            var exchangeRate = BaseCostStart.ExchangeRate;


            if (DisplayCurrency != defaultCurrencyForCoid)
            {
                var newExchangeRate = _helperCurrency.GetExchangeRateForCurrencyFromCoid(DisplayCurrency, startingMaterialCoid);
                if (newExchangeRate != exchangeRate)
                {
                    BaseCostStart.ExchangeRate = newExchangeRate;
                    CalculateCutCost();
                }
            }
            else
            {
                BaseCostStart.ExchangeRate = (decimal)1;
                CalculateCutCost();
            }
        }

        public void SetStockGradeForProducts()
        {
            StartingProduct.SetStockGrade();
            FinishedProduct.SetStockGrade();
            foreach (var productionStepCostBase in ProductionCosts)
            {
                productionStepCostBase.FinishedProduct.SetStockGrade();
                productionStepCostBase.StartingProduct.SetStockGrade();
            }
        }

        public void RecalculateRequiredQuantity()
        {
            var requiredQuantity = BaseCostStart.UpdateAllValues(StartingProduct.Coid, BaseCostStart.CostPerPound, StartingProduct.TheoWeight, OrderQuantity);
            RequiredQuantity = requiredQuantity;
        }

        public QuotePrice GenerateQuotePrice(string displayCurrency, string oldCurrency, QuoteSource quoteSource, TeamPriceTier teamPriceTier)
        {
            var helperUser = new HelperUser();
            _user = helperUser.GetCrmUser(Application, UserId);
            _team = _user.ViewConfig.Team.AsTeam();

            var exchangeRates = _helperCurrency.GetExchangeRatesFromCoid(StartingProduct.Coid);

            //var exchangeRates = StockItemsAdvancedQuery.GetExchangeRatesFromCoid(StartingProduct.Coid);
            var exchangeRateUsd = exchangeRates.USD;
            var exchangeRateCad = exchangeRates.CAD;
            var exchangeRateGbp = exchangeRates.GBP;


            CheckForChangeCurrencyExchangeRate();

            CheckForStockChange(displayCurrency);


            DisplayCurrency = displayCurrency;
            if (DisplayCurrency == string.Empty)
            {
                DisplayCurrency = _team.DefaultCurrency;
            }
            OldCurrency = (oldCurrency == string.Empty) ? DisplayCurrency : oldCurrency;

            if (DisplayCurrency != OldCurrency)
            {
                var rateForCurrencyChange =
                    _helperCurrency.ConvertValueFromCurrencyToCurrency(1, OldCurrency, DisplayCurrency);

                // Force value of model to change for exchange rate change
                foreach (var costOverrideModel in CostOverrides)
                {
                    if (costOverrideModel.OverrideType != "TheoWeight")
                    {
                        costOverrideModel.Value = costOverrideModel.Value * rateForCurrencyChange;
                    }
                }

                MaterialCostValue.ApplyOverrides(CostOverrides.Select(x=> x.AsCostOverride()).ToList());

                MaterialPriceValue.ConvertAllOverridesForExchangeRate(rateForCurrencyChange);
                FinalPriceOverrideValue = FinalPriceOverrideValue * rateForCurrencyChange;
                AdjustProductionCostsUsingNewExchangeRate(DisplayCurrency);
            }

            CheckIfProductionCostListIsInDifferentCurrency(DisplayCurrency);

            RecalculateRequiredQuantity();

            MaterialCostValue.RequiredQuantity = RequiredQuantity;

            CalculateCutCost();


            var costOverrides = CostOverrides.Select(x => x.AsCostOverride()).ToList();
            MaterialCostValue = new MaterialCostValue(
                startingProduct: StartingProduct, 
                orderQuantity: OrderQuantity, 
                baseCost: BaseCostStart, 
                testPieces: TestPieces, 
                kurfInchesPerCut: KurfInchesPerCut, 
                costOverrides: costOverrides, 
                cutCostPerPiece:CutCostPerPiece, 
                displayCurrency: displayCurrency);

            MaterialPriceValue.UpdateCost(MaterialCostValue, teamPriceTier);

            UpdateWeightLengthRequirementsBeforeCalculating(MaterialCostValue);


            //MaterialPriceValue.UpdateCost(MaterialCostValue, teamPriceTier);
            if (ProductionCosts.Any())
            {
                FinishedProduct = ProductionCosts.Last().FinishedProduct;
            }

            UpdateStartingProductEqualsLastStepFinishedProduct();
            UpdateRequiredQuantityForAllProductionCosts();

            foreach (var productionCost in ProductionCosts)
            {
                foreach (var productionCostCostValue in productionCost.CostValues)
                {
                    productionCostCostValue.TotalPieces = MaterialCostValue.TotalPieces;
                    productionCostCostValue.TotalInches = MaterialCostValue.TotalInches;
                    productionCostCostValue.TotalPounds = productionCost.StartingPounds;
                }
            }

            var result = new QuotePrice()
            {
                BaseCost = BaseCostStart,
                RequiredQuantity = RequiredQuantity,
                MaterialCostValue = MaterialCostValue,
                MaterialPriceValue = MaterialPriceValue,
                FinishedProduct = FinishedProduct,
                StartingProduct = StartingProduct,
                CostOverrides = CostOverrides.Select(x=>x.AsCostOverride()).ToList(),
                ProductionCosts = ProductionCosts,
                TestPieces = TestPieces,
                ExchangeRateGbp = exchangeRateGbp,
                ExchangeRateCad = exchangeRateCad,
                ExchangeRateUsd = exchangeRateUsd,
                CustomerUom = CustomerUom,
                QuoteSource = quoteSource
            };

            //var finishedQuantityWithoutKerf = FinishedQuantity.GetFinishedQuantityWithoutKerf(MaterialCostValue.KerfPoundsPerPiece,
            //    MaterialCostValue.KurfInchesPerCut);

            var finalPriceOverride =
                new FinalPriceOverride(FinishedQuantity, result.TotalPrice, result.CustomerUom, FinalPriceOverrideType, FinalPriceOverrideValue);

            //var finalPriceOverride =
            //    new FinalPriceOverride(finishedQuantityWithoutKerf, result.TotalPrice, result.CustomerUom, FinalPriceOverrideType,
            //        FinalPriceOverrideValue);

            result.FinalPriceOverride = finalPriceOverride;

            return result;
        }

        private void CheckIfProductionCostListIsInDifferentCurrency(string displayCurrency)
        {
            foreach (var productionStepCostBase in ProductionCosts.ToList())
            {
                foreach (var costValue in productionStepCostBase.CostValues.ToList())
                {
                    //if (costValue.Currency == string.Empty)
                    //{
                    //    costValue.Currency = toCurrency;
                    //}

                    //if (costValue.Currency != toCurrency)
                    //{
                        costValue.Currency = displayCurrency;

                        costValue.ProductionCost =
                            _helperCurrency.ConvertValueFromCurrencyToCurrency(costValue.ProductionCost, costValue.Currency, displayCurrency);

                        costValue.MinimumCost =
                            _helperCurrency.ConvertValueFromCurrencyToCurrency(costValue.MinimumCost,
                                costValue.Currency, displayCurrency);

                        costValue.InternalCost =
                            _helperCurrency.ConvertValueFromCurrencyToCurrency(costValue.InternalCost,
                                costValue.Currency, displayCurrency);

                    //}
                }
            }
        }

        //private void AdjustProductionCostsUsingNewExchangeRate(decimal rateForCurrencyChange, string toCurrency)
        //{
        //    foreach (var productionStepCostBase in ProductionCosts.ToList())
        //    {
        //        foreach (var costValue in productionStepCostBase.CostValues.ToList())
        //        {
        //            costValue.ProductionCost = costValue.ProductionCost * rateForCurrencyChange;
        //            costValue.MinimumCost = costValue.MinimumCost * rateForCurrencyChange;
        //            costValue.InternalCost = costValue.InternalCost * rateForCurrencyChange;
        //            costValue.Currency = toCurrency;
        //        }
        //    }
        //}
        private void AdjustProductionCostsUsingNewExchangeRate(string displayCurrency)
        {
            var helperCurrency = new HelperCurrencyForIMetal();

            foreach (var productionStepCostBase in ProductionCosts.ToList())
            {
                foreach (var costValue in productionStepCostBase.CostValues.ToList())
                {
                    costValue.InternalCost = helperCurrency.ConvertValueFromCurrencyToCurrency(costValue.InternalCost, costValue.Currency,
                        displayCurrency);

                    costValue.MinimumCost = helperCurrency.ConvertValueFromCurrencyToCurrency(costValue.MinimumCost, costValue.Currency,
                        displayCurrency);

                    costValue.ProductionCost = helperCurrency.ConvertValueFromCurrencyToCurrency(costValue.ProductionCost, costValue.Currency,
                        displayCurrency);
                    costValue.Currency = displayCurrency;
                }
            }
        }

        private void UpdateStartingProductEqualsLastStepFinishedProduct()
        {
            var lastFinishedProduct = StartingProduct;
            foreach (var productionStepCostBase in ProductionCosts)
            {
                productionStepCostBase.StartingProduct = lastFinishedProduct;
                lastFinishedProduct = productionStepCostBase.FinishedProduct;
            }
        }

        private void UpdateRequiredQuantityForAllProductionCosts()
        {
            var requiredQuantity = RequiredQuantity;
            foreach (var productionStepCostBase in ProductionCosts)
            {
                productionStepCostBase.RequiredQuantity = requiredQuantity;
                requiredQuantity = productionStepCostBase.FinishQuantity;
            }
        }

        private void CheckForStockChange(string displayCurrency)
        {
            if (ChangeStartingProductToThisStockItemId > 0)
            {
                var newStartingProduct =
                    ProductMaster.FromStockId(StartingProduct.Coid, ChangeStartingProductToThisStockItemId);
                if (newStartingProduct.ProductMaster != null)
                {
                    StartingProduct = newStartingProduct.ProductMaster;
                    var stockItem = newStartingProduct.StockItem;
                    var baseCostValues = BaseCost.FromStockItems(StartingProduct.Coid, stockItem.CostPerLb,
                        stockItem.TheoWeight, OrderQuantity, stockItem.TagNumber, displayCurrency);

                    RequiredQuantity = baseCostValues.RequiredQuantity;
                    BaseCostStart = baseCostValues.BaseCost;
                    ChangeStartingProductToThisStockItemId = 0;
                }
            }
        }

        private void UpdateWeightLengthRequirementsBeforeCalculating(MaterialCostValue materialCostValue)
        {
            foreach (var productionCost in ProductionCosts)
            {
                productionCost.TotalInches = materialCostValue.TotalInches;
            }
        }

        public void CalculateCutCost()
        {
            /*
             * LOW ALLOY – 0.48
             * STAINLESS – 0.83
             * NICKEL – 1.18
            */
            /* 6/29/18
             * LOW ALLOY – 0.48
             * STAINLESS – 0.48
             * NICKEL – 1.18
            */
            if (StartingProduct.StockGrade == string.Empty)
            {
                CutCostPerPiece = CutCostPerPieceOverride ?? 0;
                return;
            }


            if (CutCostPerPieceOverride != null)
            {
                CutCostPerPiece = CutCostPerPieceOverride ?? 0;
                return;
            }

            var squareInches = (decimal)3.1416 * (StartingProduct?.OuterDiameter / 2) * (StartingProduct?.OuterDiameter / 2) ?? 0;

            decimal timeSquareInch = (decimal)0;

            if ((FinishedProduct.MetalCategory == "LOW ALLOY") || (FinishedProduct.MetalCategory == "OTHER"))
            {
                if (FinishedProduct.StockGrade.StartsWith("86") || FinishedProduct.StockGrade.StartsWith("41") || FinishedProduct.StockGrade.StartsWith("10"))
                {
                    timeSquareInch = (decimal)0.40;
                }
                else
                {
                    timeSquareInch = (decimal)0.48;
                }

            }
            if (FinishedProduct.MetalCategory == "STAINLESS")
            {
                timeSquareInch = (decimal)0.48;
            }
            if (FinishedProduct.MetalCategory == "NICKEL")
            {
                timeSquareInch = (decimal)1.18;
            }


            decimal totalCutTime = squareInches * timeSquareInch;
            decimal totalSawHours = (totalCutTime * RequiredQuantity.Pieces) / 60;
            decimal totalSawMinutes = totalSawHours * 60;
            CutCostPerPiece = ((totalSawMinutes * (decimal)0.75) / RequiredQuantity.Pieces) * BaseCostStart.ExchangeRate;
        }

    }
}
