using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class TrainingScheduledCourse : ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public TrainingCourseRef TrainingCourse { get; set; }
        public int RepeatEveryNumberOfMonths { get; set; } = 0;

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = TrainingCourse.ChangeOfficeName(locationId, newName, modified);
            return modified;
        }
    }
}
