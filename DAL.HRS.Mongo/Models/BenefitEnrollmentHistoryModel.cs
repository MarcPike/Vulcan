using DAL.HRS.Mongo.DocClass.Properties;
using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.DocClass.Benefits
{
    public class BenefitEnrollmentHistoryModel: BaseListModel, IHavePropertyValues
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime? CreatedOn { get; set; }
        public PropertyValueRef BenefitPlan { get; set; }
        public PropertyValueRef OptionTypeCode { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal EmployeeCost { get; set; }
        public decimal EmployerCost { get; set; }
        public decimal TotalCost => EmployeeCost + EmployerCost;
        public PropertyValueRef CoverageType { get; set; }
        public PropertyValueRef StatusChangeType { get; set; }

        public override string ToString()
        {
            return
                $"Id: {Id} Created: {CreatedOn?.ToLongDateString()} Plan: {BenefitPlan?.Code} Option: {OptionTypeCode?.Code} Begin: {BeginDate?.ToLongDateString()} End: {EndDate?.ToLongDateString()} Cover Type: {CoverageType?.Code} Status: {StatusChangeType?.Code} Cost -> Employer: {EmployerCost} Employee: {EmployeeCost}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, BenefitPlan);
            LoadCorrectPropertyValueRef(entity, OptionTypeCode);
            LoadCorrectPropertyValueRef(entity, CoverageType);
            LoadCorrectPropertyValueRef(entity, StatusChangeType);
        }

        public BenefitEnrollmentHistoryModel() { }
        public BenefitEnrollmentHistoryModel(BenefitEnrollmentHistory hist)
        {
            Id = hist.Id.ToString();
            CreatedOn = hist.CreatedOn;
            BenefitPlan = hist.BenefitPlan;
            OptionTypeCode = hist.OptionTypeCode;
            BeginDate = hist.BeginDate?.Date;
            EndDate = hist.EndDate?.Date;
            EmployeeCost = hist.EmployeeCost;
            EmployerCost = hist.EmployerCost;
            CoverageType = hist.CoverageType;
            StatusChangeType = hist.StatusChangeType;
        }

    }
}