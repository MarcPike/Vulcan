using BLL.EMail;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.QNG;
using SVC.QNG.Exporter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Extensions;
using log4net;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Vulcan.IMetal.Helpers;

namespace SVC.QNG.Exporter
{
    [TestFixture()]
    public class QuoteExportWorker
    {
        private readonly RepositoryBase<QngExportLog> _repExportLog = new RepositoryBase<QngExportLog>();
        private DateTime _lastExportTime;
        private List<CrmQuote> _quotesToExport { get; set; } = new List<CrmQuote>();
        private QngExportLog _log = new QngExportLog();
        private List<string> _errors => new List<string>();
        private static HelperCurrencyForIMetal _helperCurrency => new HelperCurrencyForIMetal();

        private bool isTesting = false;
        private static readonly ILog Log = LogManager.GetLogger(typeof(QngExportService));

        [Test]
        public void Execute()
        {

            GetLastExportDateTime();
            GetQuotesToExport();

            Stopwatch watcher = new Stopwatch();
            watcher.Start();

            ExportQuotesToSqlServer();
            SaveLog();

            watcher.Stop();
            Log.Info($"Completed @ {DateTime.Now} - Elapsed time {watcher.Elapsed}");

        }

        [Test]
        public void ExportSingleQuote()
        {
            EnvironmentSettings.CrmProduction();

            var quoteId = 112456;
            var queryHelper = new MongoRawQueryHelper<CrmQuote>();
            var filter = queryHelper.FilterBuilder.Where(x => x.QuoteId == quoteId);
            var quote = queryHelper.Find(filter).FirstOrDefault();
            Assert.IsNotNull(quote);
            _quotesToExport.Add(quote);
            isTesting = true;

            Stopwatch watcher = new Stopwatch();
            watcher.Start();

            ExportQuotesToSqlServer();
            SaveLog();

            watcher.Stop();
            Log.Info($"Completed @ {DateTime.Now} - Elapsed time {watcher.Elapsed}");

        }

        [Test]
        public void AllEuropeQuotes()
        {
            EnvironmentSettings.CrmProduction();

            var queryHelper = new MongoRawQueryHelper<CrmQuote>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Coid == "EUR" && x.DisplayCurrency != "GBP");
            var quotes = queryHelper.Find(filter).ToList();
            _quotesToExport.AddRange(quotes);
            isTesting = true;

            Stopwatch watcher = new Stopwatch();
            watcher.Start();

            ExportQuotesToSqlServer();
            SaveLog();

            watcher.Stop();
            Log.Info($"Completed @ {DateTime.Now} - Elapsed time {watcher.Elapsed}");

        }


        private void GetLastExportDateTime()
        {
            var lastRow = _repExportLog.AsQueryable().OrderByDescending(x => x.ModifiedDateTime).FirstOrDefault();

            if (lastRow == null)
            {
                _lastExportTime = new DateTime(1980, 1, 1);
            }
            else
            {
                _lastExportTime = lastRow.ExportTime;
            }

        }

        private void GetQuotesToExport()
        {
            _log.Status = QngExportStatus.Fetching;

            //SaveAllQuickQuoteItemsOneTime();

            _quotesToExport = new RepositoryBase<CrmQuote>().AsQueryable().Where(x=>x.ModifiedDateTime >= _lastExportTime).ToList();


        }

        private void SaveAllQuickQuoteItemsOneTime()
        {
            var quickQuoteQuotes = new RepositoryBase<CrmQuote>().AsQueryable().Where(x=>x.Items.Any(i=>i.IsQuickQuoteItem)).ToList();
            foreach (var quickQuoteQuote in quickQuoteQuotes)
            {
                quickQuoteQuote.SaveToDatabase();                
            }
        }

        private void ExportQuotesToSqlServer()
        {
            _log.Status = QngExportStatus.Exporting;

            using (var context = new ODSContext())
            {
                var removeRequired = context.Vulcan_CrmQuote.Any();

                if (!isTesting)
                {
                    AddMissingQuotesToQuotesToExport(context);
                }

                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;

                //var inspectQuotes = new List<int>()
                //{
                //    103354
                //};

                Log.Info($"Started @ {DateTime.Now} - processing {_quotesToExport.Count}");
                foreach (var crmQuote in _quotesToExport.ToList())
                {
                    //if (inspectQuotes.Any(x => x == crmQuote.QuoteId))
                    //{
                    //    var message = "We are here";
                    //}

                    if (removeRequired)
                    {
                        RemoveExistingRow(context, crmQuote);
                    }
                    List<CrmQuoteItem> quoteItems = new List<CrmQuoteItem>();
                    try
                    {
                        quoteItems = AddQuote(context, crmQuote);
                        AddQuoteItems(context, crmQuote, quoteItems);
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var item in ex.EntityValidationErrors.Where(x => x.IsValid == false).ToList())
                        {
                            var entry = item.Entry;
                            var errors = item.ValidationErrors;
                        }

                        var error = $"QuoteId: {crmQuote.QuoteId} => {EF_Utils.GetEntityValidationErrors(ex)}";
                        _errors.Add(error);
                        Log.Error(error);
                        EMailSupport.SendEmailToSupport("Export to QNG Exception", new List<string>()
                        {
                            "marc.pike@howcogroup.com",
                            "isidro.gallegos@howcogroup.com"
                        }, error);
                        _log.RowsSkipped++;
                        _log.Errors.Add(error);

                        continue;
                    }
                    catch (Exception ex)
                    {

                        StringBuilder errors = new StringBuilder();
                        errors.AppendLine($"QuoteId: {crmQuote.QuoteId} => {ex.Message} : {ex.InnerException?.Message ?? "No InnerException"}");
                        foreach (var crmQuoteItem in quoteItems)
                        {
                            errors.AppendLine(ObjectDumper.Dump(quoteItems));
                        }

                        var error = errors.ToString();
                        Log.Error(error);
                        _errors.Add(error);


                        EMailSupport.SendEmailToSupport("Export to QNG Exception", new List<string>()
                        {
                            "marc.pike@howcogroup.com",
                            "isidro.gallegos@howcogroup.com"
                        }, errors.ToString());
                        _log.RowsSkipped++;
                        _log.Errors.Add(errors.ToString());

                        continue;
                    }


                    _log.RowsExported++;
                }

                var removeDeletedQuotes = new RemoveDeletedQuotesWorker();
                removeDeletedQuotes.Execute(context);
            }
        }

        private void AddMissingQuotesToQuotesToExport(ODSContext context)
        {
            var quoteIdsAlreadyExportedAlready = context.Vulcan_CrmQuote.Select(x => x.QuoteId).Distinct().ToList();
            var missingQuotes = new RepositoryBase<CrmQuote>().AsQueryable().Where(x=> !quoteIdsAlreadyExportedAlready.Contains(x.QuoteId)).ToList();

            if (missingQuotes.Any())
            {
                _quotesToExport.AddRange(missingQuotes);
            }


        }

        private void SaveLog()
        {
            _log.Finished = DateTime.Now;
            _log.Status = QngExportStatus.Completed;
            Log.Info($"Exported {_log.RowsExported} Skipped: {_log.RowsSkipped} Errors: {_log.Errors.Count} Elapsed Time: {_log.ElapsedTime}");

            if (!isTesting)
            {
                _repExportLog.Upsert(_log);
            }
        }

        public QuoteExportWorker()
        {
        }

        private void AddQuoteItems(ODSContext context, CrmQuote crmQuote, List<CrmQuoteItem> crmQuoteItems)
        {
            if (crmQuoteItems == null) return;

            foreach (var crmQuoteItem in crmQuoteItems.Where(x=> x != null).OrderBy(x=>x.Index))
            {
                var baseCurrency = GetBaseCurrencyForQuoteItem(crmQuote, crmQuoteItem);
                var displayCurrency = GetDisplayCurrencyForQuote(crmQuote);

                var newQuoteItem = CreateNewVulcanCrmQuoteItem(crmQuote, crmQuoteItem, baseCurrency, displayCurrency);

                var exchangeRate =
                    _helperCurrency.ConvertValueFromCurrencyToCurrency(1, newQuoteItem.BaseCurrency, displayCurrency).RoundAndNormalize(4);
                var exchangeRateBase =
                    _helperCurrency.ConvertValueFromCurrencyToCurrency(1, newQuoteItem.BaseCurrency, baseCurrency).RoundAndNormalize(4);


                GetValuesForNormalCrmQuoteItem(crmQuoteItem, newQuoteItem, crmQuote.Coid, baseCurrency, displayCurrency, exchangeRate, exchangeRateBase);
                GetValuesForQuickQuoteItem(crmQuoteItem, newQuoteItem, baseCurrency, displayCurrency, exchangeRate, exchangeRateBase);
                GetValuesForMachinedPart(crmQuoteItem, newQuoteItem, baseCurrency, displayCurrency, exchangeRate, exchangeRateBase);
                GetValuesForCrozCalcItems(crmQuoteItem, newQuoteItem, baseCurrency, displayCurrency, exchangeRate, exchangeRateBase);

                context.Vulcan_CrmQuoteItem.Add(newQuoteItem);

                context.SaveChanges();

                AddTestPieces(context, crmQuote, crmQuoteItem, newQuoteItem.Id);

                AddProductionCost(context, crmQuote, crmQuoteItem, newQuoteItem.Id, newQuoteItem.ExchangeRate, baseCurrency);

            }

        }

        private void GetValuesForCrozCalcItems(CrmQuoteItem crmQuoteItem, Vulcan_CrmQuoteItem newQuoteItem, string baseCurrency, string displayCurrency, decimal exchangeRate, decimal exchangeRateBase)
        {
            if (!crmQuoteItem.IsCrozCalc) return;

            newQuoteItem.CalcItemBaseUnitCost = crmQuoteItem.CrozCalcItem.ResultBaseUnitCost;
            newQuoteItem.CalcItemBaseUnitPrice = crmQuoteItem.CrozCalcItem.ResultBaseUnitPrice;
            newQuoteItem.CalcItemPieces = crmQuoteItem.CrozCalcItem.OrderQuantity.Pieces;
            newQuoteItem.CalcItemQuantity = crmQuoteItem.CrozCalcItem.OrderQuantity.Quantity;
            newQuoteItem.CalcItemQuantityUOM = crmQuoteItem.CrozCalcItem.OrderQuantity.QuantityType;
            newQuoteItem.CalcItemRegret = crmQuoteItem.CrozCalcItem.Regret;
            newQuoteItem.CalcItemStartingProductLabel = crmQuoteItem.CrozCalcItem.StartingProductLabel;
            newQuoteItem.CalcItemFinishedProductLabel = crmQuoteItem.CrozCalcItem.FinishedProductLabel;
            newQuoteItem.CalcItemUnitCost = crmQuoteItem.CrozCalcItem.UnitCost;
            newQuoteItem.CalcItemUnitPrice = crmQuoteItem.CrozCalcItem.UnitPrice;
            newQuoteItem.CalcItemTotalCost = crmQuoteItem.CrozCalcItem.TotalCost;
            newQuoteItem.CalcItemTotalPrice = crmQuoteItem.CrozCalcItem.TotalPrice;
        }

        private void GetValuesForMachinedPart(CrmQuoteItem crmQuoteItem, Vulcan_CrmQuoteItem newQuoteItem, string baseCurrency, string displayCurrency,
            decimal exchangeRate, decimal exchangeRateBase)
        {
            if (!crmQuoteItem.IsMachinedPart) return;
            var model = crmQuoteItem.MachinedPartModel;


            newQuoteItem.QuoteSource = crmQuoteItem.QuoteSource.ToString();
            newQuoteItem.StartingProductCode = model.MachinedPart.ProductCode;
            newQuoteItem.DisplayCurrency = displayCurrency;
            newQuoteItem.FinalPrice = model.TotalPrice;
            newQuoteItem.CostPerPiece = model.PieceCost;
            newQuoteItem.PricePerEach = model.PiecePrice;
            newQuoteItem.Pieces = model.Pieces;
            newQuoteItem.Margin = model.Margin;
            newQuoteItem.StartingProductCoid = model.Coid;
            newQuoteItem.TagNumber = model.MachinedPart.TagNumber;
            newQuoteItem.StartingProductCondition = model.MachinedPart.ProductCondition;
            newQuoteItem.StartingStockGrade = model.MachinedPart.StockGrade;
            newQuoteItem.TotalPounds = model.PieceWeightLbs * model.Pieces;
            newQuoteItem.TotalKilograms = model.PieceWeightKilos * model.Pieces;

            newQuoteItem.StartingStratificationRank = model.MachinedPart.StratificationRank;

            newQuoteItem.BaseFinalPrice = (newQuoteItem.FinalPrice ?? 0) * exchangeRateBase;
            newQuoteItem.BaseCostPerPiece = newQuoteItem.CostPerPiece * exchangeRateBase;
            newQuoteItem.BasePricePerEach = newQuoteItem.PricePerEach * exchangeRateBase;

        }

        private static void AddProductionCost(ODSContext context, CrmQuote crmQuote, CrmQuoteItem crmQuoteItem, int vulcanQuoteItemId, decimal exchangeRate, string baseCurrency)
        {
            if ((crmQuoteItem.IsQuickQuoteItem == false) && (crmQuoteItem.IsMachinedPart == false) &&
                (crmQuoteItem.CalculateQuotePriceModel.ProductionCosts.Any()))
            {
                foreach (var productionStepCostBase in crmQuoteItem.CalculateQuotePriceModel.ProductionCosts)
                {
                    foreach (var costValue in productionStepCostBase.CostValues.Where(x => x.IsActive))
                    {
                        var newVulcanCrmQuoteItemProductionCost = new Vulcan_CrmQuoteItem_ProductionCost()
                        {
                            QIId = vulcanQuoteItemId,
                            QuoteId = crmQuote.QuoteId,
                            QuoteItemId = crmQuoteItem.Index,
                            Coid = productionStepCostBase.Coid,
                            ResourceType = productionStepCostBase.ResourceTypeName,
                            LocationName = string.Empty,

                            InternalCost = costValue.InternalCost * exchangeRate,
                            BaseInternalCost = costValue.InternalCost,

                            MinimumCost = costValue.MinimumCost * exchangeRate,
                            BaseMinimumCost = costValue.MinimumCost,

                            PerType = costValue.PerType.ToString(),

                            ProductionCost = costValue.ProductionCost * exchangeRate,
                            BaseProductionCost = costValue.ProductionCost,

                            TotalInches = costValue.TotalInches,
                            TotalInternalCost = costValue.TotalInternalCost * exchangeRate,
                            TotalPieces = costValue.TotalPieces,
                            TotalPounds = costValue.TotalPounds,
                            TotalProductionCost = costValue.TotalProductionCost * exchangeRate,
                            TypeName = costValue.TypeName,
                            ImportDateTimeUTC = DateTime.Now.ToUniversalTime(),
                            BaseCurrency = baseCurrency

                        };
                        context.Vulcan_CrmQuoteItem_ProductionCost.Add(newVulcanCrmQuoteItemProductionCost);
                        context.SaveChanges();
                    }
                }
            }
        }

        private static void AddTestPieces(ODSContext context, CrmQuote crmQuote, CrmQuoteItem crmQuoteItem, int vulcanQuoteItemId)
        {
            if ((crmQuoteItem.IsQuickQuoteItem == false) && (crmQuoteItem.IsMachinedPart == false) && (crmQuoteItem.CalculateQuotePriceModel.TestPieces.Any()))
            {
                foreach (var quoteTestPiece in crmQuoteItem.CalculateQuotePriceModel.TestPieces)
                {
                    var newVulcanCrmQuoteItemTestPiece = new Vulcan_CrmQuoteItem_TestPieces()
                    {
                        QIId = vulcanQuoteItemId,
                        Coid = quoteTestPiece.Coid,
                        Pieces = quoteTestPiece.OrderQuantity.Pieces,
                        Quantity = quoteTestPiece.OrderQuantity.Quantity,
                        QuantityType = quoteTestPiece.OrderQuantity.QuantityType,
                        QuoteId = crmQuote.QuoteId,
                        QuoteItemId = crmQuoteItem.Index,
                        StartingProductCode = crmQuoteItem.QuotePrice.StartingProduct.ProductCode,
                        TestName = quoteTestPiece.Name ?? "(no name provided)",
                        ImportDateTimeUTC = DateTime.Now.ToUniversalTime()

                    };
                    context.Vulcan_CrmQuoteItem_TestPieces.Add(newVulcanCrmQuoteItemTestPiece);
                    context.SaveChanges();
                }
            }
        }

        private static string Left(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value ?? string.Empty;
            maxLength = Math.Abs(maxLength);

            if (value.Length <= maxLength)
            {
                return value;
            }
            else
            {
                return value.Substring(0, maxLength);
            }
        }
    

        private static void GetValuesForQuickQuoteItem(CrmQuoteItem crmQuoteItem, Vulcan_CrmQuoteItem newQuoteItem, 
            string baseCurrency, string displayCurrency, decimal exchangeRate, decimal exchangeRateBase)
        {
            if (crmQuoteItem.IsQuickQuoteItem)
            {

                newQuoteItem.QuickQuoteItemCost = crmQuoteItem.QuickQuoteData.Cost;
                var finishedProductCode = crmQuoteItem.QuickQuoteData.FinishedProduct?.ProductCode ?? string.Empty;

                newQuoteItem.QuickQuoteItemFinishedProductCode = Left(finishedProductCode, 120);

                newQuoteItem.QuickQuoteItemLabel = Left(crmQuoteItem.QuickQuoteData.Label, 120);
                newQuoteItem.QuickQuoteItemPieces = crmQuoteItem.QuickQuoteData.OrderQuantity.Pieces;
                newQuoteItem.QuickQuoteItemPrice = crmQuoteItem.QuickQuoteData.Price;
                newQuoteItem.QuickQuoteItemCost = crmQuoteItem.QuickQuoteData.Cost;
                newQuoteItem.QuickQuoteItemQuantity = crmQuoteItem.QuickQuoteData.OrderQuantity.Quantity;
                newQuoteItem.QuickQuoteItemQuantityType = crmQuoteItem.QuickQuoteData.OrderQuantity.QuantityType;
                newQuoteItem.QuickQuoteItemRegret = crmQuoteItem.QuickQuoteData.Regret;
                newQuoteItem.DisplayCurrency = displayCurrency;
                newQuoteItem.ExchangeRate = exchangeRate;
                newQuoteItem.BaseQuickQuoteItemPrice = (exchangeRate > 0 ) ? newQuoteItem.QuickQuoteItemPrice / exchangeRate : newQuoteItem.QuickQuoteItemPrice;
                newQuoteItem.BaseQuickQuoteItemCost = (exchangeRate > 0) ? newQuoteItem.QuickQuoteItemCost / exchangeRate : newQuoteItem.QuickQuoteItemCost;

            }
        }

        private static void GetValuesForNormalCrmQuoteItem(CrmQuoteItem crmQuoteItem, Vulcan_CrmQuoteItem newQuoteItem, 
            string coid, string baseCurrency, string displayCurrency, decimal exchangeRate, decimal exchangeRateBase)
        {

            if ((crmQuoteItem.IsQuickQuoteItem == false) && 
                (crmQuoteItem.IsMachinedPart == false) &&
                (crmQuoteItem.IsCrozCalc == false))
            {
                newQuoteItem.CostPerInch = crmQuoteItem.QuotePrice.MaterialCostValue.MaterialCostPerInch * exchangeRate;
                newQuoteItem.BaseCostPerInch = crmQuoteItem.QuotePrice.MaterialCostValue.MaterialCostPerInch * exchangeRateBase;

                newQuoteItem.CostPerKg = crmQuoteItem.QuotePrice.MaterialCostValue.MaterialCostPerKg * exchangeRate;
                newQuoteItem.BaseCostPerKg = crmQuoteItem.QuotePrice.MaterialCostValue.MaterialCostPerKg * exchangeRateBase;

                newQuoteItem.CostPerPiece = crmQuoteItem.QuotePrice.MaterialCostValue.MaterialCostPerInch *
                                            crmQuoteItem.QuotePrice.RequiredQuantity.TotalInches() * exchangeRate;
                newQuoteItem.BaseCostPerPiece = crmQuoteItem.QuotePrice.MaterialCostValue.MaterialCostPerInch *
                                            crmQuoteItem.QuotePrice.RequiredQuantity.TotalInches() * exchangeRateBase;

                newQuoteItem.CostPerPound = crmQuoteItem.QuotePrice.MaterialCostValue.MaterialCostPerPound * exchangeRate;
                newQuoteItem.BaseCostPerPound = crmQuoteItem.QuotePrice.MaterialCostValue.MaterialCostPerPound * exchangeRateBase;

                newQuoteItem.CutCostPerPiece = crmQuoteItem.CalculateQuotePriceModel.CutCostPerPiece * exchangeRate;
                newQuoteItem.BaseCutCostPerPiece = crmQuoteItem.CalculateQuotePriceModel.CutCostPerPiece * exchangeRateBase;

                newQuoteItem.CutCostPerPieceOverride =
                    crmQuoteItem.CalculateQuotePriceModel.CutCostPerPieceOverride ?? (decimal)0 * exchangeRate;
                newQuoteItem.BaseCutCostPerPieceOverride =
                    crmQuoteItem.CalculateQuotePriceModel.CutCostPerPieceOverride ?? (decimal)0 * exchangeRateBase;

                newQuoteItem.BaseCurrency = baseCurrency;
                newQuoteItem.DisplayCurrency = displayCurrency;
                newQuoteItem.ExchangeRate = exchangeRate;

                var finalMargin = QuoteCalculations.GetMargin(crmQuoteItem.QuotePrice.TotalPrice * exchangeRate,
                    crmQuoteItem.QuotePrice.FinalPrice * exchangeRate);

                if (finalMargin < -1)
                {
                    finalMargin = -1;
                }

                newQuoteItem.FinalMargin = finalMargin;

                newQuoteItem.FinalPrice = crmQuoteItem.QuotePrice.FinalPrice * exchangeRate;
                newQuoteItem.BaseFinalPrice = crmQuoteItem.QuotePrice.FinalPrice * exchangeRateBase;

                newQuoteItem.FinalPriceOverrideType =
                    crmQuoteItem.CalculateQuotePriceModel.FinalPriceOverrideType.ToString() ?? string.Empty;

                newQuoteItem.FinalPriceOverrideValue = crmQuoteItem.CalculateQuotePriceModel.FinalPriceOverrideValue * exchangeRate;
                newQuoteItem.BaseFinalPriceOverrideValue = crmQuoteItem.CalculateQuotePriceModel.FinalPriceOverrideValue * exchangeRateBase;

                newQuoteItem.FinishInsideDiameter = crmQuoteItem.QuotePrice.FinishedProduct.InsideDiameter;
                newQuoteItem.FinishKilograms = crmQuoteItem.QuotePrice.FinishQuantity.TotalKilograms();
                newQuoteItem.FinishMetalCategory = crmQuoteItem.QuotePrice?.FinishedProduct?.MetalCategory ?? string.Empty;
                newQuoteItem.FinishOuterDiameter = crmQuoteItem.QuotePrice.FinishedProduct.OuterDiameter;
                newQuoteItem.FinishPounds = crmQuoteItem.QuotePrice.FinishPounds;
                newQuoteItem.FinishProductCode = crmQuoteItem.QuotePrice.FinishedProduct.ProductCode ?? string.Empty;
                newQuoteItem.FinishProductCoid = crmQuoteItem.QuotePrice.FinishedProduct.Coid ?? string.Empty;
                newQuoteItem.FinishProductCondition = crmQuoteItem.QuotePrice.FinishedProduct.ProductCondition ?? string.Empty;
                newQuoteItem.FinishProductId = crmQuoteItem.QuotePrice.FinishedProduct?.ProductId ?? 0;
                newQuoteItem.FinishProductType = crmQuoteItem.QuotePrice.FinishedProduct.ProductType ?? string.Empty;
                newQuoteItem.FinishStockGrade = crmQuoteItem.QuotePrice.FinishedProduct.StockGrade ?? string.Empty;

                var finishedStratificationRank = crmQuoteItem.QuotePrice.FinishedProduct.StratificationRank;
                if (finishedStratificationRank == "unknown") finishedStratificationRank = "?";
                newQuoteItem.FinishedStratificationRank = finishedStratificationRank;


                if (crmQuoteItem.QuotePrice.FinishedProduct.ProductId > 0)
                {
                    var fullProduct = crmQuoteItem.QuotePrice.FinishedProduct.GetProductMasterFull();
                    newQuoteItem.FinishedProductCategory = fullProduct?.ProductCategory ?? "";
                }


                newQuoteItem.KerfTotalCost = crmQuoteItem.QuotePrice.MaterialCostValue.KerfTotalCost;
                newQuoteItem.BaseKerfTotalCost = crmQuoteItem.QuotePrice.MaterialCostValue.KerfTotalCost * exchangeRateBase;

                newQuoteItem.KerfTotalPrice = crmQuoteItem.QuotePrice.MaterialPriceValue.KerfTotalPrice;
                newQuoteItem.BaseKerfTotalPrice = crmQuoteItem.QuotePrice.MaterialPriceValue.KerfTotalPrice * exchangeRateBase;

                newQuoteItem.KerfInchesPerCut = crmQuoteItem.QuotePrice.MaterialCostValue.KurfInchesPerCut;

                //newQuoteItem.KurfTotalPrice = crmQuoteItem.QuotePrice.MaterialPriceValue.KerfTotalPrice;
                newQuoteItem.Margin = crmQuoteItem.QuotePrice.MaterialPriceValue.Margin;
                newQuoteItem.MarginOverride = crmQuoteItem.QuotePrice.MaterialPriceValue.MarginOverride;

                newQuoteItem.MaterialOnlyCost = crmQuoteItem.QuotePrice.MaterialOnlyCost * exchangeRate;
                newQuoteItem.BaseMaterialOnlyCost = crmQuoteItem.QuotePrice.MaterialOnlyCost * exchangeRateBase;

                newQuoteItem.MaterialTotalPrice = crmQuoteItem.QuotePrice.TotalMaterialPrice * exchangeRate;
                newQuoteItem.BaseMaterialTotalPrice = crmQuoteItem.QuotePrice.TotalMaterialPrice * exchangeRateBase;

                newQuoteItem.MaterialTotalPriceOverride = crmQuoteItem.CalculateQuotePriceModel.MaterialPriceValue
                    .MaterialTotalPriceOverride * exchangeRate;
                newQuoteItem.BaseMaterialTotalPriceOverride = crmQuoteItem.CalculateQuotePriceModel.MaterialPriceValue
                                                              .MaterialTotalPriceOverride * exchangeRateBase;


                newQuoteItem.Pieces = crmQuoteItem.QuotePrice.FinishQuantity.Pieces;

                newQuoteItem.PricePerEach = crmQuoteItem.QuotePrice.PricePerEach * exchangeRate;
                newQuoteItem.BasePricePerEach = crmQuoteItem.QuotePrice.PricePerEach * exchangeRateBase;

                newQuoteItem.PricePerFoot = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerFoot * exchangeRate;
                newQuoteItem.BasePricePerFoot = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerFoot * exchangeRateBase;

                newQuoteItem.PricePerFootOverride = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerFootOverride * exchangeRate;
                newQuoteItem.BasePricePerFootOverride = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerFootOverride * exchangeRateBase;

                newQuoteItem.PricePerInch = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerInch * exchangeRate;
                newQuoteItem.BasePricePerInch = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerInch * exchangeRateBase;

                newQuoteItem.PricePerInchOverride = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerInchOverride * exchangeRate;
                newQuoteItem.BasePricePerInchOverride = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerInchOverride * exchangeRateBase;

                newQuoteItem.PricePerKilogram = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerKilogram * exchangeRate;
                newQuoteItem.BasePricePerKilogram = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerKilogram * exchangeRateBase;

                newQuoteItem.PricePerKilogramOverride =
                    crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerKilogramOverride * exchangeRate;
                newQuoteItem.BasePricePerKilogramOverride =
                    crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerKilogramOverride * exchangeRateBase;

                newQuoteItem.PricePerPound = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerPound * exchangeRate;
                newQuoteItem.BasePricePerPound = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerPound * exchangeRateBase;

                newQuoteItem.PricePerPoundOverride = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerPoundOverride * exchangeRate;
                newQuoteItem.BasePricePerPoundOverride = crmQuoteItem.QuotePrice.MaterialPriceValue.PricePerPoundOverride * exchangeRateBase;

                newQuoteItem.ProductionCostTotal = crmQuoteItem.QuotePrice.ProductionCostTotal * exchangeRate;
                newQuoteItem.BaseProductionCostTotal = crmQuoteItem.QuotePrice.ProductionCostTotal * exchangeRateBase;

                newQuoteItem.ProductionPriceTotal = crmQuoteItem.QuotePrice.ProductionPriceTotal * exchangeRate;
                newQuoteItem.BaseProductionPriceTotal = crmQuoteItem.QuotePrice.ProductionPriceTotal * exchangeRateBase;

                

                newQuoteItem.StartingInsideDiameter = crmQuoteItem.QuotePrice.StartingProduct.InsideDiameter;
                newQuoteItem.StartingKilograms = crmQuoteItem.QuotePrice.StartingKilograms;
                newQuoteItem.StartingMetalCategory = crmQuoteItem.QuotePrice?.StartingProduct?.MetalCategory ?? string.Empty;
                newQuoteItem.StartingOuterDiameter = crmQuoteItem.QuotePrice.StartingProduct.OuterDiameter;
                newQuoteItem.StartingPounds = crmQuoteItem.QuotePrice.StartingPounds;
                newQuoteItem.StartingProductCode = crmQuoteItem.QuotePrice.StartingProduct.ProductCode ?? string.Empty;
                newQuoteItem.StartingProductCoid = crmQuoteItem.QuotePrice.StartingProduct.Coid ?? string.Empty;
                newQuoteItem.StartingProductCondition = crmQuoteItem.QuotePrice.StartingProduct.ProductCondition ?? string.Empty;
                newQuoteItem.StartingProductId = crmQuoteItem.QuotePrice.StartingProduct?.ProductId ?? 0;
                newQuoteItem.StartingProductType = crmQuoteItem.QuotePrice.StartingProduct.ProductType ?? string.Empty;
                newQuoteItem.StartingStockGrade = crmQuoteItem.QuotePrice.StartingProduct.StockGrade ?? string.Empty;

                newQuoteItem.StartingProductCategory = crmQuoteItem.QuotePrice.StartingProduct.ProductCategory;

                var startingStratificationRank = crmQuoteItem.QuotePrice.StartingProduct.StratificationRank;
                if (startingStratificationRank == "unknown") startingStratificationRank = "?";
                newQuoteItem.StartingStratificationRank = startingStratificationRank;

                if (crmQuoteItem.QuotePrice.StartingProduct.ProductId > 0)
                {
                    var fullProduct = crmQuoteItem.QuotePrice.StartingProduct.GetProductMasterFull();
                    newQuoteItem.StartingProductCategory = fullProduct?.ProductCategory ?? "";
                }
                newQuoteItem.TagNumber = crmQuoteItem.QuotePrice?.MaterialCostValue?.BaseCost?.TagNumber;
                newQuoteItem.HeatNumber = crmQuoteItem.QuotePrice.StartingProduct.HeatNumber;
                newQuoteItem.TestPieceInches = crmQuoteItem.QuotePrice.TestPiecesInches;

                newQuoteItem.TestPiecesTotalPrice = crmQuoteItem.QuotePrice.MaterialPriceValue.TestPiecesTotalPrice * exchangeRate;
                newQuoteItem.BaseTestPiecesTotalPrice = crmQuoteItem.QuotePrice.MaterialPriceValue.TestPiecesTotalPrice;

                newQuoteItem.TheoWeight = crmQuoteItem.QuotePrice.StartingProduct.TheoWeight;

                newQuoteItem.TotalCost = crmQuoteItem.QuotePrice.TotalCost * exchangeRate;
                newQuoteItem.BaseTotalCost = crmQuoteItem.QuotePrice.TotalCost * exchangeRateBase;

                newQuoteItem.TotalCutCost = crmQuoteItem.QuotePrice.TotalCutCost * exchangeRate;
                newQuoteItem.BaseTotalCutCost = crmQuoteItem.QuotePrice.TotalCutCost * exchangeRateBase;

                newQuoteItem.TotalFeet = crmQuoteItem.QuotePrice.FinishQuantity.TotalFeet();
                newQuoteItem.TotalInches = crmQuoteItem.QuotePrice.FinishQuantity.TotalInches();
                newQuoteItem.TotalKilograms = crmQuoteItem.QuotePrice.FinishQuantity.TotalKilograms();
                newQuoteItem.TotalPounds = crmQuoteItem.QuotePrice.FinishQuantity.TotalPounds();
                newQuoteItem.LostProductCode = crmQuoteItem.LostProductCode ?? string.Empty;
                newQuoteItem.RequestedProductCode = (crmQuoteItem.RequestedProductCode != null) ? Left(crmQuoteItem.RequestedProductCode,60) : string.Empty;
                newQuoteItem.LostDateTimeUTC = crmQuoteItem.LostDate?.ToUniversalTime();
                newQuoteItem.StartingProductSize = crmQuoteItem.QuotePrice?.StartingProduct?.ProductSize ?? string.Empty;
                newQuoteItem.FinishProductSize = crmQuoteItem.QuotePrice?.FinishedProduct?.ProductSize ??string.Empty;
            }

            if ((crmQuoteItem.IsQuickQuoteItem) && (crmQuoteItem.QuickQuoteData != null))
            {
                if (crmQuoteItem.QuickQuoteData.StartingProduct != null)
                {
                    newQuoteItem.StartingProductCode = crmQuoteItem.QuickQuoteData.StartingProduct.ProductCode;
                    newQuoteItem.StartingInsideDiameter = crmQuoteItem.QuickQuoteData.StartingProduct.InsideDiameter;
                    newQuoteItem.StartingProductCategory = crmQuoteItem.QuickQuoteData.StartingProduct.ProductCategory;
                    newQuoteItem.StartingMetalCategory = crmQuoteItem.QuickQuoteData.StartingProduct.MetalCategory ?? string.Empty;
                    newQuoteItem.StartingOuterDiameter = crmQuoteItem.QuickQuoteData.StartingProduct.OuterDiameter;
                    newQuoteItem.StartingProductCode = crmQuoteItem.QuickQuoteData.StartingProduct.ProductCode ?? string.Empty;
                    newQuoteItem.StartingProductCoid = crmQuoteItem.QuickQuoteData.StartingProduct.Coid ?? string.Empty;
                    newQuoteItem.StartingProductCondition = crmQuoteItem.QuickQuoteData.StartingProduct.ProductCondition ?? string.Empty;
                    newQuoteItem.StartingProductId = crmQuoteItem.QuickQuoteData.StartingProduct?.ProductId ?? 0;
                    newQuoteItem.StartingProductType = crmQuoteItem.QuickQuoteData.StartingProduct.ProductType ?? string.Empty;
                    newQuoteItem.StartingStockGrade = crmQuoteItem.QuickQuoteData.StartingProduct.StockGrade ?? string.Empty;

                    var startingStratRankForQuickQuoteItem = crmQuoteItem.QuickQuoteData.StartingProduct.StratificationRank;
                    if (startingStratRankForQuickQuoteItem == "unknown")
                        startingStratRankForQuickQuoteItem = "?";
                    newQuoteItem.StartingStratificationRank =
                        startingStratRankForQuickQuoteItem;

                }

            }

        }

        private static string GetBaseCurrencyForCoid(string coid)
        {
            if (coid == "EUR") return "GBP";
            if (coid == "CAN") return "CAN";
            return "USD";
        }
        private static string GetBaseCurrencyForQuoteItem(CrmQuote quote, CrmQuoteItem quoteItem)
        {
            var coid = quoteItem.Coid;
            if (quoteItem.QuoteSource == QuoteSource.StockItem)
            {
                coid = quoteItem.QuotePrice.StartingProduct.Coid;
            }
            else if (quoteItem.QuoteSource == QuoteSource.MachinedPart)
            {
                coid = quoteItem.MachinedPartModel.Coid;
            }
            else if (quoteItem.QuoteSource == QuoteSource.MadeUpCost)
            {
                coid = quoteItem.QuotePrice.StartingProduct.Coid;
            }
            else if (quoteItem.QuoteSource == QuoteSource.QuickQuoteItem)
            {
                coid = quote.Coid;
            }
            else if (quoteItem.QuoteSource == QuoteSource.NonStockItem)
            {
                coid = quoteItem.QuotePrice.StartingProduct.Coid;
            }
            else if (quoteItem.QuoteSource == QuoteSource.PurchaseOrderItem)
            {
                coid = quoteItem.QuotePrice.StartingProduct.Coid;
            }

            if (coid == "EUR") return "GBP";
            if (coid == "CAN") return "CAD";
            return "USD";
        }

        private static string GetDisplayCurrencyForQuote(CrmQuote quote)
        {
            var displayCurrency = quote.DisplayCurrency;

            if (String.IsNullOrEmpty(displayCurrency))
            {
                var coid = quote.Coid;

                if (coid == "EUR") displayCurrency = "GBP";
                if (coid == "CAN") displayCurrency = "CAD";
                displayCurrency = "USD";
            }

            return displayCurrency;
        }


        private static Vulcan_CrmQuoteItem CreateNewVulcanCrmQuoteItem(CrmQuote crmQuote, CrmQuoteItem crmQuoteItem, string baseCurrency, string displayCurrency)
        {
            var poNumber = ((String.IsNullOrEmpty(crmQuoteItem.PoNumber)) ? crmQuote.PoNumber : crmQuoteItem.PoNumber) ?? String.Empty;

            var newQuoteItem = new Vulcan_CrmQuoteItem()
            {
                QuoteItemId = crmQuoteItem.Index,
                QuoteId = crmQuote.QuoteId,
                QuoteSource = crmQuoteItem.QuoteSource.ToString(),
                SalesPersonNotes = crmQuoteItem.SalesPersonNotes,
                CreateDateTimeUTC = crmQuoteItem.CreateDateTime.ToUniversalTime(),
                Currency = crmQuoteItem.BaseCurrency,
                CustomerNotes = crmQuoteItem.CustomerNotes,
                IsCostMadeup = crmQuoteItem.QuoteSource == QuoteSource.MadeUpCost,
                IsLost = crmQuoteItem.IsLost,
                IsQuickQuoteItem = crmQuoteItem.IsQuickQuoteItem,
                ModifiedDateTimeUTC = crmQuoteItem.ModifiedDateTime.ToUniversalTime(),
                OemType = crmQuoteItem.OemType,
                PartNumber = crmQuoteItem.PartNumber.TrimEnd(),
                PartSpecification = Left(crmQuoteItem.PartSpecification.TrimEnd(),1000),

                PoNumber = Left(poNumber,40),
                LeadTime = Left(crmQuoteItem.LeadTime.TrimEnd(),100),
                LostProductCode = crmQuoteItem.LostProductCode ?? string.Empty,
                LostReason = (crmQuoteItem.LostReasonId != string.Empty)
                    ? new RepositoryBase<LostReason>().Find(crmQuoteItem.LostReasonId).Reason
                    : string.Empty,
                LostComments = LeftString(crmQuoteItem.LostComments, 500),

                QuickQuoteItemLabel = string.Empty,
                QuickQuoteItemFinishedProductCode = string.Empty,
                QuickQuoteItemQuantityType = string.Empty,

                StartingMetalCategory = string.Empty,
                StartingProductCode = string.Empty,
                StartingProductCoid = string.Empty,
                StartingProductCondition = string.Empty,
                StartingProductType = string.Empty,
                StartingStockGrade = string.Empty,
                TagNumber = string.Empty,
                HeatNumber = string.Empty,

                FinishMetalCategory = string.Empty,
                FinishProductCode = string.Empty,
                FinishProductCoid = string.Empty,
                FinishProductCondition = string.Empty,
                FinishProductType = string.Empty,
                FinishStockGrade = string.Empty,
                FinalPriceOverrideType =  string.Empty,
                RequestedProductCode = string.Empty,
                DisplayCurrency = displayCurrency,
                BaseCurrency = baseCurrency,
                ImportDateTimeUTC = DateTime.Now.ToUniversalTime(),

            };
            
            return newQuoteItem;
        }

        private List<CrmQuoteItem> AddQuote(ODSContext context, CrmQuote crmQuote)
        {
            var exportStatus = crmQuote.ExportStatus.ToString();

            DateTime? expiredDate = (crmQuote.Status == DAL.Vulcan.Mongo.Models.PipelineStatus.Expired)
                ? crmQuote.SubmitDate?.AddDays(crmQuote.ValidityDays).ToUniversalTime()
                : null;


            var quoteItems = crmQuote.Items.Select(x => x.AsQuoteItem()).ToList();


            var newQuote = new Vulcan_CrmQuote()
            {
                Coid = crmQuote.Coid,
                ObjectId = crmQuote.Id.ToString(),
                QuoteLinkId = crmQuote.QuoteLinkId.ToString(),
                QuoteLinkType = (crmQuote.QuoteLinkType == QuoteLinkType.Repeat) ? "R" : "O",
                CompanyCode = crmQuote.Company?.Code ?? string.Empty,
                CompanyName = crmQuote.Company?.Name ?? string.Empty,
                CreateDateTimeUTC = crmQuote.CreateDateTime.ToUniversalTime(),
                CustomerNotes = crmQuote.CustomerNotes ?? string.Empty,
                ExportIMetalStatus = crmQuote.ExportStatus.ToString(),
                ExportRequestedBy = (crmQuote.ExportRequestedBy == null) ? string.Empty : crmQuote.ExportRequestedBy.FullName,
                FreightTerms = crmQuote.FreightTerm ?? string.Empty,
                IsLost = crmQuote.IsLost,
                IsProspect = crmQuote.IsProspect,
                LostReason = (crmQuote.LostReasonId == string.Empty)
                    ? string.Empty
                    : new RepositoryBase<LostReason>().Find(crmQuote.LostReasonId).Reason,
                LostComments = LeftString(crmQuote.LostComments,500),
                ModifiedDateTimeUTC = crmQuote.ModifiedDateTime.ToUniversalTime(),
                PaymentTerms = Left(crmQuote.PaymentTerm,50),
                ProspectName = crmQuote.Prospect?.Name ?? string.Empty,
                QuoteId = crmQuote.QuoteId,
                QuoteStatus = crmQuote.Status.ToString(),
                RevisionNumber = crmQuote.RevisionNumber,
                RfqNumber = LeftString(crmQuote.RfqNumber,200),
                SalesGroupCode = crmQuote.SalesGroupCode ?? "",
                SalesOrderId = (String.IsNullOrEmpty(crmQuote.ExternalOrderId)) ? 0 : int.Parse(crmQuote.ExternalOrderId),
                SalesPerson = crmQuote.SalesPerson.FullName,
                SalesPersonNotes = crmQuote.SalesPersonNotes ?? string.Empty,
                SubmitDateTimeUTC = crmQuote.SubmitDate?.ToUniversalTime() ?? new DateTime(1980, 1, 1).ToUniversalTime(),
                TeamName = crmQuote.Team.Name,
                ValidityDays = crmQuote.ValidityDays,
                WonDateTimeUTC = crmQuote.WonDate?.ToUniversalTime() ?? new DateTime(1980, 1, 1).ToUniversalTime(),     
                ReportDateTimeUTC = crmQuote.ReportDate?.ToUniversalTime() ?? new DateTime(1980,1,1).ToUniversalTime(),
                ImportDateTimeUTC = DateTime.Now.ToUniversalTime(),
                LostDateTimeUTC = crmQuote.LostDate?.ToUniversalTime(),
                ExpiredDateTimeUTC = expiredDate,
                Bid = crmQuote.Bid
            };

            if (crmQuote.Contact != null)
            {
                var contact = crmQuote.Contact.AsContact();
                newQuote.ContactFirstName = LeftString(contact.Person.FirstName, 30);
                newQuote.ContactLastName = LeftString(contact.Person.LastName, 30);
                newQuote.ContactMiddleName = LeftString(contact.Person.MiddleName, 30);
                newQuote.ContactEmailAddress = LeftString(contact.Person.EmailAddresses.FirstOrDefault()?.Address, 50);
                newQuote.ContactPhoneNumber = LeftString(contact.Person.PhoneNumbers.FirstOrDefault()?.Number, 30);
            }

            //if ((!crmQuote.IsProspect) && (crmQuote.Company != null))
            //{
            //    var company = crmQuote.Company.AsCompany();
            //    newQuote.OrderClassificationCode = company.OrderClassificationCode;
            //    newQuote.OrderClassificationDescription = company.OrderClassificationDescription;

            //}

            context.Vulcan_CrmQuote.Add(newQuote);
            context.SaveChanges();

            return quoteItems;

        }

        private static void RemoveExistingRow(ODSContext context, CrmQuote crmQuote)
        {
            var existingRow = context.Vulcan_CrmQuote.SingleOrDefault(x => x.QuoteId == crmQuote.QuoteId);
            if (existingRow != null)
            {
                context.Vulcan_CrmQuote.Remove(existingRow);
            }

            var removeItems = context.Vulcan_CrmQuoteItem.Where(x => x.QuoteId == crmQuote.QuoteId).ToList();
            foreach (var quoteItem in removeItems)
            {
                context.Vulcan_CrmQuoteItem.Remove(quoteItem);
            }
            context.SaveChanges();

            //var removeProdCosts = context.Vulcan_CrmQuoteItem_ProductionCost.Where()

        }

        private static string LeftString(string value, int length)
        {
            if (value == null) return String.Empty;

            if (value.Length > length) return value.Substring(0, length);

            return value;

        }


    }
}
