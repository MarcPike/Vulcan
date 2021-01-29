using System.Collections.Generic;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.StockItems;
using Vulcan.IMetal.Helpers;

namespace Vulcan.IMetal.Models
{
    //public class InventoryModel : Vulcan.IMetal.Models.BaseModel<InventoryModelHelper>
    //{
    //    public int Id { get; set; }
    //    public string Coid { get; set; }
    //    public string ProductCategory { get; set; }
    //    public string FilterProductCode { get; set; }
    //    public string FilterProductGrade { get; set; }
    //    public string FilterProductSize { get; set; }
    //    public string ProductSizeDescription { get; set; }
    //    public string FilterStockType { get; set; }
    //    public string FilterMetalCategory { get; set; }
    //    public string FilterMetalType { get; set; }
    //    public string FilterMaterialType { get; set; }
    //    public string FilterMaterialTypeDescription { get; set; }
    //    public string FilterProductCondition { get; set; }
    //    public List<PartNumberSpecification> CustomerPartSpecification { get; set; }
    //    public decimal FilterOutsideDiameter { get; set; }
    //    public decimal FilterInsideDiameter { get; set; }
    //    public decimal TheoWeight { get; set; }
    //    public string Mill { get; set; }
    //    public string CountryOfMaterialOrigin { get; set; }
    //    public string TagNumber { get; set; }
    //    public string HeatNumber { get; set; }
    //    public string PartNumber { get; set; }
    //    public string Warehouse { get; set; }
    //    public string WarehouseName { get; set; }
    //    public string StockLocation { get; set; }
    //    public string StockRemarks { get; set; }
    //    public int Pieces { get; set; }
    //    public decimal FilterLength { get; set; }
    //    public decimal Weight { get; set; }
    //    public decimal Quantity { get; set; }
    //    public decimal Gauge { get; set; } = 0;
    //    public decimal FilterDensity { get; set; } = 0;
    //    public decimal MaterialCost { get; set; } = 0;
    //    public decimal MiscellaneousCost { get; set; } = 0;
    //    public decimal ProductionCost { get; set; } = 0;
    //    public decimal SurchargeCost { get; set; } = 0;
    //    public decimal TransportCost { get; set; } = 0;
    //    public decimal TotalCost => MaterialCost + MiscellaneousCost + ProductionCost + SurchargeCost + TransportCost;
    //    public decimal UnitCost => (Quantity == 0) ? 0 : TotalCost/Quantity;

    //    public static InventoryModel Convert(string coid, StockItem stockItem, StockItemsDataContext context)
    //    {

    //        return new InventoryModel()
    //        {
    //            Coid = coid,
    //            Id = stockItem.Id,
    //            CountryOfMaterialOrigin = "US",
    //            HeatNumber = Helper.GetHeatNumber(stockItem),
    //            TagNumber = Helper.GetTagNumber(stockItem),
    //            Pieces = stockItem.PhysicalPiece ?? 0,
    //            FilterLength = stockItem.PhysicalQuantity ?? 0,
    //            Weight = stockItem.PhysicalWeight ?? 0,
    //            Quantity = stockItem.PhysicalQuantity ?? 0,
    //            Gauge = Helper.GetGauge(stockItem),
    //            FilterDensity = Helper.GetDensity(stockItem),
    //            FilterInsideDiameter = Helper.GetInsideDiameter(stockItem),
    //            FilterOutsideDiameter = Helper.GetOutsideDiameter(stockItem),
    //            FilterMaterialType = Helper.GetMaterialType(stockItem),
    //            FilterMaterialTypeDescription = Helper.GetMaterialTypeDescription(stockItem),
    //            Mill = Helper.GetMill(stockItem),
    //            ProductCategory = Helper.GetProductCategory(stockItem),
    //            FilterProductCode = Helper.GetProductCode(stockItem),
    //            FilterProductCondition = Helper.GetProductCondition(stockItem),
    //            FilterProductGrade = Helper.GetProductGrade(stockItem),
    //            ProductSizeDescription = Helper.GetProductSizeDescription(stockItem),
    //            PartNumber = Helper.GetPartNumber(stockItem, context),
    //            FilterProductSize = Helper.GetProductSize(stockItem, context),
    //            Warehouse = Helper.GetWarehouse(stockItem),
    //            WarehouseName = Helper.GetWarehouseName(stockItem),
    //            StockLocation = Helper.GetStockLocation(stockItem),
    //            StockRemarks = Helper.GetStockRemarks(stockItem),
    //            FilterStockType = Helper.GetStockType(stockItem),
    //            MaterialCost = stockItem.MaterialCost ?? 0,
    //            MiscellaneousCost = stockItem.MiscellaneousCost ?? 0,
    //            ProductionCost = stockItem.ProductionCost ?? 0,
    //            SurchargeCost = stockItem.SurchargeCost ?? 0,
    //            TransportCost = stockItem.TransportCost ?? 0,
    //            TheoWeight = Helper.GetTheoWeight(stockItem, context, coid),
    //            FilterMetalCategory = Helper.GetMetalType(stockItem),
    //            FilterMetalType = Helper.GetMetalType2(stockItem),
    //        };

    //    }
    //}
}
