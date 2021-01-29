using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.Context.Company;

namespace Vulcan.IMetal.Models
{
    public class CompanyCreditRuleModel
    {
        public string  Status { get; set; }
        public decimal? CreditLimit { get; set; }
        public decimal? CreditLimitPercentage { get; set; }
        public int? OverdueDaysAllowed { get; set; }
        public decimal? OverduePercentageAllowed { get; set; }
        public DateTime? CreditLimitDate { get; set; }
        public DateTime? CreditLimitExpiry { get; set; }

        public CompanyCreditRuleModel(CompanyCreditRule creditRule)
        {
            Status = creditRule.Status;
            CreditLimit = creditRule.CreditLimit;
            CreditLimitPercentage = creditRule.CreditLimitPercentage;
            OverdueDaysAllowed = creditRule.OverdueDaysAllowed;
            OverduePercentageAllowed = creditRule.OverduePercentageAllowed;
            CreditLimitDate = creditRule.CreditLimitDate;
            CreditLimitExpiry = creditRule.CreditLimitExpiry;
        }

        public CompanyCreditRuleModel()
        {
            
        }
    }
}
