using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProductionSteps
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? BranchId { get; set; }
        public int? Number { get; set; }
        public int? ProductionAllocationId { get; set; }
        public int? ProcessGroupId { get; set; }
        public int? ConsumedProductSpecificationId { get; set; }
        public int? ConsumedPieces { get; set; }
        public decimal? ConsumedQuantity { get; set; }
        public decimal? ConsumedWeight { get; set; }
        public int? RequiredProductSpecificationId { get; set; }
        public string ProcessNotes { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ProcessId { get; set; }
        public int? PreviousStepId { get; set; }
        public int? NextStepId { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? ReplacementStepId { get; set; }
        public int? NumberOfCuts { get; set; }
        public bool? NonProductionStep { get; set; }
        public bool? AutoCompleteStepAllowed { get; set; }
        public int? ProductionFactor { get; set; }
        public decimal? ScrapPercentage { get; set; }
        public int? CommitmentSequenceNumber { get; set; }
        public int? EstimatedPacks { get; set; }
        public decimal? OutsideDiameter { get; set; }
        public decimal? OutsideDiameterMinimum { get; set; }
        public decimal? OutsideDiameterMaximum { get; set; }
        public decimal? PackHeight { get; set; }
        public decimal? PackHeightMinimum { get; set; }
        public decimal? PackHeightMaximum { get; set; }
        public decimal? PackWeight { get; set; }
        public decimal? PackWeightMinimum { get; set; }
        public decimal? PackWeightMaximum { get; set; }
        public decimal? InsideDiameter { get; set; }
        public decimal? InsideDiameterMinimum { get; set; }
        public decimal? InsideDiameterMaximum { get; set; }
        public int? PackCountMinimum { get; set; }
        public int? PackCountMaximum { get; set; }
        public decimal? TotalYieldWeight { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? OutsideProcessing { get; set; }
        public int? OutworkPurchaseItemId { get; set; }
        public decimal? LeadTime { get; set; }
        public int? LeadTimeUnitId { get; set; }
        public int? EstimatedSetupMinutesPerQuantity { get; set; }
        public int? EstimatedRunMinutesPerQuantity { get; set; }
        public int? EstimatedSetupMinutesPerQuantityUnitId { get; set; }
        public int? EstimatedRunMinutesPerQuantityUnitId { get; set; }
        public bool? PlaceHolder { get; set; }
        public int? OutworkSupplierId { get; set; }
        public decimal? OutworkCost { get; set; }
        public int? OutworkCostUnitId { get; set; }
        public decimal? OutworkValue { get; set; }
        public int? OutworkProductId { get; set; }
        public string Reference { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? SequenceNumber { get; set; }
        public int? SalesItemId { get; set; }
        public bool? TransportStep { get; set; }
        public int? ProcessingBranchId { get; set; }
        public int? ProcessingWarehouseId { get; set; }
        public int? FromBranchId { get; set; }
        public int? FromWarehouseId { get; set; }
        public int? ToBranchId { get; set; }
        public int? ToWarehouseId { get; set; }
        public int? TransportPieces { get; set; }
        public decimal? TransportQuantity { get; set; }
        public decimal? TransportWeight { get; set; }
    }
}
