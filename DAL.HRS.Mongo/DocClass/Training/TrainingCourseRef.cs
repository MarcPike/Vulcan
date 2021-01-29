using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DocumentFormat.OpenXml.Wordprocessing;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Training
{
    [BsonIgnoreExtraElements]
    public class TrainingCourseRef : ReferenceObject<TrainingCourse>, ISupportLocationNameChangesNested
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PropertyValueRef GroupClassification { get; set; }
        public PropertyValueRef CourseType { get; set; }
        public PropertyValueRef VenueType { get; set; }
        public int ExpirationMonths { get; set; }

        public TrainingCourseRef()
        {
        }

        public TrainingCourseRef(TrainingCourse c) : base(c)
        {
            Name = c.Name;
            GroupClassification = c.GroupClassification;
            CourseType = c.CourseType;
            VenueType = c.VenueType;
            Description = c.Description;
            ExpirationMonths = c.ExpirationMonths;
        }

        public TrainingCourse AsTrainingCourse()
        {
            return ToBaseDocument();
        }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = GroupClassification.ChangeOfficeName(locationId, newName, modified);
            modified = CourseType.ChangeOfficeName(locationId, newName, modified);
            modified = VenueType.ChangeOfficeName(locationId, newName, modified);

            return modified;
        }
    }
}