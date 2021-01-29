using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DAL.HRS.Mongo.DocClass.Compensation
{
    [BsonIgnoreExtraElements]
    public class KronosPayRuleHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? KronosPayRuleEffectiveDate { get; set; }
        //public string KronosPayRuleName { get; set; }
        public PropertyValueRef KronosPayRuleType { get; set; }

    }

}