using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.Models
{
    public class QngCompensationModel
    {
        private Encryption _encryption { get; set; } = Encryption.NewEncryption;
        public string EmployeeId { get; set; }
        public string PayrollId { get; set; }
        public string FirstName { get; set; }
        public string PreferredName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public PropertyValueRef Status1 { get; set; }
        public PropertyValueRef Status2 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public PropertyValueRef CountryCode { get; set; }
        public PropertyValueRef CountryOfOriginCode { get; set; }
        public PropertyValueRef Ethnicity { get; set; }

        public PropertyValueRef CostCenterCode { get; set; }
        public LocationRef Location { get; set; }

        public JobTitleRef JobTitle { get; set; }
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
        public decimal AutoAllowance { get; set; } = 0;
        public DateTime? AutoAllowanceEffectiveDate { get; set; }
        public decimal TitleAllowance { get; set; } = 0;
        public DateTime? TitleAllowanceEffectiveDate { get; set; }
        public decimal RelocationAssistance { get; set; } = 0;
        public DateTime? RelocationAssistanceEffectiveDate { get; set; }
        public decimal CostOfLiving { get; set; } = 0;
        public DateTime? CostOfLivingEffectiveDate { get; set; }
        public decimal AccommodationHousingAllowance { get; set; } = 0;
        public DateTime? AccommodationHousingAllowanceEffectiveDate { get; set; }
        public decimal SignOnBonus { get; set; } = 0;
        public DateTime? SignOnBonusEffectiveDate { get; set; }
        public decimal EducationAllowance { get; set; } = 0;
        public DateTime? EducationAllowanceEffectiveDate { get; set; }
        public decimal PaymentRate70p { get; set; } = 0;
        public DateTime? PaymentRate70pEffectiveDate { get; set; }
        public decimal CpfContribution { get; set; } = 0;
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

        private List<BonusSchemeModel> BonusSchemes { get; set; } = new List<BonusSchemeModel>();
        private List<CompensationHistoryModel> CompensationHistory { get; set; } = new List<CompensationHistoryModel>();
        private List<BonusModel> BonusHistory { get; set; } = new List<BonusModel>();




        // Dates

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        private DateTime DateOf { get; set; } = DateTime.Now.Date;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] 
        public DateTime? EffectiveDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? OriginalHireDate { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? RehireDate { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? PriorServiceDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? TerminationDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime BirthDay { get; set; }
        public DateTime? ConfirmationDate { get; set; }

        public int Age
        {
            get
            {
                try
                {
                    DateTime now = DateOf.Date;
                    int age = now.Year - BirthDay.Year;
                    if (BirthDay > now.AddYears(-age)) age--;
                    return age;
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }

        public TimeSpan Seniority
        {
            get
            {
                var startDate = (RehireDate ?? PriorServiceDate ?? OriginalHireDate ?? DateOf);
                var endDate = (TerminationDate ?? DateOf);

                var result = endDate - startDate;
                return result;

            }
        }

        public QngCompensationModel() { }
        public QngCompensationModel(ObjectId id, string payrollId, string firstName, string preferredName, string lastName,
            string address1, string address2, string address3, string city, string stateProvince, string postalCode,
            PropertyValueRef countryCode, PropertyValueRef countryOfOriginCode, PropertyValueRef ethnicity, LocationRef location,
            PropertyValueRef kronosLaborLevel, PropertyValueRef kronosDepartmentCode,
            PropertyValueRef status1, PropertyValueRef status2,  bool isActive, EmployeeRef manager, 
            PropertyValueRef terminationCode, PayrollRegionRef payrollRegion, PropertyValueRef gender, 
            byte[] governmentId, PropertyValueRef eeoCode, PropertyValueRef workAreaCode, 
            DateTime? originalHireDate, DateTime? rehireDate, DateTime? priorServiceDate, DateTime? terminationDate, 
            byte[] birthDay, Compensation compensation, JobTitleRef jobTitle, DateTime? dateOf, PropertyValueRef costCenterCode, DateTime? confirmationDate)
        {
            EmployeeId = id.ToString();
            PayrollId = payrollId;
            FirstName = firstName;
            PreferredName = preferredName;
            LastName = lastName;
            FullName = (preferredName != string.Empty) ? $"{preferredName} {lastName}" : $"{firstName} {lastName}";
            Address1 = address1;
            Address2 = address2;
            Address3 = address3;
            City = city;
            StateProvince = stateProvince;
            PostalCode = postalCode;
            CountryCode = countryCode;
            CountryOfOriginCode = countryOfOriginCode;
            Ethnicity = ethnicity;
            Location = location;
            //KronosLaborLevelCode = kronosLaborLevel;
            KronosDepartmentCode = kronosDepartmentCode;
            Status1 = status1;
            Status2 = status2;
            IsActive = isActive;
            Manager = manager;
            TerminationCode = terminationCode;
            PayrollRegion = payrollRegion;
            Gender = gender;
            GovernmentId = _encryption.Decrypt<string>(governmentId);
            EEOCode = eeoCode;
            WorkAreaCode = workAreaCode;

            OriginalHireDate = originalHireDate?.ToUniversalTime();
            RehireDate = rehireDate?.ToUniversalTime();
            PriorServiceDate = priorServiceDate?.ToUniversalTime();
            TerminationDate = terminationDate?.ToUniversalTime();
            BirthDay = (_encryption.Decrypt<DateTime>(birthDay)).ToUniversalTime();
            ConfirmationDate = confirmationDate?.ToUniversalTime();

            DateOf = dateOf ?? DateTime.Now.Date;
            JobTitle = jobTitle;
            CostCenterCode = costCenterCode;

            if (location != null)
            {
                KronosLaborLevelForLocation = location.AsLocation().KronosLaborLevel;
            }

            SetCompensationFields(compensation);


        }



        private void SetCompensationFields(Compensation compensation)
        {
            if (compensation == null) return;
            MonthlyCompensation = _encryption.Decrypt<decimal>(compensation.MonthlyCompensation);


            PayGradeType = compensation.PayGradeType;
            PayRateType = compensation.PayRateType;
            CurrencyType = compensation.CurrencyType;
            IncreaseType = compensation.IncreaseType;
            
            PayFrequencyType = compensation.PayFrequencyType;
            AwsEligible = compensation.AwsEligible;
            PayRateAmount = _encryption.Decrypt<decimal>(compensation.PayRateAmount);
            KronosPayRuleType = compensation.KronosPayRuleType;
            BaseHours = compensation.BaseHours;

            EffectiveDate = null;
            var effectiveDate = _encryption.Decrypt<DateTime>(compensation.EffectiveDate);
            if (effectiveDate != default(DateTime))
            {
                EffectiveDate = effectiveDate.ToUniversalTime();
            }


            decimal allowanceTotal = 0;
            if (compensation.OtherCompensation != null)
            {
                foreach (var otherCompensation in compensation.OtherCompensation.ToList())
                {
                    var compensationType = otherCompensation.CompensationType?.Code;
                    if ((compensationType == null) || (!compensationType.Contains("Allowance"))) continue;

                    if (otherCompensation.EndDate != null)
                    {
                        //var endDate = enc.Decrypt<DateTime>(otherCompensation.EndDate);
                        //var thisYear = DateTime.Now.Year;
                        //if (endDate.Year != thisYear)
                        continue;
                    }

                    allowanceTotal += _encryption.Decrypt<decimal>(otherCompensation.Annualized);
                }
            }

            AnnualSalary = 0;


            if (PayRateType != null)
            {
                if (PayRateType.Code.ToUpper().Contains("HOURLY"))
                {
                    AnnualSalary = PayRateAmount * BaseHours * 52;
                }
                else if (PayRateType.Code.ToUpper().Contains("BIWEEKLY"))
                {
                    AnnualSalary = PayRateAmount * 26;
                }
                else if (PayRateType.Code.ToUpper().Contains("SEMIMONTHLY"))
                {
                    AnnualSalary = PayRateAmount * 24;
                }
                else if (PayRateType.Code.ToUpper().Contains("MONTHLY"))
                {
                    AnnualSalary = PayRateAmount * 12;
                }
            }


            AnnualSalaryWithAllowances = AnnualSalary + allowanceTotal;


            if (compensation.OtherCompensation.Any())
            {
                var autoAllowance = compensation.OtherCompensation.Where(x =>
                    x.CompensationType.Code == "Auto / Transport  Allowance" && x.EndDate == null).ToList();
                if (autoAllowance.Any())
                {
                    AutoAllowance = autoAllowance.Where(x=>x.EndDate == null).Sum(a => _encryption.Decrypt<decimal>(a.Amount));
                    AutoAllowanceEffectiveDate = autoAllowance.Max(x => _encryption.Decrypt<DateTime>(x.EffectiveDate));
                }


                var titleAllowance = compensation.OtherCompensation.Where(x =>
                    x.CompensationType.Code == "Title Allowance" && x.EndDate == null).ToList();

                if (titleAllowance.Any())
                {
                    TitleAllowance = titleAllowance.Where(x => x.EndDate == null).Sum(a => _encryption.Decrypt<decimal>(a.Amount));
                    TitleAllowanceEffectiveDate = titleAllowance.Max(x => _encryption.Decrypt<DateTime>(x.EffectiveDate));
                }


                var relocationAssistance = compensation.OtherCompensation.Where(x =>
                    x.CompensationType.Code == "Relocation Assistance" && x.EndDate == null).ToList();
                if (relocationAssistance.Any())
                {
                    RelocationAssistance = relocationAssistance.Where(x => x.EndDate == null).Sum(a => _encryption.Decrypt<decimal>(a.Amount));
                    RelocationAssistanceEffectiveDate =
                        relocationAssistance.Max(x => _encryption.Decrypt<DateTime>(x.EffectiveDate));
                }


                var costOfLiving = compensation.OtherCompensation.Where(x =>
                    x.CompensationType.Code == "Cost of Living" && x.EndDate == null).ToList();
                if (costOfLiving.Any())
                {
                    CostOfLiving = costOfLiving.Where(x => x.EndDate == null).Sum(a => _encryption.Decrypt<decimal>(a.Amount));
                    CostOfLivingEffectiveDate = costOfLiving.Max(x => _encryption.Decrypt<DateTime>(x.EffectiveDate));
                }


                var accommodationHousingAllowance = compensation.OtherCompensation.Where(x =>
                    x.CompensationType.Code == "Accommodation/Housing Allowance" && x.EndDate == null).ToList();
                if (accommodationHousingAllowance.Any())
                {
                    AccommodationHousingAllowance =
                        accommodationHousingAllowance.Where(x => x.EndDate == null).Sum(a => _encryption.Decrypt<decimal>(a.Amount));
                    AccommodationHousingAllowanceEffectiveDate =
                        accommodationHousingAllowance.Max(x => _encryption.Decrypt<DateTime>(x.EffectiveDate));
                }


                var signOnBonus = compensation.OtherCompensation.Where(x =>
                    x.CompensationType.Code == "Sign-On Bonus" && x.EndDate == null).ToList();
                if (signOnBonus.Any())
                {
                    SignOnBonus = signOnBonus.Where(x => x.EndDate == null).Sum(a => _encryption.Decrypt<decimal>(a.Amount));
                    SignOnBonusEffectiveDate = signOnBonus.Max(x => _encryption.Decrypt<DateTime>(x.EffectiveDate));
                }


                var educationAllowance = compensation.OtherCompensation.Where(x =>
                    x.CompensationType.Code == "Education/School Allowance Minor Children" && x.EndDate == null).ToList();
                if (educationAllowance.Any())
                {
                    EducationAllowance = educationAllowance.Where(x => x.EndDate == null).Sum(a => _encryption.Decrypt<decimal>(a.Amount));
                    EducationAllowanceEffectiveDate =
                        educationAllowance.Max(x => _encryption.Decrypt<DateTime>(x.EffectiveDate));
                }


                var paymentRate70p = compensation.OtherCompensation.Where(x =>
                    x.CompensationType.Code == "70p Payment Rate" && x.EndDate == null).ToList();
                if (paymentRate70p.Any())
                {
                    PaymentRate70p = paymentRate70p.Where(x => x.EndDate == null).Sum(a => _encryption.Decrypt<decimal>(a.Amount));
                    PaymentRate70pEffectiveDate =
                        paymentRate70p.Max(x => _encryption.Decrypt<DateTime>(x.EffectiveDate));
                }


                var cpfContribution = compensation.OtherCompensation.Where(x =>
                    x.CompensationType.Code == "CPF Contribution" && x.EndDate == null).ToList();
                if (cpfContribution.Any())
                {
                    CpfContribution = cpfContribution.Where(x => x.EndDate == null).Sum(a => _encryption.Decrypt<decimal>(a.Amount));
                    CpfContributionEffectiveDate =
                        cpfContribution.Max(x => _encryption.Decrypt<DateTime>(x.EffectiveDate));
                }

                AutoAllowanceEffectiveDate = AutoAllowanceEffectiveDate?.ToUniversalTime();
                TitleAllowanceEffectiveDate = TitleAllowanceEffectiveDate?.ToUniversalTime();
                RelocationAssistanceEffectiveDate = RelocationAssistanceEffectiveDate?.ToUniversalTime();
                CostOfLivingEffectiveDate = CostOfLivingEffectiveDate?.ToUniversalTime();
                AccommodationHousingAllowanceEffectiveDate =
                    AccommodationHousingAllowanceEffectiveDate?.ToUniversalTime();
                SignOnBonusEffectiveDate = SignOnBonusEffectiveDate?.ToUniversalTime();
                EducationAllowanceEffectiveDate = EducationAllowanceEffectiveDate?.ToUniversalTime();
                PaymentRate70pEffectiveDate = PaymentRate70pEffectiveDate?.ToUniversalTime();
                CpfContributionEffectiveDate = CpfContributionEffectiveDate?.ToUniversalTime();
            }

            SetCompensationHistory(compensation.CompensationHistory);
            SetBonusHistory(compensation.BonusHistory);
            SetBonusScheme(compensation.BonusScheme);

            if (CompensationHistory.Any())
            {
                var mostRecentCompensation = CompensationHistory.FirstOrDefault();
                LastIncreaseAmount = mostRecentCompensation.ActualIncreaseAmount;
                LastIncreasePercent = mostRecentCompensation.PercentOfIncrease;

            }

            SetBonusFields(compensation.BonusHistory);
            SetBonusSchemeFields();
        }

        private void SetBonusSchemeFields()
        {
            if (!BonusSchemes.Any()) return;

            var lastBonusScheme = BonusSchemes.First();

            LastBonusSchemeType = lastBonusScheme.BonusSchemeType;
            LastBonusSchemeComment = lastBonusScheme.Comment;
            LastBonusSchemeEffectiveDate = lastBonusScheme.EffectiveDate?.ToUniversalTime();
            LastBonusSchemeEndDate = lastBonusScheme.EndDate?.ToUniversalTime();
            LastBonusSchemePayFrequencyType = lastBonusScheme.PayFrequencyType;
            LastBonusSchemeTargetPercentage = lastBonusScheme.TargetPercentage;
        }

        private void SetBonusHistory(List<Bonus> bonusHistory)
        {
            if (bonusHistory == null) return;
            foreach (var bonus in bonusHistory)
            {
                var model = new BonusModel(bonus);
                model.DatePaid = model.DatePaid.ToUniversalTime();
                BonusHistory.Add(model);
            }

            BonusHistory = BonusHistory.OrderByDescending(x => x.DatePaid).ToList();


        }

        private void SetBonusFields(List<Bonus> bonusHistory)
        {
            if ((bonusHistory == null) || (!bonusHistory.Any())) return;

            var lastBonus = bonusHistory.First();
            LastBonusAmount = lastBonus.Amount;
            LastBonusType = lastBonus.BonusType;
            LastBonusCalendarYear = lastBonus.CalendarYear;
            LastBonusDatePaid = lastBonus.DatePaid.ToUniversalTime();
            LastBonusFiscalYear = lastBonus.FiscalYear;
            LastBonusPercentPaid = lastBonus.PercentPaid;
            LastBonusComment = lastBonus.Comment;

        }

        private void SetBonusScheme(List<BonusScheme> bonusScheme)
        {
            if (bonusScheme == null) return;
            foreach (var s in bonusScheme)
            {
                var model = new BonusSchemeModel(s);
                model.EffectiveDate = model.EffectiveDate?.ToUniversalTime();
                model.EndDate = model.EndDate?.ToUniversalTime();
                BonusSchemes.Add(model);
            }

            BonusSchemes = BonusSchemes.OrderByDescending(x => x.EffectiveDate).ToList();
        }


        private void SetCompensationHistory(List<CompensationHistory> compensationHistory)
        {
            if (compensationHistory == null) return;
            foreach (var c in compensationHistory)
            {
                var model = new CompensationHistoryModel(c);
                model.EffectiveDate = model.EffectiveDate.ToUniversalTime();
                model.CreatedOn = model.CreatedOn.ToUniversalTime();
                CompensationHistory.Add(model);
            }

            CompensationHistory = CompensationHistory.OrderByDescending(x => x.CreatedOn).ToList();


        }
    }
}
