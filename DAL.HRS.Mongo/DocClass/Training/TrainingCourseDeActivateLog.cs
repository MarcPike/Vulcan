using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class TrainingCourseDeActivateLog: BaseDocument
    {
        public TrainingCourseRef TrainingCourse { get; set; }
        
        public HrsUserRef DeActivatedByUser { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DeactivateDate { get; set; } = DateTime.Now;

        public static MongoRawQueryHelper<TrainingCourseDeActivateLog> Helper = new MongoRawQueryHelper<TrainingCourseDeActivateLog>();

        public TrainingCourseDeActivateLog()
        {
            
        }

        public TrainingCourseDeActivateLog(TrainingCourse course, HrsUserRef user)
        {
            TrainingCourse = course.AsTrainingCourseRef();
            DeActivatedByUser = user;

            var trainingEvents = TrainingEvent.Helper
                .Find(x => x.TrainingCourse.Id == TrainingCourse.Id && x.StartDate > DateTime.Now).ToList();

            foreach (var trainingEvent in trainingEvents)
            {
                var trainingEventId = trainingEvent.Id.ToString();
                foreach (var trainingEventAttendee in trainingEvent.Attendees)
                {
                    var employee = trainingEventAttendee.Employee.AsEmployee();
                    var employeeId = employee.Id.ToString();

                    foreach (var trainingEventRef in employee.TrainingEvents.Where(x=>x.Id == trainingEventId).ToList())
                    {
                        employee.TrainingEvents.Remove(trainingEventRef);
                    }

                    var requiredActivities = RequiredActivity.Helper.Find(x =>
                        x.Employee.Id == employeeId && x.TrainingCourse.Id == TrainingCourse.Id).ToList();

                    foreach (var requiredActivity in requiredActivities)
                    {
                        RequiredActivity.Helper.DeleteOne(requiredActivity.Id);
                    }
                }

                TrainingEvent.Helper.DeleteOne(trainingEvent.Id);
            }

            course.IsActive = false;
            DAL.HRS.Mongo.DocClass.Training.TrainingCourse.Helper.Upsert(course);

            Helper.Upsert(this);

        }
    }
}
