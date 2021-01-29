using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Benefits
{
    public class BenefitDependentHistory : ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreatedOn { get; set; }
        public string DependentName { get; set; }
        public PropertyValueRef RelationShip { get; set; }
        public string GovernmentId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? DateOfBirth { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? BeginDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? EndDate { get; set; }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = RelationShip.ChangeOfficeName(locationId, newName, modified);

            return modified;
        }
    }
}