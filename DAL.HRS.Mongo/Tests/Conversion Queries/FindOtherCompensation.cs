using System;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Conversion_Queries
{
    [TestFixture()]
    public class FindOtherCompensation
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var query = new RepositoryBase<Employee>().AsQueryable()
                .Where(x => x.Compensation.OtherCompensation.Count > 0).ToString();
            var employeesWithComp = new RepositoryBase<Employee>().AsQueryable()
                .Where(x => x.Compensation.OtherCompensation.Count > 0).ToList();
            foreach (var employee in employeesWithComp)
            {
                Console.WriteLine($"{employee.OldHrsId} - {employee.FirstName} {employee.LastName}");
            }

        }
    }
}
