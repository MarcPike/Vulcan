using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Benefits
{
    public class BenefitDependent : ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DependentName { get; set; }
        public PropertyValueRef RelationShip { get; set; }
        public PropertyValueRef Gender { get; set; }
        public string GovernmentId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? DateOfBirth { get; set; }
        public string PrimaryCarePhysicianId { get; set; }
        public bool? Surcharge { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? BeginDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? EndDate { get; set; }
        public PropertyValueRef DesignationType { get; set; }
        public PropertyValueRef BeneficiaryPercentageType { get; set; }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = RelationShip.ChangeOfficeName(locationId, newName, modified);
            modified = Gender.ChangeOfficeName(locationId, newName, modified);
            modified = DesignationType.ChangeOfficeName(locationId, newName, modified);
            modified = BeneficiaryPercentageType.ChangeOfficeName(locationId, newName, modified);

            return modified;
        }
    }
}