using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SpecStepheat
    {
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? Cuser { get; set; }
        public int? Muser { get; set; }
        public string State { get; set; }
        public string MpoBranch { get; set; }
        public int? MpoNumber { get; set; }
        public int? MpoItem { get; set; }
        public int? MpoRelease { get; set; }
        public int? MpoCustomer { get; set; }
        public string Groupcode { get; set; }
        public string Sizecode { get; set; }
        public string Gradecode { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Gauge { get; set; }
        public string Partno { get; set; }
        public string HeatSpecCode { get; set; }
        public int? PcsRequired { get; set; }
        public int? PcsUnitsId { get; set; }
        public decimal? WeightRequired { get; set; }
        public int? WeightUnitsId { get; set; }
        public string HtStages { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public int? EquivSection { get; set; }
        public int? NumberOff { get; set; }
        public string Mill { get; set; }
        public string HireworkCastNumber { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string OrderType { get; set; }
        public int? FinalStepNumber { get; set; }
        public string OrderTypePostImport { get; set; }
    }
}
