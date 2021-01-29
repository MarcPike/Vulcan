using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class TrainingInitialCourse: ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public TrainingCourseRef TrainingCourse { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public int DaysToComplete { get; set; } = 90;
        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = TrainingCourse.ChangeOfficeName(locationId, newName, modified);

            return modified;
        }
    }
}
