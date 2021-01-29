using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SpecExtras
    {
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? Cuser { get; set; }
        public int? Muser { get; set; }
        public string State { get; set; }
        public int? SpecHeaderId { get; set; }
        public string MicroSpecCode { get; set; }
        public string FerriteSpecCode { get; set; }
        public decimal? FerriteMin { get; set; }
        public decimal? FerriteMax { get; set; }
        public string CorrosionSpecCode { get; set; }
        public decimal? MinSoakTemp { get; set; }
        public int? MinSoakUnitsId { get; set; }
        public decimal? MinSoakHours { get; set; }
        public string StandardTextOld { get; set; }
        public string TestcertTextOld { get; set; }
        public string StandardText { get; set; }
        public string TestcertText { get; set; }
        public string TestresultsText { get; set; }
        public string HeattreatmentText { get; set; }
    }
}
