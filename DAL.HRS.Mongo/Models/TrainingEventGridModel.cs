using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;

namespace DAL.HRS.Mongo.Models
{
    public class TrainingEventGridModel
    {
        public string Id { get; set; }
        //public EmployeeRef Employee { get; set; }
        public int OldHrsId { get; set; } = 0;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CertificateExpiration { get; set; }
        public decimal Cost { get; set; }
        public TrainingCourseRef TrainingCourse { get; set; }
        public int AttendeeCount { get; set; }
        public string ExternalInstructor { get; set; }
        public EmployeeRef InternalInstructor { get; set; }

        public LocationRef Location { get; set; }

        public TrainingEventGridModel()
        {
        }

        public TrainingEventGridModel(TrainingEvent trainingEvent)
        {
            Id = trainingEvent.Id.ToString();
            OldHrsId = trainingEvent.OldHrsId;
            StartDate = trainingEvent.StartDate;
            EndDate = trainingEvent.EndDate;
            CertificateExpiration = trainingEvent.CertificateExpiration;
            Cost = trainingEvent.Cost;
            TrainingCourse = trainingEvent.TrainingCourse;
            AttendeeCount = trainingEvent.Attendees.Count;
            ExternalInstructor = trainingEvent.ExternalInstructor;
            InternalInstructor = trainingEvent.InternalInstructor;
            Location = trainingEvent.Location;
        }
    }
}