using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Training;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace DAL.HRS.Mongo.Models
{
    public class TrainingEventSmallModel
    {
        public string Id { get; set; }
        public TrainingCourseRef TrainingCourse { get; set; }
        public int OldHrsId { get; set; } = 0;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CertificateExpiration { get; set; }
        public decimal Cost { get; set; }
        public int AttendeesCount { get; set; }
        public string ExternalInstructor { get; set; }
        public EmployeeRef InternalInstructor { get; set; }

        public LocationRef Location { get; set; }

        public TrainingEventSmallModel()
        {
        }
    }
}