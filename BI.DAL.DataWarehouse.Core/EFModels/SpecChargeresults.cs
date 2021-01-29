using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SpecChargeresults
    {
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? Cuser { get; set; }
        public int? Muser { get; set; }
        public string State { get; set; }
        public int? ChhdId { get; set; }
        public int? ChtrId { get; set; }
        public string Treatment { get; set; }
        public int? Temperature { get; set; }
        public int? HeatSoak { get; set; }
        public string Coolant { get; set; }
        public int? CoolSoak { get; set; }
        public int? CoolMin { get; set; }
        public int? CoolMax { get; set; }
        public string RecordTempUsing { get; set; }
        public string Furnace { get; set; }
        public DateTime? DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public int? GasIn { get; set; }
        public int? GasOut { get; set; }
        public int? HeatSoakMins { get; set; }
        public int? CoolSoakMins { get; set; }
    }
}
