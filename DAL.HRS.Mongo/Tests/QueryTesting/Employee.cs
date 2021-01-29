using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.QueryTesting
{

    [TestFixture]
    public class EmployeeTesting
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void MyTest()
        {

            var queryBuilder = new MongoRawQueryHelper<Employee>();

            var result = queryBuilder.FindWithProjection(
                queryBuilder.FilterBuilder.Where(x => x.LastName.StartsWith("S")),
                queryBuilder.ProjectionBuilder.Expression(x => new
                    {x.Id, x.LastName, x.FirstName, x.PayrollId, HasComp = x.Compensation != null}),
                queryBuilder.SortBuilder.Ascending(x=>x.PayrollId));

            foreach (var value in result)
            {
                Console.WriteLine($"{value.PayrollId} - {value.LastName}, {value.FirstName} Comp?: {value.HasComp}");
            }


        }
    }
}


