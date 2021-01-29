using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesChargeDiscounts
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? SalesChargeId { get; set; }
        public string Description { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? ExchangeRate { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
