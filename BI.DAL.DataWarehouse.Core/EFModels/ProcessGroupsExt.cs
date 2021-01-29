using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProcessGroupsExt
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public string BranchCode { get; set; }
        public int? Number { get; set; }
        public string ProcessGroupReference { get; set; }
        public int? State { get; set; }
        public string ProcessGroupStateDescription { get; set; }
        public string ProcessGroupStatusCode { get; set; }
        public string ProcessGroupStatusDescription { get; set; }
        public DateTime? ProductionScheduleDate { get; set; }
        public int? ProductionScheduleShift { get; set; }
        public int? WorkCentreId { get; set; }
        public string WorkCentreCode { get; set; }
        public string WorkCentreDescription { get; set; }
        public string RecoveryType { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
