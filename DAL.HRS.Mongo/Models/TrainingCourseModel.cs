using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;

namespace DAL.HRS.Mongo.Models
{
    public class TrainingCourseModel: BaseModel, IHavePropertyValues
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PropertyValueRef CourseType { get; set; }
        public PropertyValueRef VenueType { get; set; }
        public PropertyValueRef GroupClassification { get; set; }
        public int ExpirationMonths { get; set; }
        public int OldHrsId { get; set; }
        public bool IsActive { get; set; }

        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();

        public TrainingCourseModel()
        {
        }

        public TrainingCourseModel(TrainingCourse t)
        {
            Id = t.Id.ToString();
            Name = t.Name;
            CourseType = t.CourseType;
            VenueType = t.VenueType;
            GroupClassification = t.GroupClassification;
            Description = t.Description;
            ExpirationMonths = t.ExpirationMonths;
            OldHrsId = t.OldHrsId;
            Locations = t.Locations ?? new List<LocationRef>();
            IsActive = t.IsActive;
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            
        }
    }
}
