using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.AllRequiredActivities
{

    [TestFixture]
    public class RequiredActivitiesQuery
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var sw = new Stopwatch();
            sw.Start();
            var requiredActivities = DAL.HRS.Mongo.Models.RequiredActivitiesQuery.GetAllForActiveEmployees(null);

            var trainingActivityCount = requiredActivities.Count(x => x.TrainingCourse != null);

            sw.Stop();
            Console.WriteLine($"Total: {requiredActivities.Count} Training: {trainingActivityCount} Duration: {sw.Elapsed}");
        }

        [Test]
        public void UpdateOldRequiredActivitiesAsCompleted()
        {
            var helper = new MongoRawQueryHelper<RequiredActivity>();
            var rows = helper.Find(helper.FilterBuilder.Empty).ToList();
            foreach (var requiredActivity in rows)
            {
                if ((requiredActivity.Status == RequiredActivityStatus.Unknown) &&
                    (requiredActivity.DateCompleted != null))
                {
                    requiredActivity.Status = RequiredActivityStatus.Completed;
                }
                if (requiredActivity.TrainingCourse != null)
                {
                    var trainingCourse = requiredActivity.TrainingCourse.AsTrainingCourse();
                    if (trainingCourse != null)
                    {
                        requiredActivity.Type = trainingCourse.Certification != null ? 
                            RequiredActivityType.TrainingCertification : RequiredActivityType.Training;
                    }
                }

                helper.Upsert(requiredActivity);
            }
        }
    }



}
