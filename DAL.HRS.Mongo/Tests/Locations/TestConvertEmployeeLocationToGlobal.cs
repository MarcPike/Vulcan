using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Mongo.Tests.Locations
{
    [TestFixture()]
    public class TestConvertEmployeeLocationToGlobal
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void ExamineOnly()
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var filter = queryHelper.FilterBuilder.Empty;
            var project = queryHelper.ProjectionBuilder.Expression(x => x.Location);
            var employeeLocations = queryHelper.FindWithProjection(filter, project).Take(10);

            foreach (var locationRef in employeeLocations)
            {
                var location = locationRef.AsLocation();
            }


        }
    }
}
