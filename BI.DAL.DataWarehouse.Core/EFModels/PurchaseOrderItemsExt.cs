using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class PurchaseOrderItemsExt
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? PurchaseHeaderId { get; set; }
        public string BranchCode { get; set; }
        public int? Ponumber { get; set; }
        public int? PoitemNumber { get; set; }
        public string PoreferenceNumber { get; set; }
        public int? PoitemInternalStatusId { get; set; }
        public string PoitemInternalStatusCode { get; set; }
        public string PoitemInternalStatusDescription { get; set; }
        public string PoitemStatusCode { get; set; }
        public string PoitemStatusDescription { get; set; }
        public DateTime? PoitemDuedate { get; set; }
        public string PoitemCreatedBy { get; set; }
        public string PoitemModifiedBy { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
