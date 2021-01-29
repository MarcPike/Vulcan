using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class GoodsInwardsHeaders
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int? BranchId { get; set; }
        public int? Number { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string DeliveryReference { get; set; }
        public int? CarrierId { get; set; }
        public bool? Updated { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ReceiptType { get; set; }
        public int? VehicleRunId { get; set; }
        public int? CustomerId { get; set; }
        public int? PurchaseHeaderId { get; set; }
        public DateTime? FixDate { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public DateTime? PrintedDate { get; set; }
        public int? VesselId { get; set; }
    }
}
