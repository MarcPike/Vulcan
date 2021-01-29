using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class StockAllocations
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? ItemId { get; set; }
        public int? ProductId { get; set; }
        public int? StockItemId { get; set; }
        public int? WarehouseId { get; set; }
        public decimal? Dim1 { get; set; }
        public decimal? Dim2 { get; set; }
        public decimal? Dim3 { get; set; }
        public decimal? Dim4 { get; set; }
        public decimal? Dim5 { get; set; }
        public bool? ReserveOnly { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? Pieces { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? InvoiceWeight { get; set; }
        public string Comments { get; set; }
        public int? ParentId { get; set; }
        public decimal? PackingWeight { get; set; }
        public string ItemType { get; set; }
        public int? SalesItemId { get; set; }
        public int? DespatchItemId { get; set; }
        public int? JobConsumptionId { get; set; }
        public int? InboundAllocationId { get; set; }
        public int? VehicleStopDetailId { get; set; }
        public int? ProductLevelAllocationId { get; set; }
        public bool? Firm { get; set; }
        public DateTime? PrintedDate { get; set; }
        public int? InboundAllocatedPieces { get; set; }
        public decimal? InboundAllocatedQuantity { get; set; }
        public decimal? InboundAllocatedWeight { get; set; }
        public int? Cuts { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public string AutomatedProcessType { get; set; }
    }
}
