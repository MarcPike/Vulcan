using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class WorkCentresForCapacityPlanning
    {
        public string Coid { get; set; }
        public int WorkCentreId { get; set; }
        public string WorkCentreCode { get; set; }
        public string WorkType { get; set; }
        public string ProcessCode { get; set; }
        public int? EstimatedMinutes { get; set; }
        public int? Tonnage { get; set; }
        public string GroupType { get; set; }
        public string BucketDescription { get; set; }
        public string WorkCentreDisplayName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
    }
}
