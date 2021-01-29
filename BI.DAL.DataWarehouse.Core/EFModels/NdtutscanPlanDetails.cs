using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class NdtutscanPlanDetails
    {
        public int Id { get; set; }
        public string Directory { get; set; }
        public string ScanCoverage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
    }
}
