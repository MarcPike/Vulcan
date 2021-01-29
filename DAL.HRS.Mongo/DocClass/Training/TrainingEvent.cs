using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class TrainingEvent : BaseDocument
    {
        public static MongoRawQueryHelper<TrainingEvent> Helper = new MongoRawQueryHelper<TrainingEvent>();

        public int OldHrsId { get; set; } = 0;
        //public EmployeeRef Employee { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? StartDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? EndDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? CertificateExpiration { get; set; }

        public decimal Cost { get; set; }
        public TrainingCourseRef TrainingCourse { get; set; }
        public List<TrainingAttendee> Attendees { get; set; } = new List<TrainingAttendee>();
        public string ExternalInstructor { get; set; }
        public EmployeeRef InternalInstructor { get; set; }
        public LocationRef Location { get; set; }


        public decimal TrainingHours { get; set; } = 0;

        public bool HasAttendeeList(List<string> empIdList)
        {
            return Attendees.Any(x => empIdList.Contains(x.Employee.Id));
        }

        public bool HasAttendee(string employeeId)
        {
            return Attendees.Any(x => x.Employee.Id == employeeId);
        }

        public TrainingEventRef AsTrainingEventRef()
        {
            return new TrainingEventRef(this);
        }
    }
}