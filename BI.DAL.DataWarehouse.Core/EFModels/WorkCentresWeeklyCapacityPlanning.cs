using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class WorkCentresWeeklyCapacityPlanning
    {
        public string Coid { get; set; }
        public string GroupType { get; set; }
        public int WorkWeekNumber { get; set; }
        public DateTime WeekBeginDate { get; set; }
        public int Sun { get; set; }
        public int Mon { get; set; }
        public int Tue { get; set; }
        public int Wed { get; set; }
        public int Thu { get; set; }
        public int Fri { get; set; }
        public int Sat { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
