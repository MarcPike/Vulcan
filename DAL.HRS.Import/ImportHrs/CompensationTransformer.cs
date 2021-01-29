using System;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.SqlServer;
using DAL.Vulcan.Mongo.Base.DocClass;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Import.ImportHrs
{

    public static class CompensationTransformer
    {

        public static void TransFormCompensation(Employee employee, CompensationModelHrs compensationHrs)
        {
            employee.Compensation = null;

            if (compensationHrs == null) return;



            var bonusHistory = new List<Bonus>();
            var bonusScheme = new List<BonusScheme>();
            var payRules = new List<KronosPayRuleHistory>();
            var otherComp = new List<OtherCompensation>();
            var payGradeHist = new List<PayGradeHistory>();
            var premiumComps = new List<PremiumComp>();
            var compHistory = new List<CompensationHistory>();

            foreach (var x in compensationHrs.CompensationHistory)
            {
                compHistory.Add(new CompensationHistory()
                {
                    ActualIncreaseAmount = x.ActualIncreaseAmount,
                    AnnualSalary = x.AnnualSalary,
                    CreatedOn = x.CreatedOn,
                    EffectiveDate = x.EffectiveDate,
                    IncreaseType = PropertyBuilder.CreatePropertyValue("IncreaseType", "Type of increase",
                        x.IncreaseType, string.Empty).AsPropertyValueRef(),
                    PayFrequencyType = PropertyBuilder.CreatePropertyValue("PayFrequencyType", "Type of Pay Frequency", 
                        x.PayFrequencyType, string.Empty).AsPropertyValueRef(),
                    PayGradeMaximum = x.PayGradeMaximum,
                    PayGradeMinimum = x.PayGradeMinimum,
                    PayGradeType = PropertyBuilder.CreatePropertyValue("PayGradeType", "Type of Pay Grade",
                        Encryption.NewEncryption.Decrypt<string>(x.PayGradeType), "Pay Grade").AsPropertyValueRef(),
                    PayRateAmount = x.PayRateAmount,
                    PercentOfIncrease = x.PercentOfIncrease
                });
            }

    

            foreach (var x in compensationHrs.BonusHistory)
            {
                bonusHistory.Add(new Bonus()
                {
                    Amount = x.Amount,
                    BonusType =
                        PropertyBuilder.CreatePropertyValue("BonusType", "Type of bonus",
                            x.BonusType, string.Empty).AsPropertyValueRef(),
                    Comment = x.Comment,
                    DatePaid = x.DatePaid,
                    FiscalYear = x.FiscalYear,
                    Id = x.Id,
                    PercentPaid = x.PercentPaid
                });
            }

        
            foreach (var x in compensationHrs.BonusScheme)
            {
                bonusScheme.Add(new BonusScheme()
                {
                    BonusSchemeType = PropertyBuilder.CreatePropertyValue("BonusSchemeType", "Type of Bonus Scheme",
                        x.BonusSchemeType, string.Empty).AsPropertyValueRef(),
                    Comment = x.Comment,
                    EffectiveDate = x.EffectiveDate,
                    EndDate = x.EndDate,
                    Id = x.Id,
                    PayFrequencyType = PropertyBuilder.CreatePropertyValue("PayFrequencyType", "Type of Pay Frequency",
                        x.PayFrequencyType, string.Empty).AsPropertyValueRef(),
                    TargetPercentage = x.TargetPercentage
                });
            }

            foreach (var x in compensationHrs.OtherCompensation)
            {
                otherComp.Add(new OtherCompensation()
                {
                    Amount = x.Amount,
                    Comment = x.Comment,
                    CompensationType = PropertyBuilder.CreatePropertyValue("CompensationType", "Type of Pay Compensation",
                        x.CompensationType, string.Empty).AsPropertyValueRef(),
                    EffectiveDate = x.EffectiveDate,
                    EndDate = x.EndDate,
                    Id = x.Id

                });
            }

            foreach (var x in compensationHrs.KronosPayRuleHistory)
            {
                payRules.Add(new KronosPayRuleHistory()
                {
                    Id = x.Id,
                    KronosPayRuleEffectiveDate = x.KronosPayRuleEffectiveDate,
                    KronosPayRuleType = PropertyBuilder.CreatePropertyValue("KronosRuleType", "Kronos Rule Type", x.KronosPayRuleName ?? "(empty)", "").AsPropertyValueRef()
                });
            }


            foreach (var x in compensationHrs.PayGradeHistory)
            {
                payGradeHist.Add(new PayGradeHistory()
                {
                    CreateDateTime = x.CreateDateTime,
                    Id = x.Id,
                    Maximum = x.Maximum,
                    Minimum = x.Minimum,
                    PayGradeType = PropertyBuilder.CreatePropertyValue("PayGradeType", "Type of Pay Grade",
                        x.PayGradeType, string.Empty).AsPropertyValueRef(),
                });
            }
            foreach (var x in compensationHrs.PremiumComp)
            {
                premiumComps.Add(new PremiumComp()
                {
                    Branch = x.Branch,
                    Comment = x.Comment,
                    DoubleOvertimeRateFactor = x.DoubleOvertimeRateFactor,
                    Id = x.Id,
                    OvertimeRateFactor = x.OvertimeRateFactor,
                    PremiumCompensationType = PropertyBuilder.CreatePropertyValue("PremiumCompensationType", "Type of Premium Compensation",
                        x.PremiumCompensationType, string.Empty).AsPropertyValueRef(),
                    Value = x.Value,
                    ValueType = PropertyBuilder.CreatePropertyValue("PremiumCompensationValueType", "Type of Premium Compensation Value",
                        x.ValueType, string.Empty).AsPropertyValueRef(),
                });
            }
            

            employee.Compensation = new Compensation()
            {
                BaseHours = compensationHrs.BaseHours,
                PayRateAmount = compensationHrs.PayRateAmount,
                PremiumRateFactor = compensationHrs.PremiumRateFactor,
                QuarterlyCompensation = compensationHrs.QuarterlyCompensation,
                YearToDateCompensation = compensationHrs.YearToDateCompensation,
                BonusHistory = bonusHistory,
                CompensationHistory = compHistory,
                CurrencyType = PropertyBuilder.CreatePropertyValue("CurrencyType", "Type of currency",
                    compensationHrs.CurrencyType, string.Empty).AsPropertyValueRef(),
                BonusScheme = bonusScheme,

                KronosPayRuleEffectiveDate = compensationHrs.KronosPayRuleEffectiveDate,
                KronosPayRuleType = PropertyBuilder.New("KronosPayRuleType","KronosPayRuleType", compensationHrs.KronosPayRuleType,""),
                
                CurrentCompensation = compensationHrs.CurrentCompensation,
                EffectiveDate = compensationHrs.EffectiveDate,
                IncreaseType = PropertyBuilder.CreatePropertyValue("IncreaseType", "Type of increase",
                    compensationHrs.IncreaseType, string.Empty).AsPropertyValueRef(),
                MonthlyCompensation = compensationHrs.MonthlyCompensation,
                OldHrsId = compensationHrs.OldHrsId,
                OtherCompensation = otherComp,
                PayFrequencyType = PropertyBuilder.CreatePropertyValue("PayFrequencyType", "Type of Pay Frequency",
                    compensationHrs.PayFrequencyType, string.Empty).AsPropertyValueRef(),
                PayGradeHistory = payGradeHist,
                PayGradeType = PropertyBuilder.CreatePropertyValue("PayGradeType", "Type of Pay Grade",
                    compensationHrs.PayGradeType, string.Empty).AsPropertyValueRef(),
                PayRateType = PropertyBuilder.CreatePropertyValue("PayRateType", "Type of Pay Rate",
                    compensationHrs.PayRateType, string.Empty).AsPropertyValueRef(),
                PremiumComp = premiumComps,
            };

        }

    }

}