using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProcessGroups
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
        public string ProcessNotes { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ProcessId { get; set; }
        public int? WorkCentreId { get; set; }
        public int? State { get; set; }
        public int? ProductLevelAllocationId { get; set; }
        public int? ConsumedProductId { get; set; }
        public decimal? ConsumedDim1 { get; set; }
        public decimal? ConsumedDim2 { get; set; }
        public decimal? ConsumedDim3 { get; set; }
        public decimal? ConsumedDim4 { get; set; }
        public decimal? ConsumedDim5 { get; set; }
        public int? CostingTotalsId { get; set; }
        public int? ProductionScheduleItemId { get; set; }
        public decimal? CoilInsideDiameter { get; set; }
        public bool? IncludeConsumables { get; set; }
        public bool? CostOnProduced { get; set; }
        public int? AllocatedCoilPieces { get; set; }
        public bool? Finalised { get; set; }
        public DateTime? PrintedDate { get; set; }
        public bool? Reprinted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? ProductionPlannerId { get; set; }
        public DateTime? StartDate { get; set; }
        public string ProductionState { get; set; }
        public bool? BulkConsumption { get; set; }
        public int? RequestSalesHeaderId { get; set; }
        public bool? NonProduction { get; set; }
        public bool? AutoCompletable { get; set; }
        public bool? OutsideProcess { get; set; }
        public int? ProductionFactor { get; set; }
        public decimal? ScrapPercentage { get; set; }
        public int? FlatRolledOutworkSupplierId { get; set; }
        public decimal? FlatRolledOutworkCost { get; set; }
        public int? FlatRolledOutworkCostUnitId { get; set; }
        public decimal? FlatRolledOutworkValue { get; set; }
        public int? FlatRolledOutworkProductId { get; set; }
        public string ConsumedSpecification1 { get; set; }
        public string ConsumedSpecification2 { get; set; }
        public string ConsumedSpecification3 { get; set; }
        public string ConsumedSpecification4 { get; set; }
        public string ConsumedSpecification5 { get; set; }
        public string ConsumedSpecification6 { get; set; }
        public string ConsumedSpecification7 { get; set; }
        public string ConsumedSpecification8 { get; set; }
        public string ConsumedSpecification9 { get; set; }
        public string ConsumedSpecification10 { get; set; }
        public decimal? ConsumedDim1NegativeTolerance { get; set; }
        public decimal? ConsumedDim1PositiveTolerance { get; set; }
        public decimal? ConsumedDim2NegativeTolerance { get; set; }
        public decimal? ConsumedDim2PositiveTolerance { get; set; }
        public decimal? ConsumedDim3NegativeTolerance { get; set; }
        public decimal? ConsumedDim3PositiveTolerance { get; set; }
        public decimal? ConsumedDim4NegativeTolerance { get; set; }
        public decimal? ConsumedDim4PositiveTolerance { get; set; }
        public decimal? ConsumedDim5NegativeTolerance { get; set; }
        public decimal? ConsumedDim5PositiveTolerance { get; set; }
        public int? StatusId { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? TransportNoteId { get; set; }
        public DateTime? DateStarted { get; set; }
        public string TimeStarted { get; set; }
    }
}
