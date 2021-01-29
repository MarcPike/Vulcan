using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.DateFix
{
    [TestFixture]
    public class DateEval
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Test()
        {
            var project = Employee.Helper.ProjectionBuilder.
                Expression(x=> new {x.FirstName, x.LastName, x.PayrollId, x.TerminationDate});
            var filter = Employee.Helper.FilterBuilder.Where(x => x.TerminationDate != null);
            var termEmps = Employee.Helper.FindWithProjection(filter, project).ToList();
            foreach (var termEmp in termEmps)
            {
                var termDateTime = termEmp.TerminationDate;
                var termDateTruncated = termDateTime?.Date;

                if (termDateTime?.DayOfWeek != termDateTruncated?.DayOfWeek)
                {
                    Console.WriteLine($@"{termEmp.PayrollId},{termEmp.FirstName} {termEmp.LastName},{termDateTruncated?.ToShortDateString()}");
                }

            }
        }

    }
}
