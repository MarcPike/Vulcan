using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class TransientCosts
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int MuserId { get; set; }
        public string Status { get; set; }
        public int? BranchId { get; set; }
        public string TransientType { get; set; }
        public int? TransientId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Dim1 { get; set; }
        public decimal? Dim2 { get; set; }
        public decimal? Dim3 { get; set; }
        public decimal? Dim4 { get; set; }
        public decimal? Dim5 { get; set; }
        public int? PhysicalPieces { get; set; }
        public decimal? PhysicalWeight { get; set; }
        public decimal? PhysicalQuantity { get; set; }
        public decimal? MaterialCost { get; set; }
        public decimal? TransportCost { get; set; }
        public decimal? ProductionCost { get; set; }
        public decimal? MiscellaneousCost { get; set; }
        public decimal? SurchargeCost { get; set; }
        public decimal? MaterialValue { get; set; }
        public decimal? TransportValue { get; set; }
        public decimal? ProductionValue { get; set; }
        public decimal? MiscellaneousValue { get; set; }
        public decimal? SurchargeValue { get; set; }
        public int? StockItemId { get; set; }
        public int? ProductionCostId { get; set; }
        public decimal? CostingWeight { get; set; }
        public decimal? MaterialStockCost { get; set; }
        public decimal? TransportStockCost { get; set; }
        public decimal? ProductionStockCost { get; set; }
        public decimal? MiscellaneousStockCost { get; set; }
        public decimal? SurchargeStockCost { get; set; }
        public int? SalesItemId { get; set; }
        public int? GoodsInwardsItemId { get; set; }
        public int? ProductionStepId { get; set; }
        public decimal? PackedWeight { get; set; }
        public int? ProductionFactor { get; set; }
        public decimal? ScrapPercentage { get; set; }
        public int? EstimatedConsumptionPieces { get; set; }
        public decimal? EstimatedConsumptionQuantity { get; set; }
        public decimal? EstimatedConsumptionWeight { get; set; }
        public int? StockProductionPartSpecificationId { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
    }
}
