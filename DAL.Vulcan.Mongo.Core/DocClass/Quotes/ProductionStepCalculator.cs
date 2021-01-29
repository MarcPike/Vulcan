using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    //public class ProductionStepCalculator
    //{
    //    public ProductionStepCostBase ProductionStep { get; set; }
    //    public BaseCost BaseCostStarting { get; set; }
    //    public BaseCost BaseCostFinished { get; private set; }
    //    public decimal InternalCost { get; set; } = 0;
    //    public decimal ProductionPrice { get; set; } = 0;
    //    public decimal TestCost { get; set; } = 0;
    //    public decimal TestPrice { get; set; } = 0;
    //    public List<ProductionStepTest> TestPieces { get; set; }
    //    public decimal TestPiecePounds => TestPieces.Sum(x => x.GetPiecesValue(ProductionStep.FinishedProduct).Pounds());

    //    public ProductionStepCalculator(ProductionStepCostBase productionStep, BaseCost startingBaseCost, BaseCost finishBaseCost, List<ProductionStepTest> testPieces)
    //    {
    //        ProductionStep = productionStep;
    //        BaseCostStarting = startingBaseCost;
    //        BaseCostFinished = finishBaseCost;
    //        TestPieces = testPieces;
    //    }


    //    //public virtual decimal GetStartingWeight()
    //    //{
    //    //    return ProductionStep.StartingProduct.TheoWeight * BaseCostStarting.TotalInches;
    //    //}

    //    //public virtual decimal GetFinishedWeight()
    //    //{
    //    //    return ProductionStep.FinishedProduct.TheoWeight * BaseCostFinished.TotalInches;
    //    //}

    //    public virtual (decimal InternalCost, decimal ProductionPrice, decimal TestPrice) GetTotalCostAndPriceFromBaseCost(BaseCost startingBaseCost)
    //    {
    //        CreateBaseCostSnapShot(startingBaseCost);

    //        var productionCost = ProductionStep.CostValues.Sum(x => x.CalculateProductionCost(BaseCostFinished));
    //        var internalCost = ProductionStep.CostValues.Sum(x => x.CalculateInternalCost(BaseCostFinished));
    //        var testCost = TestPieces.Sum(x => x.TestCost.CalculateInternalCost(BaseCostFinished));
    //        ProductionStep.ProductionCost = productionCost;
    //        ProductionStep.TestCost = testCost;

    //        return (internalCost, ProductionStep.ProductionPrice, ProductionStep.TestPrice);
    //    }

    //    private void CreateBaseCostSnapShot(BaseCost startingBaseCost)
    //    {
    //        BaseCostStarting = BaseCost.Clone(startingBaseCost);
    //        BaseCostFinished = BaseCost.Clone(startingBaseCost);
    //        BaseCostFinished.ChangeTheoWeight(ProductionStep.FinishedProduct.TheoWeight);
    //    }

    //    public void Calculate()
    //    {
    //        var calcResults = GetTotalCostAndPriceFromBaseCost(BaseCostFinished);
    //        InternalCost = calcResults.InternalCost;
    //        ProductionPrice = calcResults.ProductionPrice;
    //        TestPrice = calcResults.TestPrice;

    //    }
    //}
}
