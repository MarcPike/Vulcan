using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class HseObservationAction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ActionRequired { get; set; }
        public string AssignedTo { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DueDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CompletionDate { get; set; }
    }
}