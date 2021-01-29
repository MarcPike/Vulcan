using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Dashboard;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperDashboardTests
{
    [TestFixture]
    public class HelperDashboardTest
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void LoadDashboardItemsForTrainingEvents()
        {
            var today = DateTime.Now.Date;
            var queryHelperEmployee = new MongoRawQueryHelper<Employee>();
            var filterActive = queryHelperEmployee.FilterBuilder.Where(x => (x.TerminationDate == null || x.TerminationDate > DateTime.Now) && x.TrainingEvents.Any());
            var project = queryHelperEmployee.ProjectionBuilder.Expression(x => new { x.Id, TrainingEvents = x.TrainingEvents});

            var empEvents = queryHelperEmployee.FindWithProjection(filterActive, project).ToList();

            var queryHelperTrainingEvent = new MongoRawQueryHelper<TrainingEvent>();
            var queryHelperDashboardTrainingEvent = new MongoRawQueryHelper<DashboardItem>();

            foreach (var empEvent in empEvents)
            {
                foreach (var trainingEventRef in empEvent.TrainingEvents)
                {
                    var trainingEvent = trainingEventRef.AsTrainingEvent();

                    if (trainingEvent.CertificateExpiration != null)
                    {
                        var dashboardTrainingEvent = new DashboardItem()
                        {
                            Type = DashboardItemType.TrainingCertification,
                            DueDate = trainingEvent.CertificateExpiration ?? DateTime.Parse("1/1/1900"),
                            Employee = queryHelperEmployee.FindById(empEvent.Id).AsEmployeeRef(),
                            TrainingEvent = trainingEventRef
                        };

                        queryHelperDashboardTrainingEvent.Upsert(dashboardTrainingEvent);

                    }

                }
            }

        }


    }
}
