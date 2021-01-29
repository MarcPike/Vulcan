using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesProductionSummary
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? SalesItemId { get; set; }
        public int? ProductionStepId { get; set; }
        public int? ProcessGroupId { get; set; }
        public bool? Finalised { get; set; }
        public int? GroupStatus { get; set; }
        public int? NumberOfGroups { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
