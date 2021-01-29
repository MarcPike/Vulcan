using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class IMetalAuditLogSpec
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnBasedOn { get; set; }
        public string SourceTableForId { get; set; }
        public string ColumnFromSourceTableToJoin { get; set; }
    }
}
