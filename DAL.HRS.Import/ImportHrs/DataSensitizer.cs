using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Encryption;

namespace DAL.HRS.Import.ImportHrs
{
    public static class DataSensitizer
    {
        public static void RunForAll()
        {

        }

        public static Employee Execute(Employee employee)
        {
            Random gen = new Random();
            var enc = Encryption.NewEncryption;
            employee.GovernmentId = enc.Encrypt(SensitizeGovernmentId(employee));
            employee.Address1 = $"{employee.OldHrsId} Somewhere street";

            employee.Birthday = enc.Encrypt(RandomBirthDay());

            var comp = employee.Compensation;

            if (comp != null)
            {
                comp.CurrentCompensation = enc.Encrypt(10);
                comp.MonthlyCompensation = enc.Encrypt(0);
                comp.PayFrequencyType = PropertyBuilder.CreatePropertyValue("PayFrequencyType",
                    "Type of Pay Frequency",
                    "BiWeekly", string.Empty).AsPropertyValueRef();
                comp.BaseHours = 40;
                comp.PayRateAmount = enc.Encrypt(10);
                comp.PayRateType = PropertyBuilder.CreatePropertyValue("PayRateType",
                    "Type of Pay Rate",
                    "Hourly", string.Empty).AsPropertyValueRef();

                foreach (var otherCompensation in comp.OtherCompensation)
                {
                    otherCompensation.Amount = enc.Encrypt(100);
                    otherCompensation.CompensationType = PropertyBuilder.CreatePropertyValue("CompensationType",
                        "Type of Compensation",
                        "Sensatized", "Sensitized for QA or Development").AsPropertyValueRef();
                    otherCompensation.Comment = enc.Encrypt("Sensatized");
                }

                foreach (var premiumComp in comp.PremiumComp)
                {
                    premiumComp.Comment = enc.Encrypt("Sensatized");
                    premiumComp.DoubleOvertimeRateFactor = 10;
                    premiumComp.OvertimeRateFactor = 1;
                    premiumComp.PremiumCompensationType = PropertyBuilder.CreatePropertyValue("PremiumCompensationType","Type of PremiumCompensation",
                            "Sensatized", "Sensitized for QA or Development").AsPropertyValueRef();
                    premiumComp.Value = 100;
                    premiumComp.ValueType = PropertyBuilder.CreatePropertyValue("PremiumCompensationValueType",
                        "Type of Premium Compensation Value",
                        "Sensatized", "Sensitized for QA or Development").AsPropertyValueRef();
                }

                comp.PremiumRateFactor = enc.Encrypt(1);
                comp.QuarterlyCompensation = enc.Encrypt(10);
                comp.YearToDateCompensation = enc.Encrypt(100);
                comp.FiscalYearToDateCompensation = enc.Encrypt(110);

                foreach (var bonus in comp.BonusHistory)
                {
                    bonus.Amount = 10;
                    bonus.BonusType = PropertyBuilder.CreatePropertyValue("BonusType", "Type of Bonus", "Sensitized",
                        "Sensitized for QA or Development").AsPropertyValueRef();
                    bonus.FiscalYear = 2015;
                    bonus.PercentPaid = 1;
                    bonus.Comment = "testing bonus comment";
                }

                foreach (var bonusScheme in comp.BonusScheme)
                {
                    bonusScheme.BonusSchemeType = PropertyBuilder.CreatePropertyValue("BonusSchemeType",
                        "Type of Bonus Scheme",
                        "Discretionary", string.Empty).AsPropertyValueRef();
                }

                foreach (var compHist in comp.CompensationHistory)
                {
                    compHist.ActualIncreaseAmount = enc.Encrypt(0);
                    compHist.AnnualSalary = enc.Encrypt(100);
                    compHist.PayRateAmount = enc.Encrypt(10);
                    compHist.PercentOfIncrease = enc.Encrypt(0);
                    compHist.PayGradeType = PropertyBuilder.CreatePropertyValue("PayGradeType","Type of Pay Grade",
                        "Sensitized", "Sensitized for testing").AsPropertyValueRef();
                    compHist.IncreaseType = PropertyBuilder.CreatePropertyValue("IncreaseType", "Type of Increase",
                        "Sensitized", "Sensitized for testing").AsPropertyValueRef();
                    compHist.PayFrequencyType = PropertyBuilder.CreatePropertyValue("PayFrequencyType", "Type of Pay Frequency",
                        "Sensitized", "Sensitized for testing").AsPropertyValueRef();
                }


                var disciplineList = employee.Discipline;

                foreach (var discipline in disciplineList)
                {
                    discipline.DisciplinaryActionType = PropertyBuilder.CreatePropertyValue("DisciplinaryActionType",
                        "Disciplinary Action Type",
                        "Sensitized", "Test Only").AsPropertyValueRef();
                    discipline.EmployeeStatement = "Testing blah blah blah";
                    discipline.ManagerStatement = "Testing blah blah blah";
                    discipline.NatureOfViolationType = PropertyBuilder.CreatePropertyValue("NatureOfViolationType",
                        "Nature of Violation Type",
                        "Sensitized", "Test Only").AsPropertyValueRef();
                    discipline.DateOfViolation = new DateTime(1980,1,1);
                    //discipline.DateOfAction = new DateTime(1980, 1, 1);
                    discipline.DateOfActionAppeals = new DateTime(1980, 1, 1);

                }
            }

            foreach (var benefitDependent in employee.Benefits.BenefitDependent)
            {
                benefitDependent.GovernmentId = "123-45-6789";
            }

            //employee.SaveToDatabase();
            return employee;

            DateTime RandomBirthDay()
            {
                DateTime start = new DateTime(1950, 1, 1);
                int range = (DateTime.Today - start).Days;
                return start.AddDays(gen.Next(range));
            }

        }

        private static string SensitizeGovernmentId(Employee employee)
        {

            if (employee.PayrollId == null)
            {
                return "999-88-7777";
            }
            var numbersOnly = Regex.Replace(employee.PayrollId, "[^0-9.]", "");
            try
            {
                var payRollId = Convert.ToInt32(numbersOnly);
                var newSsnSuffix = 1110 + payRollId.ToString();
                newSsnSuffix = newSsnSuffix.Substring(newSsnSuffix.Length - 4, 4);
                return "111-11-" + newSsnSuffix;
            }
            catch (Exception)
            {
                return "???-??-????";
            }

        }
    }

}
    