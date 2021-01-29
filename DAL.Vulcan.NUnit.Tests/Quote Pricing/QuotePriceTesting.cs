using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Quotes;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.StockItems;
using Vulcan.IMetal.Queries.StockItems;

namespace DAL.Vulcan.NUnit.Tests.Quote_Pricing
{
    //public class QuotePriceDebugView
    //{
    //    public class ProductionStepDebugView
    //    {
    //        public ResourceType ResourceType { get; set; }
    //        public RequiredQuantity RequiredQuantity { get; set; }
    //        public List<CostValue> CostValues { get; set; } = new List<CostValue>();
    //        public List<ProductionStepTest> TestPieces { get; set; } = new List<ProductionStepTest>();
    //        public ProductionStepDebugView(CalculateQuotePriceModel model, ProductionStepCostBase step)
    //        {
    //            ResourceType = step.ResourceType;
    //            RequiredQuantity = step.RequiredQuantity;
    //            CostValues.AddRange(step.CostValues);
    //            TestPieces.AddRange(step.ProductionStepTests);
    //        }

    //    }


    //    public MaterialPriceOverride MaterialPriceOverride;

    //    public decimal TestPiecesTotalCost { get; set; }

    //    public int TestPieces { get; set; }

    //    public decimal KerfTotalCost { get; set; }

    //    public decimal KurfInchesPerCut { get; set; }

    //    public decimal Margin { get; set; }

    //    public decimal SalePrice { get; set; }
    //    public decimal MaterialCostPerPound { get; set; }
    //    public decimal MaterialCostPerInch { get; set; }

    //    public List<ProductionStepDebugView> ProductionCosts { get; set; } = new List<ProductionStepDebugView>();

    //    //public QuotePriceDebugView(QuotePriceModel qp, CalculateQuotePriceModel calculateQuotePriceModel)
    //    //{

    //    //    SalePrice = qp.SalePrice;
    //    //    Margin = qp.Margin;
    //    //    KurfInchesPerCut = qp.KurfInchesPerCut;
    //    //    KerfTotalCost = qp.KerfTotalCost;
    //    //    TestPieces = qp.TestPieces;
    //    //    TestPiecesTotalCost = qp.TestPiecesTotalCost;
    //    //    MaterialCostPerPound = qp.PricePerPound;
    //    //    MaterialCostPerInch = qp.StartingPricePerInch;

    //    //    ProductionCosts.AddRange(calculateQuotePriceModel.ProductionCosts.Select(x=> new ProductionStepDebugView(calculateQuotePriceModel,x.AsProductionStep())));

    //    //    MaterialPriceOverride = calculateQuotePriceModel.MaterialPriceOverride;

    //    //}

        
        
    //}

    //[TestFixture]
    //public class QuotePriceTesting
    //{
    //    public const string Application = "vulcancrm";
    //    public const string UserId = "599b1573b508d62d0c75a115";

    //    private readonly string _dividerLine = new String('=', 60);
    //    private readonly string _dividerLineTwo = new String('-', 60);

    //    public CalculateQuotePriceModel CalculateQuotePriceModel { get; set; }
    //    public decimal KurfInchesPerCut { get; set; } = (decimal)0.25;
    //    public QuotePriceModel QuotePriceModel { get; set; }
    //    public HelperQuote HelperQuote = new HelperQuote();


    //    private void SaveTestStep(string action)
    //    {
    //        Console.WriteLine(_dividerLine);
    //        Console.WriteLine(action);
    //        Console.WriteLine(_dividerLine);

    //        var debugView = new QuotePriceDebugView(QuotePriceModel, CalculateQuotePriceModel);

    //        Console.WriteLine(ObjectDumper.Dump(debugView));
    //    }

    //    [SetUp]
    //    public void Initialize()
    //    {
    //        InitializeBaseCostFromActualTagNumber();
    //        SaveTestStep("Initized Test with Product Code [718 10 SAAH]");
    //    }
    //    [Test]
    //    public void InitializeBaseCostFromActualTagNumber()
    //    {
    //        var coid = "INC";
    //        StockItemsContext context = ContextFactory.GetStockItemsContextForCoid(coid);
    //        context.Connection.Open();
    //        var stockItemQuery = StockItemsAdvancedQuery.AsQueryable(coid, context);

    //        stockItemQuery = stockItemQuery.Where(x => x.ProductCode == "718 5.25 5662").Take(1);

    //        var stockItem = stockItemQuery.First();

    //        var orderedQuantity = new OrderQuantity(5, 12, "in");

    //        var models = HelperQuote.GetNewCalculateQuotePriceModelForStockItem(coid, stockItem.ProductId, orderedQuantity, Application, UserId);

    //        CalculateQuotePriceModel = models.CalculateQuotePriceModel;
    //        QuotePriceModel = models.QuotePriceModel;

    //    }

    //    [Test]
    //    public void Start()
    //    {

    //        TestSawing();

    //        AddInspectionBlended();

    //        IncreaseeSalePriceBy1000();

    //        IncreaseMarginBy10Percent();

    //        AddHeatTreatWithTestPieceAndChangedFinishedProduct();

    //        //ExportTestStepsToExcel();
    //    }

    //    private void CalculateQuote()
    //    {
    //        var models = HelperQuote.CalculateQuotePrice(CalculateQuotePriceModel);
    //        QuotePriceModel = models.QuotePriceModel;
    //        CalculateQuotePriceModel = models.CalculateQuotePriceModel;
    //    }

    //    [Test]
    //    public void IncreaseeSalePriceBy1000()
    //    {

    //        CalculateQuotePriceModel.MaterialPriceOverride = new MaterialPriceOverride()
    //        {
    //            MaterialPriceOverrideType = MaterialPriceOverrideType.SalePrice,
    //            Value = QuotePriceModel.SalePrice + 1000
    //        };
    //        CalculateQuote();

    //        SaveTestStep("Increase Sale ProductionCost by $1000");
    //    }

    //    [Test]
    //    public void TestSawing()
    //    {
    //        var salePriceBeforeCutting = QuotePriceModel.SalePrice;

    //        var sawingCost = new ProductionStepCostBase()
    //        {
    //            IsPriceBlended = true,
    //            Id = Guid.NewGuid(),
    //            BaseCost = QuotePriceModel.BaseCostStarting,
    //            BaseCostFinished = QuotePriceModel.BaseCostFinished,
    //            StartingProduct = QuotePriceModel.StartingProduct,
    //            FinishedProduct = QuotePriceModel.FinishedProduct,
    //            CostValues = new List<CostValue>()
    //            {
    //                new CostValue()
    //                {
    //                    PerType = PerType.PerPiece,
    //                    ProductionCost = 10,
    //                    InternalCost = 5,
    //                    TypeName = "Cutting"
    //                }
    //            },
    //            ResourceType = ResourceType.Saw,
    //            RequiredQuantity = QuotePriceModel.RequiredQuantity
    //        };
    //        CalculateQuotePriceModel.ProductionCosts.Add(new ProductionStepCostModel(sawingCost));
    //        CalculateQuote();

    //        Assert.AreEqual(salePriceBeforeCutting + 
    //                        (CalculateQuotePriceModel.RequiredQuantity.Pieces * 10),
    //            QuotePriceModel.SalePrice);

    //        SaveTestStep("Add Sawing step $10/Cut as Blended InternalCost");

    //    }

    //    [Test]
    //    public void AddInspectionBlended()
    //    {
    //        var salePriceBeforeInspection = QuotePriceModel.SalePrice;

    //        var inspection = new ProductionStepCostBase()
    //        {
    //            IsPriceBlended = true,
    //            Id = Guid.NewGuid(),
    //            BaseCostStarting = QuotePriceModel.BaseCostStarting,
    //            BaseCostFinished = QuotePriceModel.BaseCostFinished,
    //            StartingProduct = QuotePriceModel.StartingProduct,
    //            FinishedProduct = QuotePriceModel.FinishedProduct,
    //            CostValues = new List<CostValue>()
    //            {
    //                new CostValue()
    //                {
    //                    PerType = PerType.PerLot,
    //                    ProductionCost = 150,
    //                    InternalCost = 50,
    //                    TypeName = "Inspection"
    //                }
    //            },
    //            ResourceType = ResourceType.Inspection,
    //            RequiredQuantity = QuotePriceModel.RequiredQuantity
    //        };
    //        CalculateQuotePriceModel.ProductionCosts.Add(new ProductionStepCostModel(inspection));
    //        CalculateQuote();

    //        // Flat fee should just be added to SalePrice
    //        Assert.AreEqual(salePriceBeforeInspection + inspection.CostValues.First().ProductionCost,
    //            QuotePriceModel.SalePrice);


    //        SaveTestStep("Add Inspection step $150/flat blended cost (Will increase the SalePrice by $150)");

    //    }

    //    [Test]
    //    public void AddHeatTreatWithTestPieceAndChangedFinishedProduct()
    //    {
    //        //ChangeFinishedProductAndSetNewBaseCostFinish();

    //        var salesPriceBeforeHeatTreat = QuotePriceModel.SalePrice;

    //        var testPieces = new OrderQuantity(1,6,"in");

    //        var heatTreat = new ProductionStepCostBase()
    //        {
    //            IsPriceBlended = true,
    //            Id = Guid.NewGuid(),
    //            BaseCostStarting = QuotePriceModel.BaseCostStarting,
    //            BaseCostFinished = QuotePriceModel.BaseCostFinished,
    //            StartingProduct = QuotePriceModel.StartingProduct,
    //            FinishedProduct = QuotePriceModel.FinishedProduct,
    //            CostValues = new List<CostValue>()
    //            {
    //                new CostValue()
    //                {
    //                    PerType = PerType.PerPound,
    //                    ProductionCost = (decimal)0.39,
    //                    InternalCost = (decimal)0.39,
    //                    MinimumCost = 150,
    //                    TypeName = "NQ&T Water"
    //                }
    //            },
    //            ResourceType = ResourceType.HeatTreat,
    //            RequiredQuantity = QuotePriceModel.RequiredQuantity,
    //        };
    //        //heatTreat.AddTestPieces("Tensile (stain)", testPieces);
    //        CalculateQuotePriceModel.ProductionCosts.Add(new ProductionStepCostModel(heatTreat));
    //        CalculateQuote();

    //        SaveTestStep("Add HeatTreat step $150/minimum / 0.39 per pound blended cost");

    //    }

    //    private void ChangeFinishedProductAndSetNewBaseCostFinish()
    //    {
    //        var coid = "INC";
    //        var finishedProduct = new StartingProduct(coid, "718 5.25 SAAH");
    //        CalculateQuotePriceModel.FinishedProduct = finishedProduct;

    //        CalculateQuotePriceModel.BaseCostFinish.ChangeTheoWeight(finishedProduct.TheoWeight);

    //    }


    //    [Test]
    //    private void IncreaseMarginBy10Percent()
    //    {

    //        CalculateQuotePriceModel.MaterialPriceOverride = new MaterialPriceOverride()
    //        {
    //            MaterialPriceOverrideType = MaterialPriceOverrideType.Margin,
    //            Value = QuotePriceModel.Margin + (decimal)0.10
    //        };
    //        CalculateQuote();
    //        SaveTestStep("Increase Margin by 10%");

    //    }

    //    /*
    //    private void ExportTestStepsToExcel()
    //    {
    //        XLWorkbook workbook = new XLWorkbook();
    //        var workSheet = workbook.Worksheets.Add("718 10 SAAH");

    //        var firstRow = true;

    //        var onRow = 6;
    //        foreach (var quoteTest in TestSteps)
    //        {
    //            if (firstRow)
    //            {

    //                workSheet.Cell("A1").Value = "ORDER QUANTITY";
    //                workSheet.Cell("A3").Value = "Pieces";
    //                workSheet.Cell("A4").Value = quoteTest.BaseCost.Pieces;

    //                workSheet.Cell("B3").Value = "Inches";
    //                workSheet.Cell("B4").Value = quoteTest.BaseCost.PieceLength.Inches;

    //                workSheet.Cell("C3").Value = "Lbs";
    //                workSheet.Cell("C4").Value = quoteTest.BaseCost.PieceWeight.Pounds;

    //                //workSheet.Cell("D3").Value = "Kerf/Piece";
    //                //workSheet.Cell("D4").Value = quoteTest.StartingBaseCost.KerfCost.InchesPerCut;

    //                //workSheet.Cell("E3").Value = "Kerf/Inches";
    //                //workSheet.Cell("E4").Value = quoteTest.StartingBaseCost.KerfCost.TotalInches;

    //                workSheet.Cell("F3").Value = "Est InternalCost/Inch";
    //                workSheet.Cell("F4").Value = quoteTest.BaseCost.MaterialCostPerInch;

    //                workSheet.Cell("G3").Value = "Est InternalCost/Pound";
    //                workSheet.Cell("G4").Value = quoteTest.BaseCost.EstCostPerPound;

    //                workSheet.Cell("H3").Value = "Est InternalCost/Kg";
    //                workSheet.Cell("H4").Value = quoteTest.BaseCost.EstCostPerKg;

    //                workSheet.Cell("I3").Value = "Est InternalCost/Piece";
    //                workSheet.Cell("I4").Value = quoteTest.BaseCost.EstCostPerPiece;

    //                workSheet.Cell("J3").Value = "WAvg InternalCost/Inch";
    //                workSheet.Cell("J4").Value = quoteTest.BaseCost.WAvgCostPerInch;

    //                workSheet.Cell("K3").Value = "WAvg InternalCost/Pound";
    //                workSheet.Cell("K4").Value = quoteTest.BaseCost.WAvgCostPerPound;

    //                workSheet.Cell("L3").Value = "WAvg InternalCost/Kg";
    //                workSheet.Cell("L4").Value = quoteTest.BaseCost.WAvgCostPerKg;

    //                //workSheet.Cell("M3").Value = "Kerf InternalCost";
    //                //workSheet.Cell("M4").Value = quoteTest.StartingBaseCost.KerfCost;

    //                //workSheet.Cell("N3").Value = "Kerf Inches/Each";
    //                //workSheet.Cell("N4").Value = quoteTest.StartingBaseCost.KerfCost.InchesPerCut;

    //                //workSheet.Cell("O3").Value = "Kerf Inches";
    //                //workSheet.Cell("O4").Value = quoteTest.StartingBaseCost.KerfCost.TotalInches;

    //                //workSheet.Cell("P3").Value = "Kerf Inches";
    //                //workSheet.Cell("P4").Value = quoteTest.StartingBaseCost.KerfCost.PoundsPerPiece;

    //                //workSheet.Cell("Q3").Value = "Kerf Pounds";
    //                //workSheet.Cell("Q4").Value = quoteTest.StartingBaseCost.KerfCost.TotalPounds;

    //                workSheet.Cell("R3").Value = "Total InternalCost";
    //                workSheet.Cell("R4").Value = quoteTest.BaseCost.InternalCost;

    //                workSheet.Cell($"A{onRow}").Value = "Test Operation";
    //                workSheet.Cell($"B{onRow}").Value = "ProductionCost/Inch";
    //                workSheet.Cell($"C{onRow}").Value = "ProductionCost/Pound";
    //                workSheet.Cell($"D{onRow}").Value = "ProductionCost/Kg";
    //                workSheet.Cell($"E{onRow}").Value = "ProductionCost/Piece";
    //                workSheet.Cell($"F{onRow}").Value = "Margin";
    //                workSheet.Cell($"G{onRow}").Value = "SalePrice";


    //            }

    //            onRow++;

    //            workSheet.Cell($"A{onRow+1}").Value = quoteTest.Operation;
    //            workSheet.Cell($"B{onRow+1}").Value = quoteTest.QuotePrice.StartingPricePerInch;
    //            workSheet.Cell($"C{onRow+1}").Value = quoteTest.QuotePrice.PricePerPound;
    //            workSheet.Cell($"D{onRow+1}").Value = quoteTest.QuotePrice.PricePerKilogram;
    //            workSheet.Cell($"E{onRow+1}").Value = quoteTest.QuotePrice.StartingPricePerPiece;
    //            workSheet.Cell($"F{onRow+1}").Value = quoteTest.QuotePrice.Margin;
    //            workSheet.Cell($"G{onRow+1}").Value = quoteTest.QuotePrice.SalePrice;

    //            var firstCost = true;
    //            foreach (var cost in quoteTest.ServiceCosts)
    //            {
    //                var onCol = "I";
    //                if (firstCost)
    //                {
    //                    onRow += 2;
    //                    workSheet.Cell($"I{onRow}").Value = "ResourceType";
    //                    workSheet.Cell($"J{onRow}").Value = "PerType";
    //                    workSheet.Cell($"K{onRow}").Value = "ProductionCost";
    //                    workSheet.Cell($"L{onRow}").Value = "InternalCost";
    //                    workSheet.Cell($"M{onRow}").Value = "IsPriceBlended";
    //                    firstCost = false;
    //                    onRow++;
    //                }

    //                workSheet.Cell($"{onCol}{onRow}").Value = cost.ResourceType;

    //                GetNextColumn(ref onCol);

    //                workSheet.Cell($"{onCol}{onRow}").Value = cost.PerType;

    //                GetNextColumn(ref onCol);

    //                workSheet.Cell($"{onCol}{onRow}").Value = cost.ProductionCost;

    //                GetNextColumn(ref onCol);

    //                workSheet.Cell($"{onCol}{onRow}").Value = cost.InternalCost;

    //                GetNextColumn(ref onCol);

    //                workSheet.Cell($"{onCol}{onRow}").Value = cost.IsPriceBlended;
    //                onRow++;

    //            }

    //            firstRow = false;
    //        }
    //        workbook.SaveAs(@"C:\Users\mpike\Documents\718-10-SAAH Quote Test.xlsx");

    //        void GetNextColumn(ref string onCol)
    //        {
    //            List<string> columns = new List<string>()
    //            {
    //                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
    //                "U", "V", "W", "X", "Y", "Z"
    //            };

    //            var indexOf = columns.IndexOf(onCol);
    //            onCol = columns[indexOf + 1];
    //        }
    //    }
    //    */
    //}
    
}
