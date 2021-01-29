using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    //public class QuotePriceGenerator
    //{
    //    public CalculateQuotePriceModel CalcModel { get; set; }
    //    private decimal KurfInchesPerCut { get; set; }
    //    private decimal CutCostPerPiece { get; set; }

    //    public QuotePrice Calculate()
    //    {
    //        CutCostPerPiece = CalcModel.CutCostPerPiece;

    //        var salePrice = (decimal)0;
    //        var costOverrides = GetCostOverrides();
    //        var productionSteps = GetProductionSteps();

    //        var startingProduct = CalcModel.StartingProduct;
    //        var baseCostStart = CalcModel.BaseCostStart;

    //        var baseCostFinish =
    //            BaseCost.CreateNewBaseCostFinishedFromStarting(baseCostStart, CalcModel.FinishedProduct);
    //        if (CalcModel.BaseCostFinish.IsCostMadeup)
    //        {
    //            baseCostFinish.UpdateAllValues(
    //                CalcModel.FinishedProduct.Coid,
    //                CalcModel.BaseCostFinish.MaterialCostPerPound,
    //                CalcModel.FinishedProduct.TheoWeight,
    //                CalcModel.OrderQuantity);

    //            baseCostFinish.MaterialCostPerPound = CalcModel.BaseCostFinish.MaterialCostPerPound;
    //        }

    //        salePrice = baseCostStart.MaterialTotalCost;
    //        baseCostStart = Calculate(baseCostStart, costOverrides);

    //        // Did we change the InternalCost?
    //        if (costOverrides.Count > 0)
    //        {
    //            salePrice = baseCostStart.MaterialTotalCost;
    //        }

    //        var productionStepCalculationResults = CalculateProductionSteps(baseCostFinish, ref salePrice, productionSteps);

    //        var margin = CalculateMargin(salePrice, salePrice);
    //        var totalInches = baseCostFinish.TotalInches;
    //        var totalPounds = baseCostFinish.TotalPounds;
    //        var totalKilos = baseCostFinish.TotalKilograms;

    //        //UpdateBaseCostPrices(baseCostStart, baseCostFinish, salePrice, totalInches, totalPounds, totalKilos);

    //        var quotePrice = new QuotePrice
    //        {
    //            Margin = margin,
    //            SalePrice = salePrice,
    //            StartingPricePerInch = salePrice / totalInches,
    //            PricePerKilogram = salePrice / totalKilos,
    //            PricePerPound = salePrice / totalPounds,
    //            StartingPricePerPiece = salePrice / baseCostFinish.TotalPieces,
    //            ProductionCosts = productionStepCalculationResults.BlendedCosts,
    //            KurfInchesPerCut = KurfInchesPerCut,
    //            BaseCostStart = baseCostStart,
    //            BaseCostFinish = baseCostFinish,
    //            RequiredQuantity = CalcModel.RequiredQuantity,
    //            CostOverrides = CalcModel.CostOverrides.Select(x=>x.AsCostOverride()).ToList(),
    //            MaterialPriceOverride = CalcModel.MaterialPriceOverride,
    //            StartingProduct = CalcModel.StartingProduct,
    //            FinishedProduct = CalcModel.FinishedProduct,
    //        };


    //        return quotePrice;
    //    }

    //    private void UpdateBaseCostPrices(BaseCost baseCostStart, BaseCost baseCostFinish, decimal salePrice, decimal totalInches, decimal totalPounds, decimal totalKilos)
    //    {
    //        //baseCostStart.MaterialCostPerPound = salePrice / totalPounds;
    //        //baseCostStart.CostPerKg = salePrice / totalKilos;
    //        //baseCostStart.MaterialCostPerInch = salePrice / totalInches;
    //        //baseCostStart.CostPerPiece = salePrice / baseCostStart.TotalPieces;

    //        baseCostFinish.MaterialCostPerPound = salePrice / totalPounds;
    //        baseCostFinish.CostPerKg = salePrice / totalKilos;
    //        baseCostFinish.MaterialCostPerInch = salePrice / totalInches;
    //        baseCostFinish.CostPerPiece = salePrice / baseCostFinish.TotalPieces;

    //    }

    //    public QuotePriceGenerator(CalculateQuotePriceModel calcModel, decimal kerfInchesPerCut, decimal cutCost)
    //    {
    //        CalcModel = calcModel;
    //        KurfInchesPerCut = kerfInchesPerCut;
    //        CutCostPerPiece = cutCost;
    //    }

    //    private List<ProductionStepCostBase> GetProductionSteps()
    //    {
    //        return CalcModel.ProductionCosts.Select(x=>x.AsProductionStep()).ToList();
    //    }

    //    private List<CostOverride> GetCostOverrides()
    //    {
    //        var result = new List<CostOverride>();
    //        result.AddRange(CalcModel.CostOverrides.Select(x => x.AsCostOverride()).ToList());
    //        return result;
    //    }

    //    public QuotePrice GetQuotePrice()
    //    {

    //        var quotePrice = Calculate();

    //        if (CalcModel.MaterialPriceOverride == null)
    //        {
    //            return quotePrice;
    //        }

    //        var overrideType = CalcModel.MaterialPriceOverride?.MaterialPriceOverrideType ?? MaterialPriceOverrideType.SalePrice;
    //        var overrideValue = CalcModel.MaterialPriceOverride?.Value ?? 0;

    //        switch (overrideType)
    //        {
    //            case MaterialPriceOverrideType.Margin:
    //                CalculateNewQuotePriceMargin(ref quotePrice,overrideValue);
    //                return quotePrice;
    //            case MaterialPriceOverrideType.SalePrice:
    //                CalculateNewQuoteSalePrice(ref quotePrice, overrideValue);
    //                return quotePrice;
    //            case MaterialPriceOverrideType.StartingPricePerInch:
    //                CalculateNewQuotePricePerInch(ref quotePrice, overrideValue);
    //                return quotePrice;
    //            case MaterialPriceOverrideType.PricePerPound:
    //                CalculateNewQuotePricePerPound(ref quotePrice, overrideValue);
    //                return quotePrice;
    //            case MaterialPriceOverrideType.PricePerKilogram:
    //                CalculateNewQuotePricePerKilogram(ref quotePrice, overrideValue);
    //                return quotePrice;
    //            case MaterialPriceOverrideType.StartingPricePerPiece:
    //                CalculateNewQuotePricePerPiece(ref quotePrice, overrideValue);
    //                return quotePrice;
    //        }

    //        return null;
    //    }

    //    private void CalculateNewQuotePricePerPiece(ref QuotePrice quotePrice, decimal pricePerPiece)
    //    {
    //        var finalBaseCost = quotePrice.BaseCostFinish;
    //        var totalInches = finalBaseCost.TotalInches;
    //        var totalPounds = finalBaseCost.TotalPounds;
    //        var totalKilos = finalBaseCost.TotalKilograms;
    //        var totalPieces = finalBaseCost.TotalPieces;



    //        var salePrice = totalPieces * pricePerPiece;

    //        var margin = CalculateMargin(finalBaseCost.MaterialTotalCost, salePrice);

    //        quotePrice.Margin = margin;
    //        quotePrice.SalePrice = salePrice;
    //        quotePrice.PricePerPound = (salePrice > 0) ? totalPounds / salePrice : 0;
    //        quotePrice.StartingPricePerPiece = (totalPieces > 0) ? salePrice / totalPieces : 0;
    //        quotePrice.StartingPricePerInch = (totalInches > 0) ? salePrice / totalInches : 0;
    //        quotePrice.PricePerKilogram = (totalKilos > 0) ? salePrice / totalKilos : 0;

    //    }

    //    private void CalculateNewQuotePricePerKilogram(ref QuotePrice quotePrice, decimal pricePerKg)
    //    {
    //        var finalBaseCost = quotePrice.BaseCostFinish;
    //        var totalInches = finalBaseCost.TotalInches;
    //        var totalPounds = finalBaseCost.TotalPounds;
    //        var totalKilos = finalBaseCost.TotalKilograms;
    //        var totalPieces = finalBaseCost.TotalPieces;

    //        var salePrice = totalKilos * pricePerKg;

    //        var margin = CalculateMargin(finalBaseCost.MaterialTotalCost, salePrice);

    //        quotePrice.Margin = margin;
    //        quotePrice.SalePrice = salePrice;
    //        quotePrice.PricePerPound = pricePerKg * (decimal)2.20462;
    //        quotePrice.StartingPricePerPiece = (totalPieces > 0) ? salePrice / totalPieces : 0;
    //        quotePrice.StartingPricePerInch = (totalInches > 0) ? salePrice / totalInches : 0;
    //        quotePrice.PricePerKilogram = pricePerKg;

    //    }

    //    private void CalculateNewQuotePricePerPound(ref QuotePrice quotePrice, decimal pricePerPound)
    //    {
    //        var finalBaseCost = quotePrice.BaseCostFinish;
    //        var totalInches = finalBaseCost.TotalInches;
    //        var totalPounds = finalBaseCost.TotalPounds;
    //        var totalKilos = finalBaseCost.TotalKilograms;
    //        var totalPieces = finalBaseCost.TotalPieces;

    //        var salePrice = totalPounds * pricePerPound;

    //        var margin = CalculateMargin(finalBaseCost.MaterialTotalCost, salePrice);

    //        quotePrice.Margin = margin;
    //        quotePrice.SalePrice = salePrice;
    //        quotePrice.PricePerPound = pricePerPound;
    //        quotePrice.StartingPricePerPiece = (totalPieces > 0) ? salePrice / totalPieces : 0;
    //        quotePrice.StartingPricePerInch = (totalInches > 0) ? salePrice / totalInches : 0;
    //        quotePrice.PricePerKilogram = (totalKilos > 0) ? salePrice / totalKilos : 0;

    //    }

    //    private void CalculateNewQuotePricePerInch(ref QuotePrice quotePrice, decimal pricePerInch)
    //    {
    //        var finalBaseCost = quotePrice.BaseCostFinish;
    //        var totalInches = finalBaseCost.TotalInches;
    //        var totalPounds = finalBaseCost.TotalPounds;
    //        var totalKilos = finalBaseCost.TotalKilograms;
    //        var totalPieces = finalBaseCost.TotalPieces;

    //        var salePrice = totalInches * pricePerInch;

    //        var margin = CalculateMargin(finalBaseCost.MaterialTotalCost, salePrice);

    //        quotePrice.Margin = margin;
    //        quotePrice.SalePrice = salePrice;
    //        quotePrice.PricePerPound = (totalPounds > 0) ? salePrice / totalPounds : 0;
    //        quotePrice.StartingPricePerPiece = (totalPieces > 0) ? salePrice / totalPieces : 0;
    //        quotePrice.StartingPricePerInch = (totalInches > 0) ? salePrice / totalInches : 0;
    //        quotePrice.PricePerKilogram = (totalKilos > 0) ? salePrice / totalKilos : 0;

    //    }


    //    private void CalculateNewQuotePriceMargin(ref QuotePrice quotePrice, decimal margin)
    //    {
    //        if (margin == 0) throw new Exception("Margin has to be greater than zero");
    //        if (margin > 1)
    //            margin = margin / 100;
    //        var salePrice = CalculateSalePrice(quotePrice.SalePrice, margin);

    //        var finalBaseCost = quotePrice.BaseCostFinish;
    //        var totalInches = finalBaseCost.TotalInches;
    //        var totalPounds = finalBaseCost.TotalPounds;
    //        var totalKilos = finalBaseCost.TotalKilograms;
    //        var totalPieces = finalBaseCost.TotalPieces;

    //        quotePrice.Margin = margin;
    //        quotePrice.SalePrice = salePrice;
    //        quotePrice.PricePerPound = (totalPounds > 0) ? salePrice / totalPounds : 0;
    //        quotePrice.StartingPricePerPiece = (totalPieces > 0 ) ? salePrice / totalPieces : 0;
    //        quotePrice.StartingPricePerInch = (totalInches > 0 ) ? salePrice / totalInches : 0;
    //        quotePrice.PricePerKilogram = (totalKilos > 0) ? salePrice / totalKilos : 0;

    //    }

    //    private void CalculateNewQuoteSalePrice(ref QuotePrice quotePrice, decimal salePrice)
    //    {
    //        quotePrice.SalePrice = salePrice;
    //        salePrice = quotePrice.SalePrice;
    //        var margin = CalculateMargin(quotePrice.BaseCostStart.MaterialTotalCost, quotePrice.SalePrice);

    //        var finalBaseCost = quotePrice.BaseCostFinish;
    //        var totalInches = finalBaseCost.TotalInches;
    //        var totalPounds = finalBaseCost.TotalPounds;
    //        var totalKilos = finalBaseCost.TotalKilograms;
    //        var totalPieces = finalBaseCost.TotalPieces;

    //        quotePrice.Margin = margin;
    //        quotePrice.SalePrice = salePrice;
    //        quotePrice.PricePerPound = (totalPounds > 0) ? salePrice / totalPounds : 0;
    //        quotePrice.StartingPricePerPiece = (totalPieces > 0) ? salePrice / totalPieces : 0;
    //        quotePrice.StartingPricePerInch = (totalInches > 0) ? salePrice / totalInches : 0;
    //        quotePrice.PricePerKilogram = (totalKilos > 0) ? salePrice / totalKilos : 0;

    //    }

    //    public (List<ProductionStepCostBase> BlendedCosts, List<ProductionStepCostBase> AdditionalCosts, BaseCost BaseCostFinish) CalculateProductionSteps(BaseCost baseCostStart, ref decimal salePrice, List<ProductionStepCostBase> productionCosts)
    //    {
            
    //        BaseCost baseCostFinish = baseCostStart;
    //        var productionSteps = productionCosts.Where(x => x.CostValues.Any(y=>(y.ProductionCost+y.MinimumCost) > 0 && y.IsActive)).ToList();
    //        var additionalSteps = new List<ProductionStepCostBase>();
    //        if (productionSteps.Any())
    //        {
    //            foreach (var productionCost in productionSteps)
    //            {
    //                if (productionCost.IsPriceBlended) 
    //                {
    //                    var productionStepCalculator = new ProductionStepCalculator(productionCost, baseCostStart, baseCostFinish, productionCost.ProductionStepTests);
    //                    productionStepCalculator.Calculate();
    //                    salePrice += productionStepCalculator.ProductionPrice;
    //                }
    //                else
    //                {
    //                    additionalSteps.Add(productionCost);
    //                }
    //                baseCostFinish = productionCost.BaseCostFinished;
    //            }
    //        }

    //        return (productionSteps, additionalSteps, baseCostFinish);
    //    }

    //    public BaseCost Calculate(BaseCost baseCost, List<CostOverride> costOverrides)
    //    {
    //        foreach (var costOverride in costOverrides)
    //        {
    //            switch (costOverride.OverrideType)
    //            {
    //                case OverrideType.TheoWeight:
    //                    baseCost.ChangeTheoWeight(costOverride.Value);
    //                    break;
    //                case OverrideType.MaterialCostPerInch:
    //                    baseCost.ChangeCostPerInch(costOverride.Value);
    //                    break;
    //                case OverrideType.MaterialCostPerPound:
    //                    baseCost.ChangeCostPerPound(costOverride.Value);
    //                    break;
    //                case OverrideType.CostPerKg:
    //                    baseCost.ChangeCostPerKg(costOverride.Value);
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }

    //        return baseCost;
    //    }

    //    private decimal CalculateMargin(decimal totalCost, decimal salePrice)
    //    {
    //        return QuoteCalculations.GetMargin(totalCost, salePrice);
    //    }

    //    private decimal CalculateSalePrice(decimal totalCost, decimal margin)
    //    {
    //        return QuoteCalculations.GetSalePriceFromMargin(totalCost, margin);
    //    }

    //}
}
