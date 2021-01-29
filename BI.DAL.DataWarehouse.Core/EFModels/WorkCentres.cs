using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class WorkCentres
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
        public int? EstimatedSetupTime { get; set; }
        public decimal? DefaultCoilInsideDiameter { get; set; }
        public bool IncludeConsumables { get; set; }
        public bool CostOnProduced { get; set; }
        public decimal? StandardCost { get; set; }
        public int? StandardCostUnitId { get; set; }
        public int ProductionWipDefaultingRule { get; set; }
        public decimal? TopEdgeTrim { get; set; }
        public decimal? BottomEdgeTrim { get; set; }
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
        public string TreatmentCode { get; set; }
        public bool? CopyAllSpecifications { get; set; }
        public string StockItemPrefix { get; set; }
        public bool? CopyLinkedDocuments { get; set; }
        public string LedgerSegmentCode { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public decimal? OperatorCostRate { get; set; }
        public int? OperatorCostUnitId { get; set; }
        public bool? CopyVessel { get; set; }
    }
}
