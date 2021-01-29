using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Training
{
    [TestFixture]
    public class PopulateCertification
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var queryHelper = new MongoRawQueryHelper<TrainingCourse>();

            var filterCerts = queryHelper.FilterBuilder.Where(x => x.ExpirationMonths > 0);
            var coursesThatAreCerts = queryHelper.Find(filterCerts).ToList();
            var queryHelperCerts = new MongoRawQueryHelper<Certification>();
            foreach (var course in coursesThatAreCerts)
            {
                var filterCert = queryHelperCerts.FilterBuilder.Where(x => x.Name == course.Name);

                var certification = queryHelperCerts.Find(filterCert).FirstOrDefault();

                if (certification == null)
                {
                    certification = new Certification()
                    {
                        Name = course.Name,
                        RequiredEveryMonths = course.ExpirationMonths,
                        RequiredEveryDays = 0,
                        RequiredEveryWeeks = 0,
                        TrainingCourse = course.AsTrainingCourseRef()
                    };
                    queryHelperCerts.Upsert(certification);
                }

                course.Certification = certification.AsCertificationRef();
                queryHelper.Upsert(course);
            }
        }


    }
}
