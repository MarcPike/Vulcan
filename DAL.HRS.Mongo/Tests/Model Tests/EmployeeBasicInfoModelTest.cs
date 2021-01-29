using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Mongo.Tests.Model_Tests
{
    [TestFixture()]
    public class EmployeeBasicInfoModelTest
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var filter = queryHelper.FilterBuilder.Where(x => x.LastName == "Pike");
            var project = queryHelper.ProjectionBuilder.Expression(x => x.Id);

            var sw = new Stopwatch();
            foreach (var employeeId in queryHelper.FindWithProjection(filter,project).ToList())
            {
                sw.Start();
                var model = EmployeeBasicInfoModel.FindById(employeeId);
                sw.Stop();
                Console.WriteLine($"Elapsed: {sw.Elapsed}");
                Console.WriteLine(ObjectDumper.Dump(model));
            }
        }
    }
}
