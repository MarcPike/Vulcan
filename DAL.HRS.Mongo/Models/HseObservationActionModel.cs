using System;
using DAL.HRS.Mongo.DocClass.Hse;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.Models
{
    public class HseObservationActionModel
    {
        public string Id { get; set; } 
        public string ActionRequired { get; set; }
        public string AssignedTo { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DueDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CompletionDate { get; set; }

        public HseObservationActionModel() { }

        public HseObservationActionModel(HseObservationAction a)
        {
            Id = a.Id.ToString();
            ActionRequired = a.ActionRequired;
            AssignedTo = a.AssignedTo;
            DueDate = a.DueDate;
            CompletionDate = a.CompletionDate;
        }

    }
}