using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class LoggedActions
    {
        public string Coid { get; set; }
        public long EventId { get; set; }
        public string TableName { get; set; }
        public DateTime ActionTstampTx { get; set; }
        public string Action { get; set; }
        public string NewValues { get; set; }
        public string OldValues { get; set; }
        public long? RecordId { get; set; }
        public int? MuserId { get; set; }
        public int? Version { get; set; }
        public string RecordReference { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
