using DAL.HRS.Mongo.DocClass.Properties;
using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.DocClass.Benefits
{
    public class BenefitEnrollmentModel: BaseListModel, IHavePropertyValues
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public PropertyValueRef PlanType { get; set; }
        public PropertyValueRef OptionTypeCode { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? EmployeeContribution { get; set; }
        public decimal? EmployerContribution { get; set; }
        public decimal TotalCost => (EmployeeContribution ?? 0) + (EmployerContribution ?? 0);
        public PropertyValueRef CoverageType { get; set; }
        public PropertyValueRef StatusChangeType { get; set; }
        public bool AddedOrModified { get; set; } = false;

        public BenefitEnrollmentModel()
        {

        }

        public override string ToString()
        {
            return $"Id: {Id} PlanType: {PlanType?.Code} Option: {OptionTypeCode?.Code} Begin: {BeginDate} End: {EndDate} EmployeeCont: {EmployeeContribution} EmployerCont: {EmployerContribution} CoverType: {CoverageType?.Code} Status Change: {StatusChangeType?.Code}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, PlanType);
            LoadCorrectPropertyValueRef(entity, OptionTypeCode);
            LoadCorrectPropertyValueRef(entity, CoverageType);
            LoadCorrectPropertyValueRef(entity, StatusChangeType);
        }

        public BenefitEnrollmentModel(BenefitEnrollment enr)
        {
            Id = enr.Id.ToString();
            PlanType = enr.PlanType;
            OptionTypeCode = enr.OptionTypeCode;
            BeginDate = enr.BeginDate?.Date;
            EndDate = enr.EndDate?.Date;
            EmployeeContribution = enr.EmployeeContribution;
            EmployerContribution = enr.EmployerContribution;
            CoverageType = enr.CoverageType;
            StatusChangeType = enr.StatusChangeType;
            AddedOrModified = false;
        }
    }
}