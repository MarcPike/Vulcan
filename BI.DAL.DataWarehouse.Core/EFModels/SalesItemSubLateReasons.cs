using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesItemSubLateReasons
    {
        public SalesItemSubLateReasons()
        {
            SalesItemLateReasonLinks = new HashSet<SalesItemLateReasonLinks>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int SalesItemsLateReasonsId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }

        public virtual SalesItemLateReasons SalesItemsLateReasons { get; set; }
        public virtual ICollection<SalesItemLateReasonLinks> SalesItemLateReasonLinks { get; set; }
    }
}
