using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VAudit
    {
        public int AuditId { get; set; }
        public string SourceSystem { get; set; }
        public string SourceTable { get; set; }
        public int? RecordCountSourceTable { get; set; }
        public int? RecordCountDestinationTable { get; set; }
        public int? Delta { get; set; }
        public decimal? DeltaPercent { get; set; }
        public decimal? ReloadDataThreshold { get; set; }
        public short LoadHoursOffset { get; set; }
        public DateTime? LoadStartDateTimeUtc { get; set; }
        public DateTime? LoadEndDateTimeUtc { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? RowsFetched { get; set; }
        public int? RowsInserted { get; set; }
        public int? RowsUpdated { get; set; }
        public int? RowsDeleted { get; set; }
        public bool? ReloadData { get; set; }
    }
}
