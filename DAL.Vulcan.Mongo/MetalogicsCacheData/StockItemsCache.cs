using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Vulcan.Mongo.MetalogicsCacheData
{
    public class StockItemsCache : BaseDocument
    {
        public static MongoRawQueryHelper<StockItemsCache> Helper = new MongoRawQueryHelper<StockItemsCache>();

        public Guid CacheId { get; set; }
        public string Coid { get; set; }
        public int ProductId { get; set; }
        public int StockItemId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string TagNumber { get; set; }
        public string HeatNumber { get; set; }
        public string MetalCategory { get; set; }
        public string MetalType { get; set; }
        public string StockType { get; set; }
        public string Notes { get; set; }
        public string Location { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseShortName { get; set; }
        public string ProductCode { get; set; }
        public string StockGrade { get; set; }
        public string ProductCondition { get; set; }
        public string ProductCategory { get; set; }

        public string BaseCurrency { get; set; }

        public string ProductSize { get; set; }

        public string MillCode { get; set; }
        public string MillName { get; set; }
        public string ProductControlCode { get; set; }
        public bool ControlPieces { get; set; }
        public decimal VolumeDensity { get; set; }
        public decimal Dim1StaticDimension { get; set; }
        public decimal Dim2StaticDimension { get; set; }
        public decimal Dim3StaticDimension { get; set; }


        public decimal Width { get; set; }

        public decimal Length { get; set; }

        public decimal InsideDiameter { get; set; }

        public decimal OuterDiameter { get; set; }

        public decimal Density { get; set; }

        public string StockHoldReason { get; set; }
        public string StockHoldUser { get; set; }

        public int PhysicalPieces { get; set; }
        public int AllocatedPieces { get; set; }
        public int AvailablePieces { get; set; }
        public string PiecesUnit { get; set; }


        public decimal PhysicalQuantity { get; set; }
        public decimal AllocatedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public string QuantityUnit { get; set; }


        public decimal PhysicalLength { get; set; }
        public decimal AllocatedLength { get; set; }
        public decimal AvailableLength { get; set; }
        public string LengthUnit { get; set; }


        public decimal PhysicalWeight { get; set; }
        public decimal PhysicalWeightLbs { get; set; }
        public decimal PhysicalWeightKgs { get; set; }


        public decimal AllocatedWeight { get; set; }
        public decimal AllocatedWeightLbs { get; set; }
        public decimal AllocatedWeightKgs { get; set; }


        public decimal AvailableWeight { get; set; }
        public decimal AvailableWeightLbs { get; set; }
        public decimal AvailableWeightKgs { get; set; }
        public string WeightUnit { get; set; }

        public decimal MaterialCostTotal { get; set; }
        public decimal ProductionCostTotal { get; set; }
        public decimal TransportCostTotal { get; set; }
        public decimal SurchargeCostTotal { get; set; }
        public decimal MiscellaneousCostTotal { get; set; }
        public decimal TotalCost { get; set; }

        public decimal AvailableInches { get; set; }

        public decimal CostPerInch { get; set; }

        public decimal CostPerWeight { get; set; }
        public decimal CostPerQty { get; set; }
        public string StratificationRank { get; set; }

        public decimal CostPerLb { get; set; }

        public decimal CostPerKg { get; set; }
        public decimal ProductDensity { get; set; }


        public DateTime ReceivedDate { get; set; }

        public bool IsMachinedPart { get; set; }

        public bool IsZeroWeightService { get; set; }

        public decimal PieceCost { get; set; }

        public decimal TheoWeight { get; set; }

        public string ProductType { get; set; }
        public decimal FactorForLbs { get; set; }
        public decimal FactorForKilos { get; set; }

        public StockItemsCache()
        {
            
        }

    }
}
