using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class ReqActivities
    {
        public PropertyValueRef ColorCode { get; set; }

        public int DaysRemaining { get; set; }
        
        public PropertyValueRef ActivityTypeCode { get; set; } 

        public string Description { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CompletionDeadline { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime RevCompDeadline { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DateCompleted { get; set; }

        public string CompletionStatus { get; set; }

        public bool EmployeeIsActive { get; set; }
        public bool HasSupportDocs { get; set; }
    }
}