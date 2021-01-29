using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class PartNumberSpecificationCosts
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string ItemType { get; set; }
        public int? PartNumberSpecificationId { get; set; }
        public decimal? Cost { get; set; }
        public int? CostUnitId { get; set; }
        public int? SupplierId { get; set; }
        public string BillingReference { get; set; }
        public int? CostGroupCodeId { get; set; }
        public int? PurchaseGroupId { get; set; }
        public string CostFixStatus { get; set; }
        public string Visibility { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
