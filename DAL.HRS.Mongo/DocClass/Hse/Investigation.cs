using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class Investigation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string InvestigatorName { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? InvestigatedDate { get; set; }

        public override int GetHashCode()
        {
            var hashValue = InvestigatorName + InvestigatedDate?.ToLongDateString();
            return hashValue.GetHashCode();
        }

        public override string ToString()
        {
            return $"Investigator: {InvestigatorName} Date: {InvestigatedDate}";
        }
    }
}