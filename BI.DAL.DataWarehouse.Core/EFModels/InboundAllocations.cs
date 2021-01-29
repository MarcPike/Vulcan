using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class InboundAllocations
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? PurchaseItemId { get; set; }
        public int? TransferItemId { get; set; }
        public int? SalesItemId { get; set; }
        public bool? ReserveOnly { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? Pieces { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Quantity { get; set; }
        public string Comments { get; set; }
        public string AllocationType { get; set; }
        public int? ProductLevelAllocationId { get; set; }
        public int? Cuts { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? ProductionItemId { get; set; }
        public int? EnhancedInboundAllocationId { get; set; }
    }
}
