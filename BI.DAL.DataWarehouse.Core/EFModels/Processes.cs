using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Processes
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? SetupCost { get; set; }
        public int? SetupCostUnitId { get; set; }
        public decimal? RunCost { get; set; }
        public int? RunCostUnitId { get; set; }
        public int? TimeCapacity { get; set; }
        public decimal? WeightCapacity { get; set; }
        public int? WeightCapacityUnitId { get; set; }
        public string ProcessType { get; set; }
        public bool OverrideStockSpecifications { get; set; }
        public bool CopyMechanicalSpecifications { get; set; }
        public bool CopyConditions { get; set; }
        public bool CopyRejectionDetails { get; set; }
        public bool CopyOtherNumber { get; set; }
        public bool CopySupplierReference { get; set; }
        public bool CopyAdviceNoteReference { get; set; }
        public bool CopyTestCertReference { get; set; }
        public bool CopyIdentifyingMark { get; set; }
        public bool CopyPackingType { get; set; }
        public bool CopyPackingReference { get; set; }
        public bool CopyPiecesPerPack { get; set; }
        public bool CopyPackingWeight { get; set; }
        public bool CopyProductionItem { get; set; }
        public bool CopyNotes { get; set; }
        public bool? NonProductionStep { get; set; }
        public bool? AutoCompleteStepAllowed { get; set; }
        public decimal? StandardCost { get; set; }
        public int? StandardCostUnitId { get; set; }
        public int? DefaultWorkCentreId { get; set; }
        public bool? EstimatedTimesRequired { get; set; }
        public bool? ActualTimesRequired { get; set; }
        public int ProductionWipDefaultingRule { get; set; }
        public decimal? TopEdgeTrim { get; set; }
        public decimal? BottomEdgeTrim { get; set; }
        public decimal? DefaultLeadTime { get; set; }
        public int? DefaultLeadTimeUnitId { get; set; }
        public bool? OutsideProcessing { get; set; }
        public int? OutworkProductId { get; set; }
        public bool? RecordPackedWeight { get; set; }
        public int? EstimatedSetupMinutesPerQuantity { get; set; }
        public int? EstimatedRunMinutesPerQuantity { get; set; }
        public int? EstimatedSetupMinutesPerQuantityUnitId { get; set; }
        public int? EstimatedRunMinutesPerQuantityUnitId { get; set; }
        public string LedgerSegmentCode { get; set; }
        public bool BulkConsumption { get; set; }
        public bool? ApplyKerfDimension1 { get; set; }
        public bool? ApplyKerfDimension2 { get; set; }
        public bool? ApplyKerfDimension3 { get; set; }
        public bool? ApplyKerfDimension4 { get; set; }
        public bool? ApplyKerfDimension5 { get; set; }
        public bool? CopyAllSpecifications { get; set; }
        public bool? RapidEntryAllowed { get; set; }
        public bool? CopyLinkedDocuments { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? ProductionFactor { get; set; }
        public decimal? ScrapPercentage { get; set; }
        public bool? FlatRolledCostOnProduced { get; set; }
        public bool? TransportStep { get; set; }
        public decimal? OperatorCostRate { get; set; }
        public int? OperatorCostUnitId { get; set; }
        public bool? FlatRolledAllocationsFixed { get; set; }
        public bool? DefaultWorkCentreFromSchedule { get; set; }
        public bool? CopyVessel { get; set; }
    }
}
