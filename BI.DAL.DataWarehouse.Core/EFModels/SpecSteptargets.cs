using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SpecSteptargets
    {
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? Cuser { get; set; }
        public int? Muser { get; set; }
        public string State { get; set; }
        public int? SthtId { get; set; }
        public int? MpoStep { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public int? ChhdId { get; set; }
        public string Treatment { get; set; }
        public int? TargetTemp { get; set; }
        public int? HeatSoak { get; set; }
        public string Coolant { get; set; }
        public int? CoolSoak { get; set; }
        public bool? Selected { get; set; }
        public int? Cycle { get; set; }
        public int? ChtrId { get; set; }
        public int? HeatSoakMins { get; set; }
        public int? CoolSoakMins { get; set; }
        public int? WspecTreatmentrank { get; set; }
        public int? HeatSoakMin { get; set; }
        public int? HeatSoakMinMins { get; set; }
        public int? HeatSoakMax { get; set; }
        public int? HeatSoakMaxMins { get; set; }
        public int? TargetRank { get; set; }
    }
}
