using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class StockGrades
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool? IgnoreElements { get; set; }
        public string AlloyGroup { get; set; }
        public decimal? CopperPercentage { get; set; }
        public decimal? TinPercentage { get; set; }
        public decimal? ZincPercentage { get; set; }
        public decimal? NickelPercentage { get; set; }
        public decimal? OtherPercentage { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
