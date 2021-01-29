using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class HalliburtonBillingImportBatches
    {
        public int Id { get; set; }
        public string Spreadsheet { get; set; }
        public string Salesperson { get; set; }
        public string JobNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool BatchFailed { get; set; }
    }
}
