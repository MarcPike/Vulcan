using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SpecChargehead
    {
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? Cuser { get; set; }
        public int? Muser { get; set; }
        public string State { get; set; }
        public string Branch { get; set; }
        public int? ChargeNumber { get; set; }
        public string HtStages { get; set; }
        public DateTime? LatestStartFrom { get; set; }
        public DateTime? LatestStartTo { get; set; }
        public bool? Directmpo { get; set; }
    }
}
