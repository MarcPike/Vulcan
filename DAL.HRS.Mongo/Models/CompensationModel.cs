using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Encryption;
using DAL.Vulcan.Mongo.Base.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using Bonus = DAL.HRS.Mongo.DocClass.Compensation.Bonus;
using BonusScheme = DAL.HRS.Mongo.DocClass.Compensation.BonusScheme;
using CompensationHistory = DAL.HRS.Mongo.DocClass.Compensation.CompensationHistory;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;
using OtherCompensation = DAL.HRS.Mongo.DocClass.Compensation.OtherCompensation;
using PayGradeHistory = DAL.HRS.Mongo.DocClass.Compensation.PayGradeHistory;

namespace DAL.HRS.Mongo.Models
{
    public class CompensationModel : BaseModel, IHavePropertyValues
    {
        public string Id { get; set; }

        public EmployeeRef Employee { get; set; }

        public decimal MonthlyCompensation { get; set; }

        public decimal AnnualSalary { get; set; } 

        public decimal AnnualSalaryWithAllowances { get; set; }

        public DateTime? EffectiveDate { get; set; }


        public decimal CurrentCompensation { get; set; }

        public List<decimal> BaseHourChoices { get; set; } = new List<decimal>();
        public decimal BaseHours { get; set; }

        public decimal YearToDateCompensation { get; set; }

        public decimal QuarterlyCompensation { get; set; }

        public decimal FiscalYearToDateCompensation { get; set; }

        public decimal PremiumRateFactor { get; set; }

        public PropertyValueRef PayGradeType { get; set; }
        public PropertyValueRef PayRateType { get; set; }
        public PropertyValueRef CurrencyType { get; set; }
        public PropertyValueRef IncreaseType { get; set; }
        public PropertyValueRef PayFrequencyType { get; set; }
        public PropertyValueRef AwsEligible { get; set; }
        public decimal PayRateAmount { get; set; }

        public DateTime? KronosPayRuleEffectiveDate { get; set; }
        //public string KronosPayRuleName { get; set; }
        public PropertyValueRef KronosPayRuleType { get; set; }

        public List<BonusModel> BonusHistory { get; set; } = new List<BonusModel>();
        public List<BonusSchemeModel> BonusScheme { get; set; } = new List<BonusSchemeModel>();
        public List<CompensationHistoryModel> CompensationHistory { get; set; } = new List<CompensationHistoryModel>();
        //public List<KronosPayRuleHistoryModel> KronosPayRuleHistory { get; set; } = new List<KronosPayRuleHistoryModel>();
        public List<OtherCompensationModel> OtherCompensation { get; set; } = new List<OtherCompensationModel>();
        public List<PayGradeHistoryModel> PayGradeHistory { get; set; } = new List<PayGradeHistoryModel>();
        public List<PremiumCompensationModel> PremiumCompensation { get; set; } = new List<PremiumCompensationModel>();

        public PropertyValueRef BonusEligible { get; set; } 

        public BonusYtdModel BonusYtd { get; set; }

        public bool IsDirty { get; set; } = false;

        public HrsUserRef ModifiedBy { get; set; }

        public CompensationModel()
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRefs(entity, new List<PropertyValueRef>()
            {
                PayGradeType,
                PayRateType,
                CurrencyType,
                IncreaseType,
                PayFrequencyType,
                AwsEligible,
                KronosPayRuleType,
                BonusEligible
            });
            foreach (var bonusModel in BonusHistory)
            {
                bonusModel.LoadPropertyValuesWithThisEntity(entity);
            }

            foreach (var bonusSchemeModel in BonusScheme)
            {
                bonusSchemeModel.LoadPropertyValuesWithThisEntity(entity);
            }

            foreach (var compensationHistoryModel in CompensationHistory)
            {
                compensationHistoryModel.LoadPropertyValuesWithThisEntity(entity);
            }

            foreach (var otherCompensationModel in OtherCompensation)
            {
                otherCompensationModel.LoadPropertyValuesWithThisEntity(entity);
            }

            foreach (var payGradeHistoryModel in PayGradeHistory)
            {
                payGradeHistoryModel.LoadPropertyValuesWithThisEntity(entity);
            }

            foreach (var premiumCompensationModel in PremiumCompensation)
            {
                premiumCompensationModel.LoadPropertyValuesWithThisEntity(entity);
            }
        }

        public void CalculateCompensationHistoryAnnualAdjustment()
        {
            if (!CompensationHistory.Any()) return;
            for (int i = 0; i < CompensationHistory.Count -1; i++)
            {
                var thisComp = CompensationHistory[i];
                if ((i + 1) > (CompensationHistory.Count -1)) break;
                
                var prevComp = CompensationHistory[i + 1];
                thisComp.AnnualAdjustmentAmount = thisComp.AnnualSalary - prevComp.AnnualSalary;
            }

        }

        public CompensationModel(Employee emp)
        {
            var enc = Encryption.NewEncryption;

            Employee = emp.AsEmployeeRef();
            var c = emp.Compensation;
            if (c == null) return;

            PayRateType = c.PayRateType;
            PremiumRateFactor = enc.Decrypt<decimal>(c.PremiumRateFactor);
            QuarterlyCompensation = enc.Decrypt<decimal>(c.QuarterlyCompensation);
            YearToDateCompensation = enc.Decrypt<decimal>(c.YearToDateCompensation);
            BaseHours = c.BaseHours;
            CurrencyType = c.CurrencyType;
            CurrentCompensation = enc.Decrypt<decimal>(c.CurrentCompensation);
            SetEffectiveDate();
            IncreaseType = c.IncreaseType;
            MonthlyCompensation = enc.Decrypt<decimal>(c.MonthlyCompensation);
            Id = c.Id.ToString();
            PayFrequencyType = c.PayFrequencyType;
            PayGradeType = c.PayGradeType;
            PayRateAmount = enc.Decrypt<decimal>(c.PayRateAmount);
            BonusEligible = c.BonusEligible;
            KronosPayRuleEffectiveDate = c.KronosPayRuleEffectiveDate?.Date;
            KronosPayRuleType = c.KronosPayRuleType?.Refresh();
            AwsEligible = c.AwsEligible;

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


            decimal allowanceTotal = 0;
            if (c.OtherCompensation != null)
            {
                foreach (var otherCompensation in c.OtherCompensation.ToList())
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

                    allowanceTotal += enc.Decrypt<decimal>(otherCompensation.Annualized);
                }
            }


            AnnualSalaryWithAllowances = AnnualSalary + allowanceTotal;
                                         
            //UpdatePropertyReferences.Execute(this);

            FiscalYearToDateCompensation = enc.Decrypt<decimal>(c.FiscalYearToDateCompensation);
            

            SetBonusHistory(c.BonusHistory);
            SetBonusScheme(c.BonusScheme);
            SetCompensationHistory(c.CompensationHistory);
            SetOtherCompensation(c.OtherCompensation);
            SetPayGradeHistory(c.PayGradeHistory);
            SetPremiumCompensation(c.PremiumComp);
            //SetKronosPayRuleHistory(c.KronosPayRuleHistory);

            BonusYtd = new BonusYtdModel(emp.Compensation.BonusHistory);

            var baseHours = new RepositoryBase<BaseHours>().AsQueryable().FirstOrDefault()?? new BaseHours();
            BaseHourChoices = baseHours.Values;

            CalculateCompensationHistoryAnnualAdjustment();

            void SetEffectiveDate()
            {
                EffectiveDate = null;
                var effectiveDate = enc.Decrypt<DateTime>(c.EffectiveDate);
                if (effectiveDate != default(DateTime))
                {
                    EffectiveDate = effectiveDate;
                }
            }
            LoadPropertyValuesWithThisEntity(emp.Entity);
        }

        private void SetPremiumCompensation(List<PremiumComp> premiumComp)
        {
            if (premiumComp == null) return;
            foreach (var p in premiumComp)
            {
                PremiumCompensation.Add(new PremiumCompensationModel(p));
            }
        }

        private void SetPayGradeHistory(List<PayGradeHistory> payGradeHistory)
        {
            if (payGradeHistory == null) return;
            foreach (var p in payGradeHistory)
            {
                PayGradeHistory.Add(new PayGradeHistoryModel(p));
            }

            PayGradeHistory = PayGradeHistory.OrderByDescending(x => x.CreateDateTime).ToList();
        }

        private void SetOtherCompensation(List<OtherCompensation> otherCompensation)
        {
            if (otherCompensation == null) return;
            foreach (var o in otherCompensation)
            {
                OtherCompensation.Add(new OtherCompensationModel(o));
            }

            OtherCompensation = OtherCompensation.OrderByDescending(x => x.EffectiveDate).ToList();
        }

        //private void SetKronosPayRuleHistory(List<DAL.HRS.Mongo.DocClass.Compensation.KronosPayRuleHistory> kronosPayRuleHistory)
        //{
        //    KronosPayRuleHistory.Clear();
        //    if (kronosPayRuleHistory == null) return;
        //    foreach (var h in kronosPayRuleHistory)
        //    {
        //        KronosPayRuleHistory.Add(new KronosPayRuleHistoryModel(h));
        //    }

        //}

        private void SetCompensationHistory(List<CompensationHistory> compensationHistory)
        {
            if (compensationHistory == null) return;
            foreach (var c in compensationHistory)
            {
                CompensationHistory.Add(new CompensationHistoryModel(c));
            }

            CompensationHistory = CompensationHistory.OrderByDescending(x => x.CreatedOn).ToList();

            //foreach (var compensationHistoryModel in CompensationHistory.ToList())
            //{
                
            //}

        }

        private void SetBonusScheme(List<BonusScheme> bonusScheme)
        {
            if (bonusScheme == null) return;
            foreach (var s in bonusScheme)
            {
                BonusScheme.Add(new BonusSchemeModel(s));
            }

            BonusScheme = BonusScheme.OrderByDescending(x => x.EffectiveDate).ToList();
        }

        private void SetBonusHistory(List<Bonus> bonusHistory)
        {
            if (bonusHistory == null) return;
            foreach (var bonus in bonusHistory)
            {
                BonusHistory.Add(new BonusModel(bonus));
            }

            BonusHistory = BonusHistory.OrderByDescending(x => x.DatePaid).ToList();
        }
    }
}
