using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProductionSchedules
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? BranchId { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public int? Shift { get; set; }
        public int? WorkCentreId { get; set; }
        public string Notes { get; set; }
        public int? TimeCapacity { get; set; }
        public decimal? WeightCapacity { get; set; }
        public string ScheduleStatus { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
