using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProductionCostingTotals
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? EstimatedSetupTime { get; set; }
        public int? EstimatedRunTime { get; set; }
        public int? ActualSetupTime { get; set; }
        public int? ActualRunTime { get; set; }
        public decimal? SetupCost { get; set; }
        public int? SetupCostUnitId { get; set; }
        public decimal? SetupValue { get; set; }
        public decimal? RunCost { get; set; }
        public int? RunCostUnitId { get; set; }
        public decimal? RunValue { get; set; }
        public decimal? TransportCost { get; set; }
        public int? TransportCostUnitId { get; set; }
        public decimal? TransportQuantity { get; set; }
        public int? TransportQuantityUnitId { get; set; }
        public decimal? TransportValue { get; set; }
        public decimal? MiscellaneousCost { get; set; }
        public int? MiscellaneousCostUnitId { get; set; }
        public decimal? MiscellaneousQuantity { get; set; }
        public int? MiscellaneousQuantityUnitId { get; set; }
        public decimal? MiscellaneousValue { get; set; }
        public decimal? SurchargeCost { get; set; }
        public int? SurchargeCostUnitId { get; set; }
        public decimal? SurchargeQuantity { get; set; }
        public int? SurchargeQuantityUnitId { get; set; }
        public decimal? SurchargeValue { get; set; }
        public int? ConsumedPieces { get; set; }
        public decimal? ConsumedQuantity { get; set; }
        public decimal? ConsumedWeight { get; set; }
        public decimal? ConsumedValue { get; set; }
        public int? ConsumablesPieces { get; set; }
        public decimal? ConsumablesQuantity { get; set; }
        public decimal? ConsumablesWeight { get; set; }
        public decimal? ConsumablesValue { get; set; }
        public int? ProducedPieces { get; set; }
        public decimal? ProducedQuantity { get; set; }
        public decimal? ProducedWeight { get; set; }
        public decimal? ProducedValue { get; set; }
        public int? RejectedPieces { get; set; }
        public decimal? RejectedQuantity { get; set; }
        public decimal? RejectedWeight { get; set; }
        public decimal? RejectedValue { get; set; }
        public int? ScrapPieces { get; set; }
        public decimal? ScrapQuantity { get; set; }
        public decimal? ScrapWeight { get; set; }
        public decimal? ScrapValue { get; set; }
        public int? LostPieces { get; set; }
        public decimal? LostQuantity { get; set; }
        public decimal? LostWeight { get; set; }
        public decimal? OutworkValue { get; set; }
        public int? UnprocessedPieces { get; set; }
        public decimal? UnprocessedQuantity { get; set; }
        public decimal? UnprocessedWeight { get; set; }
        public decimal? UnprocessedValue { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public decimal? TransportStepValue { get; set; }
    }
}
