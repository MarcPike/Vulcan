using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class Witness
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string WitnessName { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? WitnessDate { get; set; }

        public override int GetHashCode()
        {
            var hashValue = WitnessName + WitnessDate?.ToLongDateString() ?? "";
            return hashValue.GetHashCode();
        }

        public override string ToString()
        {
            return $"Witness: {WitnessName} Date: [{WitnessDate?.ToLongDateString()}]";
        }
    }
}