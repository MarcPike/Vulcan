using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.AuditTrails;
using DAL.HRS.Mongo.DocClass.Benefits;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperBenefits: HelperBase, IHelperBenefits
    {
        private HelperEmployee _helperEmployee = new HelperEmployee();
        private HelperUser _helperUser = new HelperUser();

        public List<BenefitsGridModel> GetBenefitsGrid(string userId)
        {

            var employees = _helperEmployee.GetAllMyEmployeeGridModelsForModule(userId, "Benefits", true, withBenefits:true);

            var result = new List<BenefitsGridModel>();
            foreach (var employee in employees)
            {
                if (result.All(x => x.EmployeeId != employee.Id.ToString()))
                {
                    result.Add(new BenefitsGridModel(employee));
                }
            }

            return result.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
        }


        public BenefitsModel GetBenefitsModel(string employeeId)
        {
            var employee = _helperEmployee.GetEmployee(employeeId);
            return new BenefitsModel(employee);
        }

        public BenefitsModel SaveBenefitsModel(BenefitsModel model)
        {
            var employee = _helperEmployee.GetEmployee(model.EmployeeId);
            if (employee == null) throw new Exception("Employee not found");

            var audit = new EmployeeAuditTrail(employee, model.ModifiedBy);

            var benefits = employee.Benefits;


                foreach (var b in model.BenefitEnrollment.Where(x=>x.AddedOrModified).ToList())
                {
                    var existing = benefits.BenefitEnrollment.SingleOrDefault(x => x.Id == Guid.Parse(b.Id));
                    if (existing != null)
                    {
                        benefits.BenefitEnrollmentHistory.Add(new BenefitEnrollmentHistory()
                        {
                            BeginDate = existing.BeginDate?.Date,
                            CoverageType = existing.CoverageType,
                            EndDate = b.EndDate?.Date,
                            Id = existing.Id,
                            OptionTypeCode = existing.OptionTypeCode,
                            StatusChangeType = existing.StatusChangeType,
                            BenefitPlan = existing.PlanType,
                            CreatedOn = DateTime.Now,
                            EmployeeCost = existing.EmployeeContribution ?? 0,
                            EmployerCost = existing.EmployerContribution ?? 0,
                        });

                        benefits.BenefitEnrollment.Remove(existing);

                    }
                    else
                    {
                        benefits.BenefitEnrollmentHistory.Add(new BenefitEnrollmentHistory()
                        {
                            BeginDate = b.BeginDate?.Date,
                            CoverageType = b.CoverageType,
                            EndDate = b.EndDate?.Date,
                            Id = Guid.Parse(b.Id),
                            OptionTypeCode = b.OptionTypeCode,
                            StatusChangeType = b.StatusChangeType,
                            BenefitPlan = b.PlanType,
                            CreatedOn = DateTime.Now,
                            EmployeeCost = b.EmployeeContribution ?? 0,
                            EmployerCost = b.EmployerContribution ?? 0
                        });

                    }


                    benefits.BenefitEnrollment.Add(new BenefitEnrollment()
                    {
                        BeginDate = b.BeginDate?.Date,
                        CoverageType = b.CoverageType,
                        EmployeeContribution = b.EmployeeContribution,
                        EmployerContribution = b.EmployerContribution,
                        EndDate = b.EndDate?.Date,
                        Id = Guid.Parse(b.Id),
                        OptionTypeCode = b.OptionTypeCode,
                        PlanType = b.PlanType,
                        StatusChangeType = b.StatusChangeType,
                    });


            }



            foreach (var b in model.BenefitDependent.Where(x=>x.AddedOrModified).ToList())
            {
                var existing = benefits.BenefitDependent.SingleOrDefault(x => x.Id == Guid.Parse(b.Id));
                if (existing != null)
                {
                    benefits.BenefitDependentHistory.Add(new BenefitDependentHistory()
                    {
                        BeginDate = existing.BeginDate?.Date,
                        CreatedOn = DateTime.Now,
                        DateOfBirth = existing.DateOfBirth?.Date,
                        DependentName = existing.DependentName,
                        EndDate = b.EndDate?.Date,
                        GovernmentId = existing.GovernmentId,
                        Id = existing.Id,
                        RelationShip = existing.RelationShip,
                    });
                    benefits.BenefitDependent.Remove(existing);

                }
                else
                {
                    benefits.BenefitDependentHistory.Add(new BenefitDependentHistory()
                    {
                        BeginDate = b.BeginDate?.Date,
                        CreatedOn = DateTime.Now,
                        DateOfBirth = b.DateOfBirth?.Date,
                        DependentName = b.DependentName,
                        EndDate = b.EndDate?.Date,
                        GovernmentId = b.GovernmentId,
                        Id = Guid.Parse(b.Id),
                        RelationShip = b.RelationShip,
                    });

                }

                benefits.BenefitDependent.Add(new BenefitDependent()
                {
                    BeginDate = b.BeginDate?.Date,
                    BeneficiaryPercentageType = b.BeneficiaryPercentageType,
                    DateOfBirth = b.DateOfBirth?.Date,
                    DependentName = b.DependentName,
                    DesignationType = b.DesignationType,
                    EndDate = b.EndDate?.Date,
                    Gender = b.Gender,
                    GovernmentId = b.GovernmentId,
                    Id = Guid.Parse(b.Id),
                    PrimaryCarePhysicianId = b.PrimaryCarePhysicianId,
                    RelationShip = b.RelationShip,
                    Surcharge = b.Surcharge
                });

            }

            foreach (var benefitDependent in benefits.BenefitDependent.ToList())
            {
                if (model.BenefitDependent.All(x => x.Id != benefitDependent.Id.ToString()))
                {
                    benefits.BenefitDependent.Remove(benefitDependent);
                }
            }

            employee.Benefits = benefits;

            employee.SaveToDatabase();
            audit.Save(employee);
            return new BenefitsModel(employee);

        }

        public BenefitEnrollmentModel GetNewEnrollmentModel()
        {
            return new BenefitEnrollmentModel();
        }

        public BenefitEnrollmentHistoryModel GetNewEnrollmentHistoryModel()
        {
            return new BenefitEnrollmentHistoryModel();
        }

        public BenefitDependentModel GetNewDependentModel()
        {
            return new BenefitDependentModel();
        }

        public BenefitDependentHistoryModel GetNewDependentHistoryModel()
        {
            return new BenefitDependentHistoryModel();
        }

        public BenefitsModel RenewBenefitsEnrollment(string employeeId, string enrollmentId, DateTime endDate, DateTime newStartDate,
            DateTime? newEndDate)
        {
            var employee = _helperEmployee.GetEmployee(employeeId);
            if (employee == null) throw new Exception("Employee not found");

            var benefits = employee.Benefits;

            var existing = benefits.BenefitEnrollment.FirstOrDefault(x => x.Id == Guid.Parse(enrollmentId));
            if (existing == null) throw new Exception("Enrollment not found");

            benefits.BenefitEnrollmentHistory.Add(new BenefitEnrollmentHistory()
            {
                BeginDate = existing.BeginDate?.Date,
                CoverageType = existing.CoverageType,
                EndDate = endDate.Date,
                Id = existing.Id,
                OptionTypeCode = existing.OptionTypeCode,
                StatusChangeType = existing.StatusChangeType,
                BenefitPlan = existing.PlanType,
                CreatedOn = DateTime.Now,
                EmployeeCost = existing.EmployeeContribution ?? 0,
                EmployerCost = existing.EmployerContribution ?? 0,
            });
            existing.EndDate = endDate.Date;

            benefits.BenefitEnrollment.Add(new BenefitEnrollment()
            {
                BeginDate = newStartDate.Date,
                CoverageType = existing.CoverageType,
                EmployeeContribution = existing.EmployeeContribution,
                EmployerContribution = existing.EmployerContribution,
                EndDate = newEndDate?.Date,
                Id = Guid.NewGuid(),
                OptionTypeCode = existing.OptionTypeCode,
                PlanType = existing.PlanType,
                StatusChangeType = existing.StatusChangeType,
            });

            employee.Benefits = benefits;
            employee.SaveToDatabase();

            return new BenefitsModel(employee);
        }
    }
}
