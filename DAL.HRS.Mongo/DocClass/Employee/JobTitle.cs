using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class JobTitle : BaseDocument, ISupportLocationNameChanges
    {
        public string Name { get; set; }
        public string Notes { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public List<TrainingScheduledCourse> TrainingScheduledCourses { get; set; } = new List<TrainingScheduledCourse>();
        public List<TrainingInitialCourse> TrainingInitialCourses { get; set; } = new List<TrainingInitialCourse>();

        public static  MongoRawQueryHelper<JobTitle> Helper = new MongoRawQueryHelper<JobTitle>();

        public JobTitleRef AsJobTitleRef()
        {
            return new JobTitleRef(this);
        }

        public void ChangeOfficeName(string locationId, string newName)
        {
            var modified = false;


            foreach (var item in TrainingScheduledCourses)
            {
                modified = item.ChangeOfficeName(locationId, newName, modified);
            }

            foreach (var item in TrainingInitialCourses)
            {
                modified = item.ChangeOfficeName(locationId, newName, modified);
            }


            if (modified) Helper.Upsert(this);
        }
    }
}
