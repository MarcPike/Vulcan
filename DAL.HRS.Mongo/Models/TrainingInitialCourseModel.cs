using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Training;

namespace DAL.HRS.Mongo.Models
{
    public class TrainingInitialCourseModel
    {
        public string Id { get; set; }
        public TrainingCourseRef TrainingCourse { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public int DaysToComplete { get; set; } = 90;

        public TrainingInitialCourseModel()
        {
        }

        public TrainingInitialCourseModel(TrainingInitialCourse t)
        {
            Id = t.Id.ToString();
            TrainingCourse = t.TrainingCourse;
            Description = t.Description;
            Comments = t.Comments;
            DaysToComplete = t.DaysToComplete;
        }

    }
}