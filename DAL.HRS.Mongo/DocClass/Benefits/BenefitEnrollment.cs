using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Benefits
{
    public class BenefitEnrollment: ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PropertyValueRef PlanType { get; set; } 
        public PropertyValueRef OptionTypeCode { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? BeginDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? EndDate { get; set; }

        public decimal? EmployeeContribution { get; set; } = 0;
        public decimal? EmployerContribution { get; set; } = 0;
        public decimal TotalCost => (EmployeeContribution ?? 0) + (EmployerContribution ?? 0);
        public PropertyValueRef CoverageType { get; set; } 
        public PropertyValueRef StatusChangeType { get; set; }
        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = PlanType?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            modified = OptionTypeCode?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            modified = CoverageType?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            modified = StatusChangeType?.ChangeOfficeName(locationId, newName, modified) ?? modified;

            return modified;
        }
    }
}