using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL.iMetal.Core.Queries;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Svc.Vulcan.iMetal.gRPC;

namespace Svc.Vulcan.iMetal.gRpc.Services
{
    public class StockItemsService : StockItems.StockItemsBase
    {
        private readonly ILogger<StockItemsService> _logger;
        public StockItemsService(ILogger<StockItemsService> logger)
        {
            _logger = logger;
        }

        public override async Task<StockItemsReply> GetAvailableStockItemsRequest(StockItemsRequest request, ServerCallContext context)
        {
            var sw = new Stopwatch();
            sw.Start();
            var queryParams = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(request.TagNumber))
            {
                queryParams.Add("TagNumber", request.TagNumber);
            }

            if (!String.IsNullOrEmpty(request.HeatNumber))
            {
                queryParams.Add("HeatNumber", request.HeatNumber);
            }

            if (!String.IsNullOrEmpty(request.ProductCode))
            {
                queryParams.Add("ProductCode", request.ProductCode);
            }

            if (request.ProductId != 0)
            {
                queryParams.Add("ProductId", request.ProductId);
            }

            if (request.StockItemId != 0)
            {
                queryParams.Add("StockItemId", request.StockItemId);
            }

            var stockItems = await StockItemsQuery.ExecuteAsync(request.Coid, queryParams);

            sw.Stop();

            var reply = new StockItemsReply()
            {
                Elapsed = Duration.FromTimeSpan(sw.Elapsed),
                RowCount = stockItems.Count,
            };

            try
            {
                var stockItemResults = new List<StockItemResult>();
                stockItemResults.AddRange(stockItems.Select(x => new StockItemResult()
                {
                    TagNumber = x.TagNumber,
                    AllocatedLength = (double)x.AllocatedLength,
                    AllocatedPieces = x.AllocatedPieces,
                    AllocatedQuantity = (double)x.AllocatedQuantity,
                    AllocatedWeight = (double)x.AllocatedWeight,
                    AllocatedWeightKgs = (double)x.AllocatedWeightKgs,
                    AllocatedWeightLbs = (double)x.AllocatedWeightLbs,
                    AvailableInches = (double)x.AvailableInches,
                    AvailableLength = (double)x.AvailableLength,
                    AvailablePieces = x.AvailablePieces,
                    AvailableQuantity = (double)x.AvailableQuantity,
                    AvailableWeight = (double)x.AvailableWeight,
                    AvailableWeightKgs = (double)x.AvailableWeightKgs,
                    AvailableWeightLbs = (double)x.AvailableWeightLbs,
                    BaseCurrency = x.BaseCurrency,
                    Coid = x.Coid,
                    ControlPieces = x.ControlPieces,
                    CostPerInch = (double)x.CostPerInch,
                    CostPerKg = (double)x.CostPerKg,
                    CostPerLb = (double)x.CostPerLb,
                    CostPerQty = (double)x.CostPerQty,
                    CostPerWeight = (double)x.CostPerWeight,
                    CreateDate = Timestamp.FromDateTime(x.CreateDate ?? DateTime.MinValue),
                    Density = (double)x.Density,
                    Dim1StaticDimension = (double)x.Dim1StaticDimension,
                    Dim2StaticDimension = (double)x.Dim2StaticDimension,
                    Dim3StaticDimenstion = (double)x.Dim3StaticDimension,
                    FactorForKgs = (double)x.FactorForKgs,
                    FactorForLbs = (double)x.FactorForLbs,
                    HeatNumber = x.HeatNumber,
                    InsideDiameter = (double)x.InsideDiameter,
                    OuterDiameter = (double)x.OuterDiameter,
                    IsMachinedPart = x.IsMachinedPart,
                    IsZeroWeightService = x.IsZeroWeightService,
                    Length = (double)x.Length,
                    LengthUnit = x.LengthUnit,
                    Location = x.Location,
                    MaterialCostTotal = (double)x.MaterialCostTotal,
                    MetalCategory = x.MetalCategory,
                    MetalType = x.MetalType,
                    MillCode = x.MillCode,
                    MillName = x.MillName,
                    MiscellaneousCostTotal = (double)x.MiscellaneousCostTotal,
                    Notes = x.Notes,
                    PhysicalLength = (double)x.PhysicalLength,
                    PhysicalPieces = x.PhysicalPieces,
                    PhysicalQuantity = (double)x.PhysicalQuantity,
                    PhysicalWeight = (double)x.PhysicalWeight,
                    PhysicalWeightKgs = (double)x.PhysicalWeightKgs,
                    PhysicalWeightLbs = (double)x.PhysicalWeightLbs,
                    PieceCost = (double)x.PieceCost,
                    PiecesUnit = x.PiecesUnit,
                    ProductCategory = x.ProductCategory,
                    ProductCode = x.ProductCode,
                    ProductCondition = x.ProductCondition,
                    ProductControlCode = x.ProductControlCode,
                    ProductDensity = (double)x.ProductDensity,
                    ProductId = x.ProductId,
                    ProductSize = x.ProductSize,
                    ProductType = x.ProductType,
                    ProductionCostTotal = (double)x.ProductionCostTotal,
                    QuantityUnit = x.QuantityUnit,
                    ReceivedDate = Timestamp.FromDateTime(x.ReceivedDate),
                    StockGrade = x.StockGrade,
                    StockHoldReason = x.StockHoldReason,
                    StockHoldUser = x.StockHoldUser,
                    StockItemId = x.StockItemId,
                    StockType = x.StockType,
                    StratificationRank = x.StratificationRank,
                    SurchargeCostTotal = (double)x.SurchargeCostTotal,
                    TheoWeight = (double)x.TheoWeight,
                    TotalCost = (double)x.TotalCost,
                    TransportCostTotal = (double)x.TransportCostTotal,
                    VolumeDensity = (double)x.VolumeDensity,
                    WarehouseCode = x.WarehouseCode,
                    WarehouseName = x.WarehouseName,
                    WarehouseShortName = x.WarehouseShortName,
                    Width = (double)x.Width
                }).ToList());

                reply.StockItemResults.AddRange(stockItemResults);
                reply.ErrorMessage = string.Empty;

            }
            catch (Exception e)
            {
                reply.ErrorMessage = e.Message;
            }
            
            return reply;
        }
    }
}
