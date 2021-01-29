using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.AuditTrails;
using DAL.HRS.Mongo.DocClass.Benefits;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeAuditTrailModel
    {
        public DateTime UpdatedAt { get; set; }
        public HrsUserRef UpdatedBy { get; set; }
        public ChangeHistory EmployeeDetailsChanges { get; set; } = new ChangeHistory();
        public ChangeHistory BenefitsChanges { get; set; } = new ChangeHistory();
        public ChangeHistory CompensationChanges { get; set; } = new ChangeHistory();
        public ChangeHistory DisciplineChanges { get; set; } = new ChangeHistory();
        public ChangeHistory PerformanceChanges { get; set; } = new ChangeHistory();

        public EmployeeRef Employee { get; set; }

        public bool AnyChanges()
        {
            return EmployeeDetailsChanges.AnyChanges() ||
                   BenefitsChanges.AnyChanges() ||
                   CompensationChanges.AnyChanges() ||
                   DisciplineChanges.AnyChanges() ||
                   PerformanceChanges.AnyChanges();
        }
        public EmployeeAuditTrailModel()
        {

        }

        public EmployeeAuditTrailModel(EmployeeAuditTrail audit)
        {
            UpdatedAt = audit.UpdatedAt;
            UpdatedBy = audit.UpdatedBy;

            Employee = audit.Current.AsEmployeeRef();

            GetEmployeeDetailChanges(new EmployeeModel(audit.Original), new EmployeeModel(audit.Current));
            GetBenefitsChanges(new BenefitsModel(audit.Original), new BenefitsModel(audit.Current));
            GetCompensationChanges(new CompensationModel(audit.Original), new CompensationModel(audit.Current));
            GetDisciplineChanges(audit.Original, audit.Current);
            GetPerformanceChanges(audit.Original, audit.Current);
        }

        private void GetDisciplineChanges(Employee original, Employee current)
        {
            var originalDisc = original.Discipline.
                Select(x => new DisciplineModel(original.AsEmployeeRef(), x)).ToList();
            var currentDisc = current.Discipline.
                Select(x => new DisciplineModel(current.AsEmployeeRef(), x)).ToList();

            var changeHistory = new ChangeHistory { HrsUser = UpdatedBy, ModifiedDate = UpdatedAt };

            foreach (var e in currentDisc)
            {
                if (originalDisc.All(x => x.Id != e.Id))
                {
                    changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Discipline", "Added",
                        e.ToString()));
                }

                if (originalDisc.Any(x =>
                    x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                {
                    changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Discipline", "Modified",
                        e.ToString()));
                }
            }

            foreach (var e in originalDisc)
            {
                if (currentDisc.All(x => x.Id != e.Id))
                {
                    changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Discipline", "Removed",
                        e.ToString()));
                }
            }

            DisciplineChanges = changeHistory;

        }
        private void GetPerformanceChanges(Employee original, Employee current)
        {
            var originalPerf = original.Performance.
                Select(x => new PerformanceModel(original.AsEmployeeRef(), x)).ToList();
            var currentPerf = current.Performance.
                Select(x => new PerformanceModel(current.AsEmployeeRef(), x)).ToList();

            var changeHistory = new ChangeHistory { HrsUser = UpdatedBy, ModifiedDate = UpdatedAt };

            foreach (var e in currentPerf)
            {
                if (originalPerf.All(x => x.Id != e.Id))
                {
                    changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Performance", "Added",
                        e.ToString()));
                }

                if (originalPerf.Any(x =>
                    x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                {
                    changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Performance", "Modified",
                        e.ToString()));
                }
            }

            foreach (var e in originalPerf)
            {
                if (currentPerf.All(x => x.Id != e.Id))
                {
                    changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Performance", "Removed",
                        e.ToString()));
                }
            }

            PerformanceChanges = changeHistory;


        }
        private void GetCompensationChanges(CompensationModel original, CompensationModel current)
        {
            var changeHistory = new ChangeHistory { HrsUser = UpdatedBy, ModifiedDate = UpdatedAt };

            ProcessDecimalFields();
            ProcessDateFields();
            ProcessBooleanFields();
            ProcessBaseHourChoices();
            ProcessPropertyFields();
            ProcessBonusYtd();
            ProcessBonuses();
            ProcessBonusSchemes();
            ProcessCompensationHistory();
            ProcessOtherCompensation();
            ProcessPayGradeHistory();
            ProcessPremiumCompensation();

            void ProcessPremiumCompensation()
            {
                foreach (var e in current.PremiumCompensation)
                {
                    if (original.PremiumCompensation.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PremiumCompensation", "Added",
                            e.ToString()));
                    }

                    if (original.PremiumCompensation.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PremiumCompensation", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.PremiumCompensation)
                {
                    if (current.PremiumCompensation.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PremiumCompensation", "Removed",
                            e.ToString()));
                    }
                }


            }

            void ProcessPayGradeHistory()
            {
                foreach (var e in current.PayGradeHistory)
                {
                    if (original.PayGradeHistory.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PayGradeHistory", "Added",
                            e.ToString()));
                    }

                    if (original.PayGradeHistory.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PayGradeHistory", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.PayGradeHistory)
                {
                    if (current.PayGradeHistory.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PayGradeHistory", "Removed",
                            e.ToString()));
                    }
                }

            }

            void ProcessOtherCompensation()
            {
                foreach (var e in current.OtherCompensation)
                {
                    if (original.OtherCompensation.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("OtherCompensation", "Added",
                            e.ToString()));
                    }

                    if (original.OtherCompensation.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("OtherCompensation", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.OtherCompensation)
                {
                    if (current.OtherCompensation.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("OtherCompensation", "Removed",
                            e.ToString()));
                    }
                }

            }

            void ProcessCompensationHistory()
            {
                foreach (var e in current.CompensationHistory)
                {
                    if (original.CompensationHistory.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("CompensationHistory", "Added",
                            e.ToString()));
                    }

                    if (original.CompensationHistory.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("CompensationHistory", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.CompensationHistory)
                {
                    if (current.CompensationHistory.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("CompensationHistory", "Removed",
                            e.ToString()));
                    }
                }

            }

            void ProcessBonusSchemes()
            {
                foreach (var e in current.BonusScheme)
                {
                    if (original.BonusScheme.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BonusScheme", "Added",
                            e.ToString()));
                    }

                    if (original.BonusScheme.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BonusScheme", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.BonusScheme)
                {
                    if (current.BonusScheme.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BonusScheme", "Removed",
                            e.ToString()));
                    }
                }

            }

            void ProcessBonuses()
            {
                foreach (var e in current.BonusHistory)
                {
                    if (original.BonusHistory.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BonusHistory", "Added",
                            e.ToString()));
                    }

                    if (original.BonusHistory.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BonusHistory", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.BonusHistory)
                {
                    if (current.BonusHistory.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BonusHistory", "Removed",
                            e.ToString()));
                    }
                }
            }

            void ProcessBooleanFields()
            {
            }

            void ProcessDateFields()
            {
                if ((current.EffectiveDate ?? DateTime.Parse("1/1/1900")) !=
                    (original.EffectiveDate ?? DateTime.Parse("1/1/1900")))
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("EffectiveDate",
                            original.EffectiveDate?.ToLongDateString() ?? string.Empty,
                            current.EffectiveDate?.ToLongDateString() ?? string.Empty));

                if ((current.KronosPayRuleEffectiveDate ?? DateTime.Parse("1/1/1900")) !=
                    (original.KronosPayRuleEffectiveDate ?? DateTime.Parse("1/1/1900")))
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("KronosPayRuleEffectiveDate",
                            original.KronosPayRuleEffectiveDate?.ToLongDateString() ?? string.Empty,
                            current.KronosPayRuleEffectiveDate?.ToLongDateString() ?? string.Empty));

            }

            void ProcessBonusYtd()
            {
                if (current.BonusYtd?.CurrentCalendarYear != original.BonusYtd?.CurrentCalendarYear)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("BonusYtd.currentCalendarYear",
                            original.BonusYtd.CurrentCalendarYear.ToString(CultureInfo.InvariantCulture),
                            current.BonusYtd.CurrentCalendarYear.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.BonusYtd?.CurrentFiscalYear != original.BonusYtd?.CurrentFiscalYear)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("BonusYtd.currentFiscalYear",
                            original.BonusYtd.CurrentFiscalYear.ToString(CultureInfo.InvariantCulture),
                            current.BonusYtd.CurrentFiscalYear.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.BonusYtd?.CurrentCalendarYearTotal != original.BonusYtd?.CurrentCalendarYearTotal)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("BonusYtd.currentCalendarYearTotal",
                            original.BonusYtd.CurrentCalendarYearTotal.ToString(CultureInfo.InvariantCulture),
                            current.BonusYtd.CurrentCalendarYearTotal.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.BonusYtd?.CurrentFiscalYearTotal != original.BonusYtd?.CurrentFiscalYearTotal)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("BonusYtd.currentFiscalYearTotal",
                            original.BonusYtd.CurrentFiscalYearTotal.ToString(CultureInfo.InvariantCulture),
                            current.BonusYtd.CurrentFiscalYearTotal.ToString(CultureInfo.InvariantCulture)));
                }

            }

            void ProcessPropertyFields()
            {
                if (current.PayGradeType?.Code != original.PayGradeType?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("PayGradeType", 
                        original.PayGradeType?.Code ?? string.Empty, 
                        current?.PayGradeType?.Code ?? string.Empty));
                if (current.PayRateType?.Code != original.PayRateType?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("PayRateType",
                        original.PayRateType?.Code ?? string.Empty,
                        current?.PayRateType?.Code ?? string.Empty));
                if (current.CurrencyType?.Code != original.CurrencyType?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("CurrencyType",
                        original.CurrencyType?.Code ?? string.Empty,
                        current?.CurrencyType?.Code ?? string.Empty));
                if (current.IncreaseType?.Code != original.IncreaseType?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("IncreaseType",
                        original.IncreaseType?.Code ?? string.Empty,
                        current?.IncreaseType?.Code ?? string.Empty));
                if (current.PayFrequencyType?.Code != original.PayFrequencyType?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("PayFrequencyType",
                        original.PayFrequencyType?.Code ?? string.Empty,
                        current?.PayFrequencyType?.Code ?? string.Empty));
                if (current.AwsEligible?.Code != original.AwsEligible?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("AwsEligible",
                        original.AwsEligible?.Code ?? string.Empty,
                        current?.AwsEligible?.Code ?? string.Empty));
                if (current.KronosPayRuleType?.Code != original.KronosPayRuleType?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("KronosPayRuleType",
                        original.KronosPayRuleType?.Code ?? string.Empty,
                        current?.KronosPayRuleType?.Code ?? string.Empty));
                if (current.BonusEligible?.Code != original.BonusEligible?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("BonusEligible",
                        original.BonusEligible?.Code ?? string.Empty,
                        current?.BonusEligible?.Code ?? string.Empty));

            }

            void ProcessBaseHourChoices()
            {
                foreach (var e in current.BaseHourChoices)
                {
                    if (original.BaseHourChoices.All(x => x != e))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BaseHourChoices", "Added",
                            e.ToString(CultureInfo.InvariantCulture)));
                    }

                }

                foreach (var e in original.BaseHourChoices)
                {
                    if (current.BaseHourChoices.All(x => x != e))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BaseHourChoices", "Removed",
                            e.ToString(CultureInfo.InvariantCulture)));
                    }
                }

            }

            void ProcessDecimalFields()
            {
                if (current.MonthlyCompensation != original.MonthlyCompensation)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("MonthlyCompensation", 
                            original.MonthlyCompensation.ToString(CultureInfo.InvariantCulture), 
                            current.MonthlyCompensation.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.AnnualSalary != original.AnnualSalary)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("AnnualSalary", 
                            original.AnnualSalary.ToString(CultureInfo.InvariantCulture), 
                            current.AnnualSalary.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.AnnualSalaryWithAllowances != original.AnnualSalaryWithAllowances)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("AnnualSalaryWithAllowances", 
                            original.AnnualSalaryWithAllowances.ToString(CultureInfo.InvariantCulture), 
                            current.AnnualSalaryWithAllowances.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.CurrentCompensation != original.CurrentCompensation)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("currentCompensation",
                            original.CurrentCompensation.ToString(CultureInfo.InvariantCulture),
                            current.CurrentCompensation.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.BaseHours != original.BaseHours)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("BaseHours",
                            original.BaseHours.ToString(CultureInfo.InvariantCulture),
                            current.BaseHours.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.YearToDateCompensation != original.YearToDateCompensation)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("YearToDateCompensation",
                            original.YearToDateCompensation.ToString(CultureInfo.InvariantCulture),
                            current.YearToDateCompensation.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.QuarterlyCompensation != original.QuarterlyCompensation)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("QuarterlyCompensation",
                            original.QuarterlyCompensation.ToString(CultureInfo.InvariantCulture),
                            current.QuarterlyCompensation.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.FiscalYearToDateCompensation != original.FiscalYearToDateCompensation)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("FiscalYearToDateCompensation",
                            original.FiscalYearToDateCompensation.ToString(CultureInfo.InvariantCulture),
                            current.FiscalYearToDateCompensation.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.PremiumRateFactor != original.PremiumRateFactor)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("PremiumRateFactor",
                            original.PremiumRateFactor.ToString(CultureInfo.InvariantCulture),
                            current.PremiumRateFactor.ToString(CultureInfo.InvariantCulture)));
                }
                if (current.PayRateAmount != original.PayRateAmount)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("PayRateAmount",
                            original.PayRateAmount.ToString(CultureInfo.InvariantCulture),
                            current.PayRateAmount.ToString(CultureInfo.InvariantCulture)));
                }

            }

            CompensationChanges = changeHistory;
        }
        private void GetBenefitsChanges(BenefitsModel original, BenefitsModel current)
        {
            var changeHistory = new ChangeHistory {HrsUser = UpdatedBy, ModifiedDate = UpdatedAt};

            ProcessPropertyFields();
            ProcessIntFields();
            ProcessDateFields();
            ProcessJobTitle();
            ProcessEmployeeRefFields();
            ProcessBenefitsEnrollment();
            ProcessBenefitsDependents();
            ProcessBenefitsEnrollmentHistory();
            ProcessBenefitDependentHistory();

            BenefitsChanges = changeHistory;

            void ProcessBenefitDependentHistory()
            {
                foreach (var e in current.BenefitDependentHistory)
                {
                    if (original.BenefitDependentHistory.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitDependentHistory", "Added",
                            e.ToString()));
                    }

                    if (original.BenefitDependentHistory.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitDependentHistory", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.BenefitDependentHistory)
                {
                    if (current.BenefitDependentHistory.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitDependentHistory", "Removed",
                            e.ToString()));
                    }
                }

            }

            void ProcessBenefitsEnrollmentHistory()
            {
                foreach (var e in current.BenefitEnrollmentHistory)
                {
                    if (original.BenefitEnrollmentHistory.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitEnrollmentHistory", "Added",
                            e.ToString()));
                    }

                    if (original.BenefitEnrollmentHistory.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitEnrollmentHistory", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.BenefitEnrollmentHistory)
                {
                    if (current.BenefitEnrollmentHistory.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitEnrollmentHistory", "Removed",
                            e.ToString()));
                    }
                }
            }

            void ProcessBenefitsDependents()
            {
                foreach (var e in current.BenefitDependent)
                {
                    if (original.BenefitDependent.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitsDependent", "Added",
                            e.ToString()));
                    }

                    if (original.BenefitDependent.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitsDependent", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.BenefitDependent)
                {
                    if (current.BenefitDependent.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitsDependent", "Removed",
                            e.ToString()));
                    }
                }

            }

            void ProcessBenefitsEnrollment()
            {
                foreach (var e in current.BenefitEnrollment)
                {
                    if (original.BenefitEnrollment.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitsEnrollment", "Added",
                            e.ToString()));
                    }

                    if (original.BenefitEnrollment.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitsEnrollment", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.BenefitEnrollment)
                {
                    if (current.BenefitEnrollment.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BenefitsEnrollment", "Removed",
                            e.ToString()));
                    }
                }


            }

            void ProcessEmployeeRefFields()
            {
                var nullString = string.Empty;

                var currentManagerName = current.Manager?.FullName ?? nullString;
                var originalManagerName = original.Manager?.FullName ?? nullString;
                if (currentManagerName != originalManagerName)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Manager", originalManagerName, currentManagerName));

            }

            void ProcessJobTitle()
            {
                if (current.JobTitle?.Name != original.JobTitle?.Name)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("JobTitle", original.JobTitle?.Name ?? string.Empty, current.JobTitle?.Name ?? string.Empty));
                }
            }

            void ProcessDateFields()
            {
                if (current.HireDate != original.HireDate)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("HireDate",
                            original.HireDate?.ToLongDateString() ?? string.Empty,
                            current.HireDate?.ToLongDateString() ?? string.Empty));

            }

            void ProcessIntFields()
            {
                if (current.Age != original.Age)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Age", original.Age.ToString(), current.Age.ToString()));
                }
            }

            void ProcessPropertyFields()
            {
                if (current.GenderCode?.Code != original.GenderCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("GenderCode", original.GenderCode?.Code ?? string.Empty, current?.GenderCode?.Code ?? string.Empty));
                if (current.CostCenterCode?.Code != original.CostCenterCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("CostCenterCode", original.CostCenterCode?.Code ?? string.Empty, current?.CostCenterCode?.Code ?? string.Empty));
            }

        }
        private void GetEmployeeDetailChanges(EmployeeModel original, EmployeeModel current)
        {

            var changeHistory = new ChangeHistory {HrsUser = UpdatedBy, ModifiedDate = UpdatedAt};

            
            ProcessEmployeeRefFields();
            ProcessDirectReports();
            ProcessLocationFields();
            ProcessPropertyFields();
            ProcessJobTitle();
            ProcessStringFields();
            ProcessDateFields();
            ProcessBooleanFields();
            ProcessIntFields();
            ProcessEducationCertifications();
            ProcessEmployeeVerifications();
            ProcessEmergencyContacts();
            ProcessEmployeePhoneNumbers();
            ProcessEmployeeEmailAddresses();

            EmployeeDetailsChanges = changeHistory;

            void ProcessEmployeeEmailAddresses()
            {
                if ((current.PersonalEmailAddress ?? string.Empty) != (original.PersonalEmailAddress ?? string.Empty))
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("PersonalEmailAddress", 
                            original.PersonalEmailAddress ?? string.Empty, 
                            current.PersonalEmailAddress ?? string.Empty));

                }

                if ((current.WorkEmailAddress ?? string.Empty) != (original.WorkEmailAddress  ?? string.Empty))
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("WorkEmailAddress", 
                            original.WorkEmailAddress ?? string.Empty, 
                            current.WorkEmailAddress ?? string.Empty));

                }

            }

            void ProcessEmployeePhoneNumbers()
            {
                foreach (var e in current.PhoneNumbers)
                {
                    if (original.PhoneNumbers.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PhoneNumbers", "Added",
                            e.ToString()));
                    }

                    if (original.PhoneNumbers.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PhoneNumbers", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.PhoneNumbers)
                {
                    if (current.PhoneNumbers.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PhoneNumbers", "Removed",
                            e.ToString()));
                    }
                }

            }

            void ProcessEmergencyContacts()
            {
                foreach (var e in current.EmergencyContacts)
                {
                    if (original.EmergencyContacts.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EmergencyContact", "Added",
                            e.ToString()));
                    }

                    if (original.EmergencyContacts.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EmergencyContact", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.EmergencyContacts)
                {
                    if (current.EmergencyContacts.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EmergencyContact", "Removed",
                            e.ToString()));
                    }
                }

            }

            void ProcessEmployeeVerifications()
            {
                foreach (var e in current.EmployeeVerifications)
                {
                    if (original.EmployeeVerifications.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EmployeeVerification", "Added",
                            e.ToString()));
                    }

                    if (original.EmployeeVerifications.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EmployeeVerification", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.EmployeeVerifications)
                {
                    if (current.EmployeeVerifications.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EmployeeVerification", "Removed",
                            e.ToString()));
                    }
                }

            }

            void ProcessEducationCertifications()
            {
                foreach (var e in current.EducationCertifications)
                {
                    if (original.EducationCertifications.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EducationCertification", "Added",
                            e.ToString()));
                    }

                    if (original.EducationCertifications.Any(x =>
                        x.Id == e.Id && x.GetHashCode() != e.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EducationCertification", "Modified",
                            e.ToString()));
                    }
                }

                foreach (var e in original.EducationCertifications)
                {
                    if (current.EducationCertifications.All(x => x.Id != e.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EducationCertification", "Removed",
                            e.ToString()));
                    }
                }

            }

            void ProcessIntFields()
            {

            }

            void ProcessBooleanFields()
            {
                if (current.IsActive != original.IsActive)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IsActive", original.IsActive.ToString(), current.IsActive.ToString()));

            }

            void ProcessDateFields()
            {
                if (current.OriginalHireDate != original.OriginalHireDate)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("OriginalHireDate",
                            original.OriginalHireDate?.ToLongDateString() ?? string.Empty,
                            current.OriginalHireDate?.ToLongDateString() ?? string.Empty));
                if (current.Birthday != original.Birthday)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Birthday",
                            original.Birthday.ToLongDateString() ?? string.Empty,
                            current.Birthday.ToLongDateString() ?? string.Empty));
                if (current.LastRehireDate != original.LastRehireDate)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("LastRehireDate",
                            original.LastRehireDate?.ToLongDateString() ?? string.Empty,
                            current.LastRehireDate?.ToLongDateString() ?? string.Empty));
                if (current.PriorServiceDate != original.PriorServiceDate)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("PriorServiceDate",
                            original.PriorServiceDate?.ToLongDateString() ?? string.Empty,
                            current.PriorServiceDate?.ToLongDateString() ?? string.Empty));
                if (current.ConfirmationDate != original.ConfirmationDate)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("ConfirmationDate",
                            original.ConfirmationDate?.ToLongDateString() ?? string.Empty,
                            current.ConfirmationDate?.ToLongDateString() ?? string.Empty));
                if (current.TerminationDate != original.TerminationDate)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("TerminationDate",
                            original.TerminationDate?.ToLongDateString() ?? string.Empty,
                            current.TerminationDate?.ToLongDateString() ?? string.Empty));
                if (current.TimeTrackingAccrualProfileEffectiveDate != original.TimeTrackingAccrualProfileEffectiveDate)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("TimeTrackingAccrualProfileEffectiveDate",
                            original.TimeTrackingAccrualProfileEffectiveDate?.ToLongDateString() ?? string.Empty,
                            current.TimeTrackingAccrualProfileEffectiveDate?.ToLongDateString() ?? string.Empty));
            }

            void ProcessStringFields()
            {
                    if (current.PayrollId != original.PayrollId) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("PayrollId", original.PayrollId, current.PayrollId));
                    if (current.FirstName != original.FirstName) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("FirstName", original.FirstName, current.FirstName));
                    if (current.LastName != original.LastName) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("LastName", original.LastName, current.LastName));
                    if (current.MiddleName != original.MiddleName) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("MiddleName", original.MiddleName, current.MiddleName));
                    if (current.PreferredName != original.PreferredName) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("PreferredName", original.PreferredName, current.PreferredName));
                    if (current.GovernmentId != original.GovernmentId) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("GovernmentId", original.GovernmentId, current.GovernmentId));
                    if (current.AdditionalGovernmentId != original.AdditionalGovernmentId) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("AdditionalGovernmentId", original.AdditionalGovernmentId, current.AdditionalGovernmentId));
                    if (current.Address1 != original.Address1) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Address1", original.Address1, current.Address1));
                    if (current.Address2 != original.Address2) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Address2", original.Address2, current.Address2));
                    if (current.Address3 != original.Address3) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Address3", original.Address3, current.Address3));
                    if (current.City != original.City) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("City", original.City, current.City));
                    if (current.State != original.State) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("State", original.State, current.State));
                    if (current.ContactCountry != original.ContactCountry) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("ContactCountry", original.ContactCountry, current.ContactCountry));
                    if (current.PostalCode != original.PostalCode) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("PostalCode", original.PostalCode, current.PostalCode));
                    if (current.Login != original.Login) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Login", original.Login, current.Login));
                    if (current.ExternalLoginId != original.ExternalLoginId) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("ExternalLoginId", original.ExternalLoginId, current.ExternalLoginId));
                    if (current.TerminationExplanation != original.TerminationExplanation) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("TerminationExplanation", original.TerminationExplanation, current.TerminationExplanation));
                    if (current.ExternalLocationText != original.ExternalLocationText) changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("ExternalLocationText", original.ExternalLocationText, current.ExternalLocationText));
                    //if (current.WorkEmailAddress != original.WorkEmailAddress) changeHistory.ValueChanges.Add(
                    //    new ChangeHistory.ValueChange("WorkEmailAddress", original.WorkEmailAddress, current.WorkEmailAddress));
                    //if (current.PersonalEmailAddress != original.PersonalEmailAddress) changeHistory.ValueChanges.Add(
                    //    new ChangeHistory.ValueChange("PersonalEmailAddress", original.PersonalEmailAddress, current.PersonalEmailAddress));

            }

            void ProcessJobTitle()
            {
                if (current.JobTitle?.Name != original.JobTitle?.Name)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("JobTitle", original.JobTitle?.Name ?? string.Empty, current.JobTitle?.Name ?? string.Empty));
                }
            }

            void ProcessEmployeeRefFields()
            {
                var nullString = string.Empty;

                var currentManagerName = current.Manager?.FullName ?? nullString;
                var originalManagerName = original.Manager?.FullName ?? nullString;
                if (currentManagerName != originalManagerName)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Manager", originalManagerName, currentManagerName));

                var currentKronosManagerName = current.KronosManager?.FullName ?? nullString;
                var originalKronosManagerName = original.KronosManager?.FullName ?? nullString;
                if (currentKronosManagerName != originalKronosManagerName)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("KronosManager", originalKronosManagerName,
                            currentKronosManagerName));
                }

            }

            void ProcessDirectReports()
            {
                foreach (var directReport in current.DirectReports)
                {
                    if (original.DirectReports.All(x => x.Id != directReport.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("DirectReport", "Added",
                            directReport.ToString()));
                    }

                    //if (original.DirectReports.Any(x =>
                    //    x.Id == directReport.Id && x.GetHashCode() != directReport.GetHashCode()))
                    //{
                    //    changeHistory.ListChanges.Add(new ChangeHistory.ListChange("DirectReport", "Modified",
                    //        directReport.ToString()));
                    //}
                }

                foreach (var directReport in original.DirectReports)
                {
                    if (current.DirectReports.All(x => x.Id != directReport.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("DirectReport", "Removed",
                            directReport.ToString()));
                    }
                }
            }

            void ProcessLocationFields()
            {
                if (current.Location?.Office != original.Location?.Office)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Location", original.Location?.Office ?? string.Empty, current.Location?.Office ?? string.Empty));

            }

            void ProcessPropertyFields()
            {
                if (current.TerminationCode?.Code != original.TerminationCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("TerminationCode", original.TerminationCode?.Code ?? string.Empty, current?.TerminationCode?.Code ?? string.Empty));
                if (current.EEOCode?.Code != original.EEOCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("EEOCode", original.EEOCode?.Code ?? string.Empty, current?.EEOCode?.Code ?? string.Empty));
                if (current.RehireStatusCode?.Code != original.RehireStatusCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("RehireStateCode", original.RehireStatusCode?.Code ?? string.Empty, current?.RehireStatusCode?.Code ?? string.Empty));
                if (current.BusinessRegionCode?.Code != original.BusinessRegionCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("BusinessRegionCode", original.BusinessRegionCode?.Code ?? string.Empty, current?.BusinessRegionCode?.Code ?? string.Empty));
                if (current.MaritalStatusCode?.Code != original.MaritalStatusCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("MaritalStatusCode", original.MaritalStatusCode?.Code ?? string.Empty, current?.MaritalStatusCode?.Code ?? string.Empty));
                if (current.GenderCode?.Code != original.GenderCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("GenderCode", original.GenderCode?.Code ?? string.Empty, current?.GenderCode?.Code ?? string.Empty));
                if (current.CostCenterCode?.Code != original.CostCenterCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("CostCenterCode", GetPropertyValueWithDescription(original.CostCenterCode), GetPropertyValueWithDescription(current.CostCenterCode)));
                if (current.ShiftCode?.Code != original.ShiftCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("ShiftCode", original.ShiftCode?.Code ?? string.Empty, current?.ShiftCode?.Code ?? string.Empty));
                if (current.KronosDepartmentCode?.Code != original.KronosDepartmentCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("KronosLaborLevelCode", original.KronosDepartmentCode?.Code ?? string.Empty, current?.KronosDepartmentCode?.Code ?? string.Empty));
                if (current.EthnicityCode?.Code != original.EthnicityCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("EthnicityCode", original.EthnicityCode?.Code ?? string.Empty, current?.EthnicityCode?.Code ?? string.Empty));
                if (current.NationalityCode?.Code != original.NationalityCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("NationalityCode", original.NationalityCode?.Code ?? string.Empty, current?.NationalityCode?.Code ?? string.Empty));
                if (current.CountryOfOriginCode?.Code != original.CountryOfOriginCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("CountryOfOriginCode", original.CountryOfOriginCode?.Code ?? string.Empty, current?.CountryOfOriginCode?.Code ?? string.Empty));
                if (current.Status1Code?.Code != original.Status1Code?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("Status1Code", original.Status1Code?.Code ?? string.Empty, current?.Status1Code?.Code ?? string.Empty));
                if (current.Status2Code?.Code != original.Status2Code?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("Status2Code", original.Status2Code?.Code ?? string.Empty, current?.Status2Code?.Code ?? string.Empty));
                if (current.WorkAreaCode?.Code != original.WorkAreaCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("WorkAreaCode", original.WorkAreaCode?.Code ?? string.Empty, current?.WorkAreaCode?.Code ?? string.Empty));
                if (current.CompanyNumberCode?.Code != original.CompanyNumberCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("CompanyNumberCode", original.CompanyNumberCode?.Code ?? string.Empty, current?.CompanyNumberCode?.Code ?? string.Empty));
                if (current.BusinessUnitCode?.Code != original.BusinessUnitCode?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("BusinessUnitCode", original.BusinessUnitCode?.Code ?? string.Empty, current?.BusinessUnitCode?.Code ?? string.Empty));
                if (current.DeviceGroup?.Code != original.DeviceGroup?.Code) changeHistory.ValueChanges.Add(
                    new ChangeHistory.ValueChange("DeviceGroup", original.DeviceGroup?.Code ?? string.Empty, current?.DeviceGroup?.Code ?? string.Empty));

                string GetPropertyValueWithDescription(PropertyValueRef property)
                {
                    if (property == null) return string.Empty;
                    return $"{property.Code} - {property.Description}";
                }
            }
        }
    }
}
