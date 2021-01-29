using BLL.EMail.Core;
using DAL.iMetal.Core.Queries;
using DAL.Vulcan.Mongo.Core.MetalogicsCacheData;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.Context;

namespace SVC.iMetal.CacheBuilder
{
    public class CacheBuilderWorker
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CacheBuilderWorker));

        public async Task ExecuteAsync(CancellationTokenSource stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                Logger.Info($"Worker running at: {DateTimeOffset.Now}");
                await LoadStockItems();
                await LoadIncoming();
                await LoadProductMasters();
            }
        }

        private async Task<bool> LoadProductMasters()
        {
            try
            {
                Logger.Info($"Starting ProductMasters query");
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var coidList = new List<string>() { "INC", "CAN", "EUR", "MSA", "DUB", "SIN" };
                var productMasters = new List<ProductMastersQuery>();
                foreach (var coid in coidList)
                {
                    productMasters.AddRange(await ProductMastersQuery.ExecuteAsync(coid));
                }

                sw.Stop();
                Logger.Info($"Elapsed {sw.Elapsed} - {productMasters.Count} read from iMetal");

                sw.Start();

                AddValues(productMasters);
                
                Logger.Info($"Elapsed {sw.Elapsed} - {productMasters.Count} rows written to ProductMastersCache");
                sw.Stop();

                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                var emailRecipients = new List<string>()
                {
                    "Isidro.Gallegos@howcogroup.com",
                    "Marc.Pike@howcogroup.com"
                };
                EMailSupport.SendEMailToSupportException(
                    EnvironmentSettings.CurrentEnvironment.ToString()+ 
                    ": SVC.iMetal.CacheBuilder exception ProductMasters", 
                    emailRecipients, e);
                Logger.Info("Skipping this load due to error");

            }

            return true;
        }

        private async Task<bool> LoadIncoming()
        {
            try
            {
                Logger.Info($"Starting PurchaseOrderItems query");
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var coidList = new List<string>() { "INC", "CAN", "EUR", "MSA", "DUB", "SIN" };
                var purchaseOrders = new List<PurchaseOrderItemsQuery>();
                foreach (var coid in coidList)
                {
                    purchaseOrders.AddRange(await PurchaseOrderItemsQuery.ExecuteAsync(coid));
                }

                sw.Stop();
                Logger.Info($"Elapsed {sw.Elapsed} - {purchaseOrders.Count} read from iMetal");

                sw.Start();
                AddValues(purchaseOrders);
                sw.Stop();

                Logger.Info($"Elapsed {sw.Elapsed} - {purchaseOrders.Count} rows written to PurchaseOrderItemsCache");
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                var emailRecipients = new List<string>()
                {
                    "Isidro.Gallegos@howcogroup.com",
                    "Marc.Pike@howcogroup.com"
                };
                EMailSupport.SendEMailToSupportException(
                    EnvironmentSettings.CurrentEnvironment.ToString() + 
                    " :SVC.iMetal.CacheBuilder exception PurchaseOrders", 
                    emailRecipients, e);
                Logger.Info("Skipping this load due to error");

            }

            return true;
        }


        private async Task<bool> LoadStockItems()
        {
            try
            {
                Logger.Info($"Starting StockItems query");
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var coidList = new List<string>() { "INC", "CAN", "EUR", "MSA", "DUB", "SIN" };
                var stockItems = new List<StockItemsQuery>();
                foreach (var coid in coidList)
                {
                    stockItems.AddRange(await StockItemsQuery.ExecuteAsync(coid));
                }

                sw.Stop();
                Logger.Info($"Elapsed {sw.Elapsed} - {stockItems.Count} read from iMetal");

                sw.Start();
                AddValues(stockItems);
                sw.Stop();
                Logger.Info($"Elapsed {sw.Elapsed} - {stockItems.Count} rows written to StockItemsCache");
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                var emailRecipients = new List<string>()
                {
                    "Isidro.Gallegos@howcogroup.com",
                    "Marc.Pike@howcogroup.com"
                };
                EMailSupport.SendEMailToSupportException(
                    EnvironmentSettings.CurrentEnvironment.ToString() + 
                    ": SVC.iMetal.CacheBuilder exception StockItems", 
                    emailRecipients, e);
                Logger.Info("Skipping this load due to error");
            }

            return true;
        }

        private static void AddValues(List<PurchaseOrderItemsQuery> values)
        {

            try
            {
                var newCacheId = Guid.NewGuid();
                PurchaseOrderItemsCache.Helper.InsertMany(
                    values.Select(x => new PurchaseOrderItemsCache()
                    {
                        CacheId = newCacheId,
                        Coid = x.Coid,
                        Status = x.Status,
                        PurchaseOrderHeaderId = x.PurchaseOrderHeaderId,
                        PoNumber = x.PoNumber,
                        ItemNumber = x.ItemNumber,
                        PurchaseOrderItemId = x.PurchaseOrderItemId,
                        CreateDate = x.CreateDate,
                        MetalCategory = x.MetalCategory,
                        MetalType = x.MetalType,
                        StockType = x.StockType,
                        Location = x.Location,
                        ProductId = x.ProductId,
                        ProductCode = x.ProductCode,
                        StockGrade = x.StockGrade,
                        ProductCondition = x.ProductCondition,
                        ProductCategory = x.ProductCategory,
                        Width = x.Width,
                        Length = x.Length,
                        Thick = x.Thick,
                        InsideDiameter = x.InsideDiameter,
                        OuterDiameter = x.OuterDiameter,
                        Density = x.Density,
                        ProductControlCode = x.ProductControlCode,
                        ControlPieces = x.ControlPieces,
                        VolumeDensity = x.VolumeDensity,
                        Dim1StaticDimension = x.Dim1StaticDimension,
                        Dim2StaticDimension = x.Dim2StaticDimension,
                        Dim3StaticDimension = x.Dim3StaticDimension,
                        OrderedPiecesUnit = x.OrderedPiecesUnit,
                        OrderedLengthUnit = x.OrderedLengthUnit,
                        OrderedWeightUnit = x.OrderedWeightUnit,
                        OrderedQuantityUnit = x.OrderedQuantityUnit,
                        OrderedPieces = x.OrderedPieces,
                        OrderedQuantity = x.OrderedQuantity,
                        OrderedLength = x.OrderedLength,
                        OrderedWeight = x.OrderedWeight,
                        OrderedWeightLbs = x.OrderedWeightLbs,
                        OrderedWeightKgs = x.OrderedWeightKgs,
                        AllocatedPiecesUnit = x.AllocatedPiecesUnit,
                        AllocatedLengthUnit = x.AllocatedLengthUnit,
                        AllocatedWeightUnit = x.AllocatedWeightUnit,
                        AllocatedQuantityUnit = x.AllocatedQuantityUnit,
                        AllocatedPieces = x.AllocatedPieces,
                        AllocatedQuantity = x.AllocatedQuantity,
                        AllocatedLength = x.AllocatedLength,
                        AllocatedWeight = x.AllocatedWeight,
                        AllocatedWeightLbs = x.AllocatedWeightLbs,
                        AllocatedWeightKgs = x.AllocatedWeightKgs,
                        DeliveredPiecesUnit = x.DeliveredPiecesUnit,
                        DeliveredLengthUnit = x.DeliveredLengthUnit,
                        DeliveredWeightUnit = x.DeliveredWeightUnit,
                        DeliveredQuantityUnit = x.DeliveredQuantityUnit,
                        DeliveredPieces = x.DeliveredPieces,
                        DeliveredQuantity = x.DeliveredQuantity,
                        DeliveredLength = x.DeliveredLength,
                        DeliveredWeight = x.DeliveredWeight,
                        DeliveredWeightLbs = x.DeliveredWeightLbs,
                        DeliveredWeightKgs = x.DeliveredWeightKgs,
                        BalancePiecesUnit = x.BalancePiecesUnit,
                        BalanceLengthUnit = x.BalanceLengthUnit,
                        BalanceWeightUnit = x.BalanceWeightUnit,
                        BalanceQuantityUnit = x.BalanceQuantityUnit,
                        BalancePieces = x.BalancePieces,
                        BalanceQuantity = x.BalanceQuantity,
                        BalanceLength = x.BalanceLength,
                        BalanceWeight = x.BalanceWeight,
                        BalanceWeightLbs = x.BalanceWeightLbs,
                        BalanceWeightKgs = x.BalanceWeightKgs,
                        MaterialCostTotal = x.MaterialCostTotal,
                        ProductionCostTotal = x.ProductionCostTotal,
                        TransportCostTotal = x.TransportCostTotal,
                        SurchargeCostTotal = x.SurchargeCostTotal,
                        MiscellaneousCostTotal = x.MiscellaneousCostTotal,
                        BaseMaterialValue = x.BaseMaterialValue,
                        BaseProductionValue = x.BaseProductionValue,
                        BaseTransportValue = x.BaseTransportValue,
                        BaseSurchargeValue = x.BaseSurchargeValue,
                        BaseMiscellaneousValue = x.BaseMiscellaneousValue,
                        TotalValue = x.TotalValue,
                        TotalCost = x.TotalCost,
                        CostPerLb = x.CostPerLb,
                        CostPerKg = x.CostPerKg,
                        ProductDensity = x.ProductDensity,
                        CostPerInch = x.CostPerInch,
                        PurchaseType = x.PurchaseType,
                        TransferType = x.TransferType,
                        RequestType = x.RequestType,
                        PurchaseCategoryCode = x.PurchaseCategoryCode,
                        PurchaseCategoryDescription = x.PurchaseCategoryDescription,
                        WarehouseCode = x.WarehouseCode,
                        WarehouseShortName = x.WarehouseShortName,
                        WarehouseName = x.WarehouseName,
                        InternalStatusId = x.InternalStatusId,
                        StatusDescription = x.StatusDescription,
                        StatusCode = x.StatusCode,
                        Supplier = x.Supplier,
                        Buyer = x.Buyer,
                        StratificationRank = x.StratificationRank,
                        DueDate = x.DueDate,
                        ItemDueDate = x.ItemDueDate,
                        FactorForKgs = x.FactorForKgs,
                        FactorForLbs = x.FactorForLbs,
                        ProductSize = x.ProductSize,
                        TheoWeight = x.TheoWeight,
                        ProductType = x.ProductType
                    }).ToArray());

                CacheSettings.SetPurchaseOrderItemsCacheId(newCacheId);
                PurchaseOrderItemsCache.Helper.DeleteMany(PurchaseOrderItemsCache.Helper.FilterBuilder.Where(x => x.CacheId != newCacheId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AddValues(List<StockItemsQuery> values)
        {

            try
            {
                var newCacheId = Guid.NewGuid();
                StockItemsCache.Helper.InsertMany(
                    values.Select(x => new StockItemsCache()
                    {
                        CacheId = newCacheId,
                        Coid = x.Coid,
                        ProductId = x.ProductId,
                        StockItemId = x.StockItemId,
                        CreateDate = x.CreateDate,
                        TagNumber = x.TagNumber,
                        HeatNumber = x.HeatNumber,
                        MetalCategory = x.MetalCategory,
                        MetalType = x.MetalType,
                        StockType = x.StockType,
                        Notes = x.Notes,
                        Location = x.Location,
                        WarehouseCode = x.WarehouseCode,
                        WarehouseName = x.WarehouseName,
                        WarehouseShortName = x.WarehouseShortName,
                        ProductCode = x.ProductCode,
                        StockGrade = x.StockGrade,
                        ProductCondition = x.ProductCondition,
                        ProductCategory = x.ProductCategory,
                        BaseCurrency = x.BaseCurrency,
                        ProductSize = x.ProductSize,
                        MillCode = x.MillCode,
                        MillName = x.MillName,
                        ProductControlCode = x.ProductControlCode,
                        ControlPieces = x.ControlPieces,
                        VolumeDensity = x.VolumeDensity,
                        Dim1StaticDimension = x.Dim1StaticDimension,
                        Dim2StaticDimension = x.Dim2StaticDimension,
                        Dim3StaticDimension = x.Dim3StaticDimension,
                        Width = x.Width,
                        Length = x.Length,
                        InsideDiameter = x.InsideDiameter,
                        OuterDiameter = x.OuterDiameter,
                        Density = x.Density,
                        StockHoldReason = x.StockHoldReason,
                        StockHoldUser = x.StockHoldUser,
                        PhysicalPieces = x.PhysicalPieces,
                        AllocatedPieces = x.AllocatedPieces,
                        AvailablePieces = x.AvailablePieces,
                        PiecesUnit = x.PiecesUnit,
                        PhysicalQuantity = x.PhysicalQuantity,
                        AllocatedQuantity = x.AllocatedQuantity,
                        AvailableQuantity = x.AvailableQuantity,
                        QuantityUnit = x.QuantityUnit,
                        PhysicalLength = x.PhysicalLength,
                        AllocatedLength = x.AllocatedLength,
                        AvailableLength = x.AvailableLength,
                        LengthUnit = x.LengthUnit,
                        PhysicalWeight = x.PhysicalWeight,
                        PhysicalWeightLbs = x.PhysicalWeightLbs,
                        PhysicalWeightKgs = x.PhysicalWeightKgs,
                        AllocatedWeight = x.AllocatedWeight,
                        AllocatedWeightLbs = x.AllocatedWeightLbs,
                        AllocatedWeightKgs = x.AllocatedWeightKgs,
                        AvailableWeight = x.AvailableWeight,
                        AvailableWeightLbs = x.AvailableWeightLbs,
                        AvailableWeightKgs = x.AvailableWeightKgs,
                        WeightUnit = x.WeightUnit,
                        MaterialCostTotal = x.MaterialCostTotal,
                        ProductionCostTotal = x.ProductionCostTotal,
                        TransportCostTotal = x.TransportCostTotal,
                        SurchargeCostTotal = x.SurchargeCostTotal,
                        MiscellaneousCostTotal = x.MiscellaneousCostTotal,
                        TotalCost = x.TotalCost,
                        AvailableInches = x.AvailableInches,
                        CostPerInch = x.CostPerInch,
                        CostPerWeight = x.CostPerWeight,
                        CostPerQty = x.CostPerQty,
                        StratificationRank = x.StratificationRank,
                        CostPerLb = x.CostPerLb,
                        CostPerKg = x.CostPerKg,
                        ProductDensity = x.ProductDensity,
                        ReceivedDate = x.ReceivedDate,
                        IsMachinedPart = x.IsMachinedPart,
                        IsZeroWeightService = x.IsZeroWeightService,
                        PieceCost = x.PieceCost,
                        TheoWeight = x.TheoWeight,
                        ProductType = x.ProductType,
                        FactorForLbs = x.FactorForLbs,
                        FactorForKgs = x.FactorForKgs
                    }).ToArray());

                CacheSettings.SetStockItemsCacheId(newCacheId);

                StockItemsCache.Helper.DeleteMany(StockItemsCache.Helper.FilterBuilder.Where(x => x.CacheId != newCacheId));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AddValues(List<ProductMastersQuery> values)
        {

            try
            {
                var newCacheId = Guid.NewGuid();
                ProductMastersCache.Helper.InsertMany(
                    values.Select(x => new ProductMastersCache()
                    {
                        CacheId = newCacheId,
                        Coid = x.Coid,
                        ProductId = x.ProductId,
                        MetalCategory = x.MetalCategory,
                        MetalType = x.MetalType,
                        StockType = x.StockType,
                        ProductCode = x.ProductCode,
                        StockGrade = x.StockGrade,
                        ProductCondition = x.ProductCondition,
                        ProductCategory = x.ProductCategory,
                        ProductSize = x.ProductSize,
                        ProductControlCode = x.ProductControlCode,
                        ControlPieces = x.ControlPieces,
                        VolumeDensity = x.VolumeDensity,
                        Dim1StaticDimension = x.Dim1StaticDimension,
                        Dim2StaticDimension = x.Dim2StaticDimension,
                        Dim3StaticDimension = x.Dim3StaticDimension,
                        Width = x.Width,
                        Length = x.Length,
                        InsideDiameter = x.InsideDiameter,
                        OuterDiameter = x.OuterDiameter,
                        Density = x.Density,
                        StratificationRank = x.StratificationRank,
                        ProductDensity = x.ProductDensity,
                        TheoWeight = x.TheoWeight,
                        ProductType = x.ProductType,
                        FactorForLbs = x.FactorForLbs,
                        FactorForKgs = x.FactorForKgs
                    }).ToArray());

                CacheSettings.SetProductMastersCacheId(newCacheId);

                ProductMastersCache.Helper.DeleteMany(ProductMastersCache.Helper.FilterBuilder.Where(x => x.CacheId != newCacheId));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
