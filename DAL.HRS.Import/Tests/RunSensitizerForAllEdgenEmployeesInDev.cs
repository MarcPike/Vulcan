using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Import.ImportHrs;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Import.Tests
{
    [TestFixture]
    public class RunSensitizerForAllEdgenEmployeesInDev
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var filter = Employee.Helper.FilterBuilder.Where(x => x.Entity.Name == "Edgen Murray");
            var employees = Employee.Helper.Find(filter).ToList();
            foreach (var employee in employees)
            {
                DataSensitizer.Execute(employee);
                Employee.Helper.Upsert(employee);
            }
        }
    }
}
