using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Training;

namespace DAL.HRS.Mongo.Models
{
    public class JobTitleCourseCopyModel
    {
        public JobTitleRef Source { get; set; }
        public JobTitleRef Target { get; set; }
        public List<TrainingCourseSelectionModel> TrainingCoursesToCopy { get; set; } = new List<TrainingCourseSelectionModel>();

        public JobTitleCourseCopyModel(JobTitle source, JobTitle target)
        {
            Source = source.AsJobTitleRef();
            Target = target.AsJobTitleRef();
            foreach (var trainingCourse in source.TrainingInitialCourses)
            {
                if (target.TrainingInitialCourses.All(x => x.Id != trainingCourse.Id))
                {
                    TrainingCoursesToCopy.Add(new TrainingCourseSelectionModel()
                    {
                        TrainingCourse = trainingCourse,
                        Checked = false
                    });
                }
            }
        }

        public JobTitleCourseCopyModel()
        {

        }
    }

    public class TrainingCourseSelectionModel
    {
        public TrainingInitialCourse TrainingCourse { get; set; }
        public bool Checked { get; set; }
    }
}
