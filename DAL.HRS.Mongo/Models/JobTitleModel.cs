using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Training;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Models
{
    public class JobTitleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<TrainingScheduleCourseModel> TrainingScheduledCourses { get; set; } = new List<TrainingScheduleCourseModel>();
        public List<TrainingInitialCourseModel> TrainingInitialCourses { get; set; } = new List<TrainingInitialCourseModel>();

        public JobTitleModel()
        {
        }

        public JobTitleModel(JobTitle j)
        {
            Id = j.Id.ToString();
            Name = j.Name;
            Notes = j.Notes;
            IsActive = j.IsActive;
            TrainingScheduledCourses =
                j.TrainingScheduledCourses.Select(x => new TrainingScheduleCourseModel(x)).ToList();
            TrainingInitialCourses = 
                j.TrainingInitialCourses.Select(x=> new TrainingInitialCourseModel(x)).ToList();

        }
    }
}
