using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class CompanyCreditRules
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public decimal? CreditLimit { get; set; }
        public DateTime? CreditLimitDate { get; set; }
        public DateTime? CreditLimitExpiry { get; set; }
        public decimal? CreditLimitPercentage { get; set; }
        public decimal? OverduePercentageAllowed { get; set; }
        public int? OverdueDaysAllowed { get; set; }
        public decimal? InsuredLimit { get; set; }
        public int? InsuranceCompanyId { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? CreditLimitCheckOption { get; set; }
    }
}
