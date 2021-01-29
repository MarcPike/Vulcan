using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesItemLateReasonLinks
    {
        public int Id { get; set; }
        public string Coid { get; set; }
        public int SalesBranchId { get; set; }
        public int SalesItemId { get; set; }
        public int? LateReasonId { get; set; }
        public int? SubLateReasonId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }

        public virtual SalesItemLateReasons LateReason { get; set; }
        public virtual SalesItemSubLateReasons SubLateReason { get; set; }
    }
}
