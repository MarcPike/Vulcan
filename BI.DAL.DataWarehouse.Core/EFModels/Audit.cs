using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Audit
    {
        public int AuditId { get; set; }
        public string SourceSystem { get; set; }
        public string SourceTable { get; set; }
        public int? RowsFetched { get; set; }
        public int? RowsInserted { get; set; }
        public int? RowsUpdated { get; set; }
        public int? RowsDeleted { get; set; }
        public bool? ReloadData { get; set; }
        public int? RecordCountSourceTable { get; set; }
        public int? RecordCountDestinationTable { get; set; }
        public decimal? ReloadDataThreshold { get; set; }
        public short LoadHoursOffset { get; set; }
        public DateTime? LoadStartDateTime { get; set; }
        public DateTime? LoadEndDateTime { get; set; }
    }
}
