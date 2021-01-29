using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProcessGroupConsumption
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? ProcessGroupId { get; set; }
        public bool? Consumable { get; set; }
        public int? StockAllocationId { get; set; }
        public int? CoilRuns { get; set; }
        public int? ConsumedStockItemId { get; set; }
        public decimal? ProcessedWeight { get; set; }
        public decimal? ProcessedPercentage { get; set; }
        public int? PlannedStockItemId { get; set; }
        public int? ProducedPlannedItemId { get; set; }
        public int? UnprocessedPieces { get; set; }
        public decimal? UnprocessedQuantity { get; set; }
        public decimal? UnprocessedWeight { get; set; }
        public decimal? RipFinishedLength { get; set; }
        public decimal? RipRtsLength { get; set; }
        public string RipLocation { get; set; }
        public int? RipProducedPieces { get; set; }
        public int? RipRtsType { get; set; }
        public int? UnprocessedCuts { get; set; }
        public string UnprocessedComments { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
