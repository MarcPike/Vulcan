using System;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Training
{
    [BsonIgnoreExtraElements]
    public class TrainingEventRef : ReferenceObject<TrainingEvent>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TrainingCourseRef TrainingCourse { get; set; }


        public TrainingEventRef(TrainingEvent t) : base(t)
        {
            TrainingCourse = t.TrainingCourse;
            StartDate = t.StartDate;
            EndDate = t.EndDate;
        }

        public TrainingEvent AsTrainingEvent()
        {
            return ToBaseDocument();
        }
    }
}