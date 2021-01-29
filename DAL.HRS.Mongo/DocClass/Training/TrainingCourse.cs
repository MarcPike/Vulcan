using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class TrainingCourse : BaseDocument
    {
        public static MongoRawQueryHelper<TrainingCourse> Helper = new MongoRawQueryHelper<TrainingCourse>();
        public int OldHrsId { get; set; } = 0;
        public string Name { get; set; }
        public string Description { get; set; }
        public int ExpirationMonths { get; set; }
        public PropertyValueRef GroupClassification { get; set; }
        public PropertyValueRef CourseType { get; set; }
        public PropertyValueRef VenueType { get; set; }

        public List<LocationRef> Locations { get; set; }
        public CertificationRef Certification { get; set; }
        public bool IsActive { get; set; } = true; // This is all that is needed to add a field

        public TrainingCourseRef AsTrainingCourseRef()
        {
            return new TrainingCourseRef(this);
        }

    }
}
