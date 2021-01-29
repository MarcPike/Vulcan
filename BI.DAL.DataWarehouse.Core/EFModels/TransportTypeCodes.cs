using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class TransportTypeCodes
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool? ExternalCost { get; set; }
        public bool? InternalCost { get; set; }
        public bool? ExternalCharge { get; set; }
        public bool? InclusiveCharge { get; set; }
        public string Type { get; set; }
        public bool? PlanTransport { get; set; }
        public int? SalesServiceProductId { get; set; }
        public int? PurchaseGroupId { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
