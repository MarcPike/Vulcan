using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class EmployeeOtherMedicalInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Date { get; set; }
        public string Comments { get; set; }
    }
}