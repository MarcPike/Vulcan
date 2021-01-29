using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Benefits
{
    public class BenefitEnrollmentHistory : ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? CreatedOn { get; set; }
        public PropertyValueRef BenefitPlan { get; set; }
        public PropertyValueRef OptionTypeCode { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? BeginDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? EndDate { get; set; }
        public decimal EmployeeCost { get; set; }
        public decimal EmployerCost { get; set; }
        public decimal TotalCost => EmployeeCost + EmployerCost;
        public PropertyValueRef CoverageType { get; set; }
        public PropertyValueRef StatusChangeType { get; set; }
        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = BenefitPlan?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            modified = OptionTypeCode?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            modified = CoverageType?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            modified = StatusChangeType?.ChangeOfficeName(locationId, newName, modified) ?? modified;

            return modified;
        }
    }
}