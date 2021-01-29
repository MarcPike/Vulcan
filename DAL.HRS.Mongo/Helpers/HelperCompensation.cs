using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.AuditTrails;
using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Encryption;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperCompensation: HelperBase, IHelperCompensation
    {
        private readonly HelperEmployee _helperEmployee;
        private readonly HelperUser _helperUser;
        //private readonly Encryption _encryption = Encryption.NewEncryption;


        public HelperCompensation()
        {
            _helperEmployee = new HelperEmployee();
            _helperUser = new HelperUser();
        }

        public List<CompensationGridModel> GetCompensationGrid(string userId)
        {


            var employees = _helperEmployee.GetAllMyEmployeeGridModelsForModule(userId, "Compensation", true, withPayrollRegion: true);
            var result = new List<CompensationGridModel>();
            foreach (var employee in employees)
            {
                result.Add(new CompensationGridModel(employee));
            }

            return result.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();

        }

        public CompensationModel GetCompensationForEmployee(string employeeId)
        {
            var employee = _helperEmployee.GetEmployee(employeeId);
            if (employee == null) throw new Exception("Employee not found");
            return new CompensationModel(employee);
        }

        public CompensationModel SaveCompensation(CompensationModel model, bool newRow = false, EmployeeAuditTrail audit=null, HrsUserRef hrsUser = null)
        {
            if (hrsUser != null)
                model.ModifiedBy = hrsUser;


            var employee = _helperEmployee.GetEmployee(model.Employee.Id);
            if (employee == null) throw new Exception("Employee not found");
            if (audit == null)
            {
                audit = new EmployeeAuditTrail(employee, model.ModifiedBy);
            }

            var compExists = true;
            var historyNeeded = false;
            var comp = employee.Compensation;
            if (comp == null)
            {
                comp = new Compensation();
                compExists = false;
            }


            if (compExists)
            {
                historyNeeded = ((_encryption.Decrypt<decimal>(comp.PayRateAmount) != model.PayRateAmount)) ||
                                 (_encryption.Decrypt<DateTime>(comp.EffectiveDate) != model.EffectiveDate) ||
                                     (comp.PayGradeType?.Code != model.PayGradeType?.Code) ||
                                     (comp.IncreaseType?.Code != model.IncreaseType?.Code );
            }


            comp.BonusEligible = model.BonusEligible;

            if (historyNeeded)
            {
                var currentPayRateAmount = _encryption.Decrypt<decimal>(comp.PayRateAmount);

                var payGradeMinimum = (model.PayGradeHistory == null) ? 0 : model.PayGradeHistory.LastOrDefault()?.Minimum ?? 0;
                var payGradeMaximum = (model.PayGradeHistory == null) ? 0 : model.PayGradeHistory.LastOrDefault()?.Maximum ?? 0;

                var annualAdjustment = model.AnnualSalary - _encryption.Decrypt<decimal>(comp.AnnualSalary);

                //// NOTE: Will need to work on Conversion of Historical data when we load Production
                decimal percentOfIncrease = 0;
                
                if (model.PayRateAmount > 0)
                {
                    percentOfIncrease = 100 - ((currentPayRateAmount / model.PayRateAmount) * 100);
                }

               

                comp.CompensationHistory.Add(new CompensationHistory()
                {
                     
                    Id = Guid.NewGuid(),
                    ActualIncreaseAmount = _encryption.Encrypt(model.PayRateAmount - currentPayRateAmount),
                    AnnualSalary = _encryption.Encrypt(model.MonthlyCompensation * 12),
                    CreatedOn = _encryption.Encrypt(DateTime.Now),
                    EffectiveDate = _encryption.Encrypt(model.EffectiveDate),
                    IncreaseType = model.IncreaseType,
                    PayFrequencyType = model.PayFrequencyType,
                    PayGradeMinimum = _encryption.Encrypt(payGradeMinimum),
                    PayGradeMaximum = _encryption.Encrypt(payGradeMaximum),
                    PayGradeType = model.PayGradeType,
                    PayRateAmount = _encryption.Encrypt(model.PayRateAmount),
                    PercentOfIncrease = _encryption.Encrypt(percentOfIncrease),
                    AnnualAdjustmentAmount = _encryption.Encrypt(annualAdjustment)
                });
            }

            comp.BaseHours = model.BaseHours;
            comp.CurrencyType = model.CurrencyType;
            comp.CurrentCompensation = _encryption.Encrypt(model.CurrentCompensation);
            comp.EffectiveDate = _encryption.Encrypt(model.EffectiveDate);
            comp.IncreaseType = model.IncreaseType;
            comp.MonthlyCompensation = _encryption.Encrypt(model.MonthlyCompensation);
            comp.AnnualSalary = _encryption.Encrypt(model.MonthlyCompensation * 12);
            comp.PayFrequencyType = model.PayFrequencyType;
            comp.PayGradeType = model.PayGradeType;
            comp.PayRateAmount = _encryption.Encrypt(model.PayRateAmount);
            comp.PayRateType = model.PayRateType;
            comp.AwsEligible = model.AwsEligible;
            comp.PremiumRateFactor = _encryption.Encrypt(model.PremiumRateFactor);
            comp.QuarterlyCompensation = _encryption.Encrypt(model.QuarterlyCompensation);
            comp.YearToDateCompensation = _encryption.Encrypt(model.YearToDateCompensation);

            comp.KronosPayRuleEffectiveDate = model.KronosPayRuleEffectiveDate;
            comp.KronosPayRuleType = model.KronosPayRuleType?.Refresh();

            employee.Compensation = comp;

            //SetPremiumCompensation(model.PremiumCompensation, employee);
            SetPayGradeHistory(model.PayGradeHistory, employee);
            SetOtherCompensation(model.OtherCompensation, employee);
            //SetCompensationHistory(model.CompensationHistory, employee);
            //SetKronosPayRuleHistory(model.KronosPayRuleHistory, employee);
            SetBonusScheme(model.BonusScheme, employee);
            SetBonusHistory(model.BonusHistory, employee);

           

            employee.SaveToDatabase();
            audit.Save(employee);
            return new CompensationModel(employee);
        }

        public CompensationModel RemoveCompensationHistory(string employeeId, string historyId)
        {
            var emp = Employee.Helper.FindById(employeeId);
            if (emp == null) throw new Exception("Employee not found");

            var id = Guid.Parse(historyId);
            var compHistoryToDelete = emp.Compensation.CompensationHistory.SingleOrDefault(x => x.Id == id);
            if (compHistoryToDelete == null) throw new Exception("History not found for this Id");

            emp.Compensation.CompensationHistory.Remove(compHistoryToDelete);
            Employee.Helper.Upsert(emp);

            return new CompensationModel(emp);

        }

        public CompensationModel RemoveBonusHistory(string employeeId, string historyId)
        {
            var emp = Employee.Helper.FindById(employeeId);
            if (emp == null) throw new Exception("Employee not found");

            var id = Guid.Parse(historyId);
            var compHistoryToDelete = emp.Compensation.BonusHistory.SingleOrDefault(x => x.Id == id);
            if (compHistoryToDelete == null) throw new Exception("History not found for this Id");

            emp.Compensation.BonusHistory.Remove(compHistoryToDelete);
            Employee.Helper.Upsert(emp);

            return new CompensationModel(emp);

        }

        public CompensationModel RemovePayGradeHistory(string employeeId, string historyId)
        {
            var emp = Employee.Helper.FindById(employeeId);
            if (emp == null) throw new Exception("Employee not found");

            var id = Guid.Parse(historyId);
            var compHistoryToDelete = emp.Compensation.PayGradeHistory.SingleOrDefault(x => x.Id == id);
            if (compHistoryToDelete == null) throw new Exception("History not found for this Id");

            emp.Compensation.PayGradeHistory.Remove(compHistoryToDelete);
            Employee.Helper.Upsert(emp);

            return new CompensationModel(emp);

        }



        //private void SetKronosPayRuleHistory(List<KronosPayRuleHistoryModel> kronosPayRuleHistory, Employee emp)
        //{
        //    emp.Compensation.KronosPayRuleHistory.Clear();
        //    if (kronosPayRuleHistory == null) return;
        //    foreach (var p in kronosPayRuleHistory)
        //    {
        //        emp.Compensation.KronosPayRuleHistory.Add(new KronosPayRuleHistory()
        //            {
        //                Id = Guid.Parse(p.Id),
        //                KronosPayRuleEffectiveDate = p.KronosPayRuleEffectiveDate,
        //                KronosPayRuleType = p.KronosPayRuleType
        //            }

        //        );
        //    }
        //}


        private void SetPremiumCompensation(List<PremiumCompensationModel> premiumComp, Employee emp)
        {
            emp.Compensation.PremiumComp.Clear();
            if (premiumComp == null) return;
            foreach (var p in premiumComp)
            {
                emp.Compensation.PremiumComp.Add(new PremiumComp()
                {
                    Branch = p.Branch,
                    Comment = _encryption.Encrypt(p.Comment),
                    DoubleOvertimeRateFactor = p.DoubleOvertimeRateFactor,
                    Id = Guid.Parse(p.Id),
                    OvertimeRateFactor = p.OvertimeRateFactor,
                    PremiumCompensationType = p.PremiumCompensationType,
                    Value = p.Value,
                    ValueType = p.ValueType
                });
            }
        }

        private void SetPayGradeHistory(List<PayGradeHistoryModel> payGradeHistory, Employee emp)
        {
            emp.Compensation.PayGradeHistory.Clear();
            if (payGradeHistory == null) return;
            foreach (var p in payGradeHistory)
            {
                emp.Compensation.PayGradeHistory.Add(new PayGradeHistory()
                    {
                        Id = Guid.Parse(p.Id),
                        CreateDateTime = _encryption.Encrypt(p.CreateDateTime),
                        Minimum = _encryption.Encrypt(p.Minimum),
                        Maximum = _encryption.Encrypt(p.Maximum),
                        PayGradeType = p.PayGradeType
                }
                
                );
               
            }
        }

        private void SetOtherCompensation(List<OtherCompensationModel> otherCompensation, Employee emp)
        {
            emp.Compensation.OtherCompensation.Clear();
            if (otherCompensation == null) return;
            foreach (var o in otherCompensation)
            {
                emp.Compensation.OtherCompensation.Add(new OtherCompensation()
                {
                    Id = Guid.Parse(o.Id),
                    Amount = _encryption.Encrypt(o.Amount),
                    Comment = _encryption.Encrypt(o.Comment),
                    CompensationType = o.CompensationType,
                    EffectiveDate = _encryption.Encrypt(o.EffectiveDate),
                    EndDate = (o.EndDate == null) ? null : _encryption.Encrypt(o.EndDate),
                    Annualized = _encryption.Encrypt(o.Annualized)
                });
            }
        }

        //private void SetCompensationHistory(List<CompensationHistoryModel> compensationHistory, Employee emp)
        //{
        //    emp.Compensation.CompensationHistory.Clear();
        //    if (compensationHistory == null) return;
        //    foreach (var c in compensationHistory)
        //    {
        //        emp.Compensation.CompensationHistory.Add(new CompensationHistory()
        //        {
        //            Id = Guid.Parse(c.Id),
        //            ActualIncreaseAmount = _encryption.Encrypt(c.ActualIncreaseAmount),
        //            AnnualSalary = _encryption.Encrypt(c.AnnualSalary),
        //            CreatedOn = _encryption.Encrypt(c.CreatedOn),
        //            EffectiveDate = _encryption.Encrypt(c.EffectiveDate),
        //            IncreaseType = c.IncreaseType,
        //            PayFrequencyType = c.PayFrequencyType,
        //            PayGradeMinimum = _encryption.Encrypt(c.PayGradeMinimum),
        //            PayGradeMaximum = _encryption.Encrypt(c.PayGradeMaximum),
        //            PayGradeType = c.PayGradeType,
        //            PayRateAmount = _encryption.Encrypt(c.PayRateAmount),
        //            PercentOfIncrease = _encryption.Encrypt(c.PercentOfIncrease)
        //        });
        //    }
        //}

        private void SetBonusScheme(List<BonusSchemeModel> bonusScheme, Employee emp)
        {
            emp.Compensation.BonusScheme.Clear();
            if (bonusScheme == null) return;
            foreach (var s in bonusScheme)
            {
                emp.Compensation.BonusScheme.Add(new BonusScheme()
                {
                    Id = Guid.Parse(s.Id),
                    BonusSchemeType = s.BonusSchemeType,
                    Comment = s.Comment,
                    EffectiveDate = s.EffectiveDate?.Date,
                    EndDate = s.EndDate?.Date,
                    PayFrequencyType = s.PayFrequencyType,
                    TargetPercentage = s.TargetPercentage
                });
            }
        }

        private void SetBonusHistory(List<BonusModel> bonusHistory, Employee emp)
        {
            emp.Compensation.BonusHistory.Clear();
            if (bonusHistory == null) return;
            foreach (var b in bonusHistory)
            {
                emp.Compensation.BonusHistory.Add(new Bonus()
                {
                    Id = Guid.Parse(b.Id),
                    Amount = b.Amount,
                    BonusType = b.BonusType,
                    Comment = b.Comment,
                    DatePaid = b.DatePaid.Date.Date,
                    FiscalYear = b.FiscalYear,
                    PercentPaid = b.PercentPaid
                });
            }
        }

    }
}
