using System;
using System.Diagnostics;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Employee_Tests
{
    [TestFixture()]
    public class GetAllEmployeesTesting
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();

        }

        [Test]
        public void UsingMongoRawQueryHelper()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var queryHelper = new MongoRawQueryHelper<Employee>();

            var filterBuilder = queryHelper.FilterBuilder;
            var filter = filterBuilder.Eq(x => x.LastName, "Wilson");

            var projectBuilder = queryHelper.ProjectionBuilder;
            var project = projectBuilder.Expression(x => new {x.FirstName, x.LastName, x.CostCenterCode});

            var sortBuilder = queryHelper.SortBuilder;
            var sort = sortBuilder.Ascending(x => x.FirstName).Descending(x => x.CostCenterCode.Code);

            var collection = queryHelper.Collection;
            var results = collection.Find(filter).Project(project).Sort(sort).ToList();

            foreach (var result in results)
            {
                Console.WriteLine($"First: {result.FirstName} Last: {result.LastName} CostCenter: {result.CostCenterCode.Code}-{result.CostCenterCode.Description}");
            }
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");

        }

        [Test]
        public void ExecuteTimeTest()
        {
            var helperEmployee = new HelperEmployee();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var resultProjection = helperEmployee.GetAllMyEmployeeGridModelsForModule("5d7a6bbe095f581cd8805fa7", "Performance", true);
            sw.Stop();
            Console.WriteLine($"BaseGridModel took {sw.Elapsed.Seconds}");

            sw = new Stopwatch();
            sw.Start();
            var finalResult = resultProjection.Select(x => new PerformanceGridModel(x)).ToList();
            sw.Stop();
            Console.WriteLine($"Projection took {sw.Elapsed.Seconds}");

            //sw.Start();
            //var resultEmployees = helperEmployee.GetAllMyEmployeesForModule("5cd3205cae7ad422c84f1f54", "Benefits", true);
            //sw.Stop();
            //Console.WriteLine($"Full Employee took {sw.Elapsed.Seconds}");

        }

        [Test]
        public void TestBuilderProjection()
        {
            var builder = Builders<Employee>.Projection;
            var projection = builder.Expression(x =>
                new BaseGridModel(
                    //new  { 
                    (x.TerminationDate == null || x.TerminationDate > DateTime.Now),
                    x.Birthday,
                    x.Compensation.BaseHours,
                    x.BusinessRegionCode,
                    x.CostCenterCode,
                    x.Id,
                    x.FirstName,
                    x.GenderCode,
                    x.GovernmentId,
                    x.OriginalHireDate,
                    x.TerminationDate,
                    x.JobTitle,
                    x.KronosDepartmentCode,
                    x.LastName,
                    x.Location,
                    x.Manager,
                    x.MiddleName,
                    x.Compensation.PayGradeType,
                    x.PayrollId,
                    x.PayrollRegion,
                    x.PreferredName,
                    x.Status1Code,
                    x.OldHrsId,
                    x.LdapUser,
                    x.BusinessUnitCode,
                    x.CompanyNumberCode
                ));
                   //});

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var collection = new RepositoryBase<Employee>().Collection;
            var results = collection.Find(new BsonDocument()).Project(projection).ToList();
            sw.Stop();
            Console.WriteLine($"Rows found = {results.Count} Elapsed: {sw.Elapsed.Seconds}");

        }
    }
}
