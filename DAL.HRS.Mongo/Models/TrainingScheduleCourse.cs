using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Training;

namespace DAL.HRS.Mongo.Models
{
    public class TrainingScheduleCourseModel
    {
        public string Id { get; set; }
        public TrainingCourseRef TrainingCourse { get; set; }
        public int RepeatEveryNumberOfMonths { get; set; } = 0;

        public TrainingScheduleCourseModel()
        {
        }

        public TrainingScheduleCourseModel(TrainingScheduledCourse s)
        {
            Id = s.Id.ToString();
            TrainingCourse = s.TrainingCourse;
            RepeatEveryNumberOfMonths = s.RepeatEveryNumberOfMonths;
        }

    }
}
