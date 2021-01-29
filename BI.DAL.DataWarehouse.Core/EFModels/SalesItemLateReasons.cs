using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesItemLateReasons
    {
        public SalesItemLateReasons()
        {
            SalesItemLateReasonLinks = new HashSet<SalesItemLateReasonLinks>();
            SalesItemSubLateReasons = new HashSet<SalesItemSubLateReasons>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }
        public int? SourceSystemId { get; set; }

        public virtual ICollection<SalesItemLateReasonLinks> SalesItemLateReasonLinks { get; set; }
        public virtual ICollection<SalesItemSubLateReasons> SalesItemSubLateReasons { get; set; }
    }
}
