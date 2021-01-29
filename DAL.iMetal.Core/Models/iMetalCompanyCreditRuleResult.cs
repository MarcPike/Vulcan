using System;

namespace Vulcan.IMetal.Queries.Companies
{
    public class iMetalCompanyCreditRuleResult
    {
        public string CompanyName { get; set; }
        public string CreditStatus { get; set; }
        public decimal? CreditLimit { get; set; }
        public decimal? CreditLimitPercentage { get; set; }
        public int? OverdueDaysAllowed { get; set; }
        public decimal? OverduePercentageAllowed { get; set; }
        public DateTime? CreditLimitDate { get; set; }
        public DateTime? CreditLimitExpiry { get; set; }
        public string PaymentTerm { get; set; }
    }
}