using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Static_Helper_Classes;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.EmployeeRolodex
{
    [TestFixture()]
    public class TestRolodex
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public async Task Execute()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var rolodex = new Models.EmployeeRolodex();
            rolodex.Filter = rolodex.QueryHelper.FilterBuilder.Where(x=>x.Location.Office == "Telge");
            await rolodex.FetchResults();
            sw.Stop();
            Console.WriteLine($"Total Rows: {rolodex.Rolodex.Sum(x => x.Count)} Elapsed: {sw.Elapsed}");
            Console.WriteLine(rolodex.DumpAsYaml());

            foreach (var employeeGroup in rolodex.Rolodex)
            {
                Console.WriteLine($"{employeeGroup.Label} - {employeeGroup.Count}");
                foreach (var employeeRef in employeeGroup.Employees.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).Take(3))
                {
                    Console.WriteLine($"{employeeRef.FullName}");
                }
                Console.WriteLine("...");
                var lastOne = employeeGroup.Employees.OrderBy(x=>x.LastName).ThenBy(x=>x.FirstName).LastOrDefault();
                if (lastOne != null)
                {
                    Console.WriteLine($"{lastOne.FullName}");
                }
                Console.WriteLine("------------------------------------------");
            }
        }
    }
}
