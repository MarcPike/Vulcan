using System;
using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class Certification : BaseDocument, ISupportLocationNameChanges
    {
        public string Name { get; set; }
        public int RequiredEveryMonths { get; set; } //= DateTime.Now.Date - DateTime.Now.Date.AddMonths(-6);
        public int RequiredEveryWeeks { get; set; } //= DateTime.Now.Date - DateTime.Now.Date.AddMonths(-6);
        public int RequiredEveryDays { get; set; } //= DateTime.Now.Date - DateTime.Now.Date.AddMonths(-6);
        public TrainingCourseRef TrainingCourse { get; set; }

        public static MongoRawQueryHelper<Certification> Helper = new MongoRawQueryHelper<Certification>();

        public CertificationRef AsCertificationRef()
        {
            return new CertificationRef(this);
        }

        public void ChangeOfficeName(string locationId, string newName)
        {
            var modified = false;
            if (TrainingCourse != null)
            {
                modified = TrainingCourse.ChangeOfficeName(locationId, newName, modified);
            }

            if (modified) Helper.Upsert(this);
        }
    }
}
