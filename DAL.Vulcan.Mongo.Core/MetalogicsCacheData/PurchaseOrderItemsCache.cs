using System;
using System.Globalization;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Queries;

namespace DAL.Vulcan.Mongo.Core.MetalogicsCacheData
{
    public class PurchaseOrderItemsCache : BaseDocument
    {
        public Guid CacheId { get; set; }
        public static MongoRawQueryHelper<PurchaseOrderItemsCache> Helper = new MongoRawQueryHelper<PurchaseOrderItemsCache>();
        public string Coid { get; set; }
        public string Status { get; set; }

        public int PurchaseOrderHeaderId { get; set; }
        public int? PoNumber { get; set; }

        public int? ItemNumber { get; set; }

        public int PurchaseOrderItemId { get; set; }
        public DateTime CreateDate { get; set; }
        public string MetalCategory { get; set; }
        public string MetalType { get; set; }
        public string StockType { get; set; }
        public string Location { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string StockGrade { get; set; }
        public string ProductCondition { get; set; }
        public string ProductCategory { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Thick { get; set; }

        public decimal InsideDiameter { get; set; }
        public decimal OuterDiameter { get; set; }
        public decimal Density { get; set; }
        public string ProductControlCode { get; set; }
        public bool ControlPieces { get; set; }
        public decimal VolumeDensity { get; set; }
        public decimal Dim1StaticDimension { get; set; }
        public decimal Dim2StaticDimension { get; set; }
        public decimal Dim3StaticDimension { get; set; }

        public string OrderedPiecesUnit { get; set; }
        public string OrderedLengthUnit { get; set; }
        public string OrderedWeightUnit { get; set; }
        public string OrderedQuantityUnit { get; set; }
        public int OrderedPieces { get; set; } 
        public decimal OrderedQuantity { get; set; } 
        public decimal OrderedLength { get; set; } 
        public decimal OrderedWeight { get; set; } 
        public decimal OrderedWeightLbs { get; set; }
        public decimal OrderedWeightKgs { get; set; }

        public string AllocatedPiecesUnit { get; set; } 
        public string AllocatedLengthUnit { get; set; } 
        public string AllocatedWeightUnit { get; set; } 
        public string AllocatedQuantityUnit { get; set; } 
        public int AllocatedPieces { get; set; } 
        public decimal AllocatedQuantity { get; set; } 
        public decimal AllocatedLength { get; set; } 
        public decimal AllocatedWeight { get; set; } 
        public decimal AllocatedWeightLbs { get; set; }
        public decimal AllocatedWeightKgs { get; set; }

        public string DeliveredPiecesUnit { get; set; }
        public string DeliveredLengthUnit { get; set; }
        public string DeliveredWeightUnit { get; set; }
        public string DeliveredQuantityUnit { get; set; }
        public int DeliveredPieces { get; set; } 
        public decimal DeliveredQuantity { get; set; } 
        public decimal DeliveredLength { get; set; } 
        public decimal DeliveredWeight { get; set; } 
        public decimal DeliveredWeightLbs { get; set; }
        public decimal DeliveredWeightKgs { get; set; }

        public string BalancePiecesUnit { get; set; } 
        public string BalanceLengthUnit { get; set; } 
        public string BalanceWeightUnit { get; set; } 
        public string BalanceQuantityUnit { get; set; } 
        public int BalancePieces { get; set; } 
        public decimal BalanceQuantity { get; set; } 
        public decimal BalanceLength { get; set; } 
        public decimal BalanceWeight { get; set; } 
        public decimal BalanceWeightLbs { get; set; }
        public decimal BalanceWeightKgs { get; set; }

        public decimal MaterialCostTotal { get; set; }
        public decimal ProductionCostTotal { get; set; }
        public decimal TransportCostTotal { get; set; }
        public decimal SurchargeCostTotal { get; set; }
        public decimal MiscellaneousCostTotal { get; set; }
        public decimal TotalCost { get; set; }

        public decimal BaseMaterialValue { get; set; }
        public decimal BaseProductionValue { get; set; }
        public decimal BaseTransportValue { get; set; }
        public decimal BaseSurchargeValue { get; set; }
        public decimal BaseMiscellaneousValue { get; set; }

        public decimal TotalValue { get; set; }

        public decimal CostPerLb { get; set; }

        public decimal CostPerKg { get; set; }

        public decimal ProductDensity { get; set; }

        public decimal CostPerInch { get; set; }

        public string PurchaseType { get; set; }
        public string TransferType { get; set; }
        public string RequestType { get; set; }
        public string PurchaseCategoryCode { get; set; }
        public string PurchaseCategoryDescription { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseShortName { get; set; }
        public string WarehouseName { get; set; }

        public int? InternalStatusId { get; set; }

        public string StatusDescription { get; set; }

        public string StatusCode { get; set; }

        public string Supplier { get; set; }
        public string Buyer { get; set; }

        public string StratificationRank { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ItemDueDate { get; set; }
        public decimal FactorForKgs { get; set; }
        public decimal FactorForLbs { get; set; }

        private string Normalize(decimal value)
        {
            return (value / 1.000000000000000000000000000000000m).ToString(CultureInfo.InvariantCulture);
        }

        public string ProductSize { get; set; }

        public decimal TheoWeight { get; set; }

        public string ProductType { get; set; }

    }
}