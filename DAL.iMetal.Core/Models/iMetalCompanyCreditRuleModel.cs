using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.iMetal.Core.Models
{
    public class iMetalCompanyCreditRuleModel
    {
        public string Status { get; set; }
        public decimal? CreditLimit { get; set; }
        public decimal? CreditLimitPercentage { get; set; }
        public int? OverdueDaysAllowed { get; set; }
        public decimal? OverduePercentageAllowed { get; set; }
        public DateTime? CreditLimitDate { get; set; }
        public DateTime? CreditLimitExpiry { get; set; }

        public iMetalCompanyCreditRuleModel()
        {

        }
    }
}
