using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class IMetalColumnCompare
    {
        public string ServerName { get; set; }
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string ViewColumnName { get; set; }
        public string ViewDataType { get; set; }
        public int? ViewCharacterMaximumLength { get; set; }
        public byte? ViewNumericPrecision { get; set; }
        public short? ViewDatetimePrecision { get; set; }
        public string ViewCollationName { get; set; }
        public string QngTableName { get; set; }
        public string TableColumnName { get; set; }
        public string TableDataType { get; set; }
        public int? TableCharacterMaximumLength { get; set; }
        public byte? TableNumericPrecision { get; set; }
        public short? TableDatetimePrecision { get; set; }
        public string TableCollationName { get; set; }
    }
}
