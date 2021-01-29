using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using System;
using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Dashboard;

namespace DAL.HRS.Mongo.Models
{
    public class TrainingEventModel
    {
        public string Id { get; set; }
        public TrainingCourseRef TrainingCourse { get; set; }
        public int OldHrsId { get; set; } = 0;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CertificateExpiration { get; set; }
        public decimal Cost { get; set; }
        public List<TrainingAttendee> Attendees { get; set; } = new List<TrainingAttendee>();
        public string ExternalInstructor { get; set; }
        public EmployeeRef InternalInstructor { get; set; }

        public decimal TrainingHours { get; set; } 

        public LocationRef Location { get; set; }

        public TrainingEventModel()
        {
        }

        public TrainingEventModel(TrainingEvent trainingEvent)
        {
            Id = trainingEvent.Id.ToString();
            OldHrsId = trainingEvent.OldHrsId;
            StartDate = trainingEvent.StartDate;
            EndDate = trainingEvent.EndDate;
            CertificateExpiration = trainingEvent.CertificateExpiration;
            Cost = trainingEvent.Cost;
            TrainingCourse = trainingEvent.TrainingCourse;
            if (trainingEvent.TrainingCourse != null)
            {
                var trainingCourse = trainingEvent.TrainingCourse.AsTrainingCourse();


                if ((trainingCourse.ExpirationMonths > 0) && (trainingEvent.EndDate != null) && (CertificateExpiration == null))
                {
                    var endDate = EndDate ?? DateTime.Now.Date;
                    CertificateExpiration = endDate.AddMonths(trainingCourse.ExpirationMonths);
                    
                }


            }

            Attendees = trainingEvent.Attendees;
            //foreach (var attendee in Attendees)
            //{
            //    attendee.Employee.Refresh();
            //}
            ExternalInstructor = trainingEvent.ExternalInstructor;
            InternalInstructor = trainingEvent.InternalInstructor?.Refresh();
            Location = trainingEvent.Location;
            TrainingHours = trainingEvent.TrainingHours;
        }
    }
}
