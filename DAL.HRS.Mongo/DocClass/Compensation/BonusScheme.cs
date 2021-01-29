using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Compensation
{
    public class BonusScheme : ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PropertyValueRef BonusSchemeType { get; set; }
        public string Comment { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? EffectiveDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? EndDate { get; set; }
        public PropertyValueRef PayFrequencyType { get; set; }
        public float? TargetPercentage { get; set; }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = BonusSchemeType?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            modified = PayFrequencyType?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            return modified;
        }
    }
}