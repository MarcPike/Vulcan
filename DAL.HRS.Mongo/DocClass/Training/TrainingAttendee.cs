using System;
using DAL.HRS.Mongo.DocClass.Employee;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class TrainingAttendee
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int OldHrsId { get; set; } = 0;
        public EmployeeRef Employee { get; set; }
        public decimal Reimbursement { get; set; }
        public bool Dismissed { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? DateCompleted { get; set; }

        public RequiredActivityRef RequiredActivity { get; set; }

        public override string ToString()
        {
            var nullValue = "(null)";
            return $"{Id}-{Employee.Id}-{Reimbursement}-{Dismissed}-{DateCompleted?.ToString() ?? nullValue}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}