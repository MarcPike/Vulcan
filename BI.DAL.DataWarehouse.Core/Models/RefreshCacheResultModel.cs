using System;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public class RefreshCacheResultModel
    {
        public int RowCount { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime BuiltAt { get; set; } = new DateTime();
    }
}
