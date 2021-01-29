using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.DocClass.Benefits
{
    public class BenefitsModel: BaseModel, IHavePropertyValues
    {
        public string EmployeeId { get; set; }
        public PropertyValueRef GenderCode { get; set; }
        public int Age { get; set; }
        public DateTime? HireDate { get; set; }
        public JobTitleRef JobTitle { get; set; }
        public EmployeeRef Manager { get; set; }
        public PropertyValueRef CostCenterCode { get; set; }


        public List<BenefitEnrollmentModel> BenefitEnrollment { get; set; } = new List<BenefitEnrollmentModel>();
        public List<BenefitDependentModel> BenefitDependent { get; set; } = new List<BenefitDependentModel>();
        public List<BenefitEnrollmentHistoryModel> BenefitEnrollmentHistory { get; set; } = new List<BenefitEnrollmentHistoryModel>();
        public List<BenefitDependentHistoryModel> BenefitDependentHistory { get; set; } = new List<BenefitDependentHistoryModel>();
        public HrsUserRef ModifiedBy { get; set; }
        public BenefitsModel()
        {
        }

        public BenefitsModel(Employee.Employee e)
        {
            var entity = e.Entity;
            EmployeeId = e.Id.ToString();
            GenderCode = e.GenderCode;
            Age = e.GetAge();
            HireDate = e.OriginalHireDate;
            JobTitle = e.JobTitle;
            Manager = e.Manager;
            CostCenterCode = e.CostCenterCode;

            BenefitEnrollment = e.Benefits.BenefitEnrollment.Where(x=>x.EndDate == null || x.EndDate >= DateTime.Now).Select(x => new BenefitEnrollmentModel(x)).OrderBy(x=>x.PlanType?.Code ?? "").ToList();
            BenefitDependent = e.Benefits.BenefitDependent.Where(x => x.EndDate == null || x.EndDate >= DateTime.Now).Select(x => new BenefitDependentModel(x)).OrderBy(x=>x.DependentName).ToList();
            BenefitEnrollmentHistory = e.Benefits.BenefitEnrollmentHistory.Select(x => new BenefitEnrollmentHistoryModel(x)).OrderByDescending(x => x.CreatedOn).ToList();
            BenefitDependentHistory = e.Benefits.BenefitDependentHistory.Select(x => new BenefitDependentHistoryModel(x)).OrderByDescending(x => x.CreatedOn).ToList();

            LoadPropertyValuesWithThisEntity(entity);
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, GenderCode);
            LoadCorrectPropertyValueRef(entity, CostCenterCode);
            foreach (var benefitEnrollmentModel in BenefitEnrollment)
            {
                benefitEnrollmentModel.LoadPropertyValuesWithThisEntity(entity);
            }

            foreach (var benefitDependentModel in BenefitDependent)
            {
                benefitDependentModel.LoadPropertyValuesWithThisEntity(entity);
            }

            foreach (var benefitEnrollmentHistoryModel in BenefitEnrollmentHistory)
            {
                benefitEnrollmentHistoryModel.LoadPropertyValuesWithThisEntity(entity);
            }

            foreach (var benefitDependentHistoryModel in BenefitDependentHistory)
            {
                benefitDependentHistoryModel.LoadPropertyValuesWithThisEntity(entity);
            }
        }
    }
}