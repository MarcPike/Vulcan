using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SqlagentJobsStatus
    {
        public Guid? ViewId { get; set; }
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public Guid? StepId { get; set; }
        public int StepNo { get; set; }
        public string StepName { get; set; }
        public string LastRunStatus { get; set; }
        public DateTime? LastRunDateTime { get; set; }
        public DateTime? LastSuccessfulRunDateTime { get; set; }
        public bool? IsCurrent { get; set; }
    }
}
