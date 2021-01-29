using System;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class QngCompensationModel
    {
        public string EmployeeId { get; set; }
        public string PayrollId { get; set; }
        public string FirstName { get; set; }
        public string PreferredName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }

        public JobTitleRef JobTitle { get; set; }
        public PropertyValueRef Status1 { get; set; }
        public PropertyValueRef Status2 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public PropertyValueRef CountryCode { get; set; }
        public PropertyValueRef CountryOfOriginCode { get; set; }
        public PropertyValueRef Ethnicity { get; set; }
        public LocationRef Location { get; set; }
        //public PropertyValueRef KronosLaborLevelCode { get; set; }
        public string KronosLaborLevelForLocation { get; set; }
        public PropertyValueRef KronosDepartmentCode { get; set; }
        public PropertyValueRef PayGradeType { get; set; }
        public PropertyValueRef PayRateType { get; set; }
        public PropertyValueRef CurrencyType { get; set; }
        public PropertyValueRef IncreaseType { get; set; }
        public PropertyValueRef PayFrequencyType { get; set; }
        public PropertyValueRef AwsEligible { get; set; }
        public decimal PayRateAmount { get; set; }
        public bool IsActive { get; set; }
        public EmployeeRef Manager { get; set; }

        public PropertyValueRef TerminationCode { get; set; }

        /* Not sure about these */
        public decimal BaseHours { get; set; } = 0;
        public PayrollRegionRef PayrollRegion { get; set; }
        public PropertyValueRef Gender { get; set; }
        public string GovernmentId { get; set; }
        public PropertyValueRef EEOCode { get; set; }
        public PropertyValueRef WorkAreaCode { get; set; }
        public PropertyValueRef KronosPayRuleType { get; set; }
        public decimal AutoAllowance { get; set; } 
        public DateTime? AutoAllowanceEffectiveDate { get; set; }
        public decimal TitleAllowance { get; set; } 
        public DateTime? TitleAllowanceEffectiveDate { get; set; }
        public decimal RelocationAssistance { get; set; } 
        public DateTime? RelocationAssistanceEffectiveDate { get; set; }
        public decimal CostOfLiving { get; set; } 
        public DateTime? CostOfLivingEffectiveDate { get; set; }
        public decimal AccommodationHousingAllowance { get; set; } 
        public DateTime? AccommodationHousingAllowanceEffectiveDate { get; set; }
        public decimal SignOnBonus { get; set; } 
        public DateTime? SignOnBonusEffectiveDate { get; set; }
        public decimal EducationAllowance { get; set; } 
        public DateTime? EducationAllowanceEffectiveDate { get; set; }
        public decimal PaymentRate70p { get; set; } 
        public DateTime? PaymentRate70pEffectiveDate { get; set; }
        public decimal CpfContribution { get; set; } 
        public DateTime? CpfContributionEffectiveDate { get; set; }

        public decimal MonthlyCompensation { get; set; }

        public decimal AnnualSalary { get; set; }
    
        public decimal AnnualSalaryWithAllowances { get; set; }

        public decimal LastIncreasePercent { get; set; }

        public decimal LastIncreaseAmount { get; set; }

        public decimal LastBonusPercentPaid { get; set; }

        public int LastBonusFiscalYear { get; set; }

        public DateTime LastBonusDatePaid { get; set; }

        public int LastBonusCalendarYear { get; set; }

        public PropertyValueRef LastBonusType { get; set; }

        public decimal LastBonusAmount { get; set; }
        public string LastBonusComment { get; set; }

        public float? LastBonusSchemeTargetPercentage { get; set; }

        public PropertyValueRef LastBonusSchemePayFrequencyType { get; set; }

        public DateTime? LastBonusSchemeEndDate { get; set; }

        public DateTime? LastBonusSchemeEffectiveDate { get; set; }

        public string LastBonusSchemeComment { get; set; }

        public PropertyValueRef LastBonusSchemeType { get; set; }

        // Dates
        private DateTime DateOf { get; set; } = DateTime.Now.Date;
        public DateTime? EffectiveDate { get; set; }
        public DateTime? OriginalHireDate { get; set; }
        public DateTime? RehireDate { get; set; }
        public DateTime? PriorServiceDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime? ConfirmationDate { get; set; }

        public int Age { get; set; }

        public TimeSpan Seniority { get; set; }

        public QngCompensationModel()
        {
        }
    }
}
