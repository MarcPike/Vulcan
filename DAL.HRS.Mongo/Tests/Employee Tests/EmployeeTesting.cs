using System;
using System.Diagnostics;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Employee_Tests
{
    [TestFixture()]
    public class EmployeeTesting
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void EmployeeByLocation()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var helperEmployee = new HelperEmployee();
            var telge = new RepositoryBase<Location>().AsQueryable().Single(x=>x.Office == "Telge");
            stopWatch.Stop();

            var employees = helperEmployee.GetAllEmployeeReferencesForLocation(telge.Id.ToString());

            Console.WriteLine($"Elapsed time: {stopWatch.Elapsed}");

            foreach (var employeeRef in employees.OrderBy(x=>x.LastName))
            {
                Console.WriteLine(employeeRef.GetFullName());
            }

        }

        [Test]
        public void EmployeePossibleManagers()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var helperEmployee = new HelperEmployee();
            var employee = new RepositoryBase<Employee>().AsQueryable().FirstOrDefault(x=>x.LastName == "Centanni");

            var managers = helperEmployee.GetAllEmployeeReferencesOfPossibleManagers(employee.Id.ToString());

            stopWatch.Stop();

            Console.WriteLine($"Elapsed time: {stopWatch.Elapsed}");

            foreach (var employeeRef in managers)
            {
                Console.WriteLine(employeeRef.GetFullName());
            }

        }

        [Test]
        public void GetAllHrsUsers()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var helperUser = new HelperUser();

            var hrsUsers = helperUser.GetAllHrsUsers();

            stopWatch.Stop();

            Console.WriteLine($"Elapsed time: {stopWatch.Elapsed}");

            foreach (var hrsUserModel in hrsUsers)
            {
                Console.WriteLine(ObjectDumper.Dump(hrsUserModel));
            }

        }

        [Test]
        public void PerformanceReviewerNotNull()
        {
            var rep = new RepositoryBase<Employee>();
            foreach (var employee in rep.AsQueryable().ToList())
            {
                if (employee.Performance.Any(x => x.Reviewer != null))
                {
                    Console.WriteLine(employee.PayrollId);
                    continue;
                }
            }
        }

        [Test]
        public void TestRawQuery()
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();
            Stopwatch sw = new Stopwatch();

            sw.Start();
            var filter = queryHelper.FilterBuilder.Where(x => x.Location.Office == "Telge");
            var project = queryHelper.ProjectionBuilder.Expression(x => new
                {x.LastName, x.FirstName, Age = x.GetAge(), x.CostCenterCode.Code, x.JobTitle.Name});
            var sort = queryHelper.SortBuilder.Ascending(x => x.LastName).Ascending(x => x.FirstName);

            var results = queryHelper.FindWithProjection(filter, project, sort).ToList();

            sw.Stop();
            Console.WriteLine("-----------------------------------------------------------------------------");
            Console.WriteLine($"2nd pass Elapsed Time: {sw.Elapsed} Rows: {results.Count()}");
            Console.WriteLine();

            foreach (var result in results)
            {
                Console.WriteLine($"LastName: {result.LastName}\n" +
                                  $"FirstName: {result.FirstName}\n" +
                                  $"Age: {result.Age}\n" +
                                  $"Cost Center Code: {result.Code}\n" +
                                  $"Job Title: {result.Name}\n");
            }


        }

        //[Test]
        //public void TestExplain()
        //{
        //    var queryHelper = new MongoRawQueryHelper<Employee>();
        //    Stopwatch sw = new Stopwatch();

        //    sw.Start();
        //    var filter = queryHelper.FilterBuilder.Where(x => x.Location.Office == "Telge");

        //    var explain = queryHelper.Explain(filter);
        //    sw.Stop();
        //    Console.WriteLine($"Time to explain: {sw.Elapsed}");
        //    Console.WriteLine(explain.ToString());
        //}
    }
}
