using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperCompensation
{
    [TestFixture]
    class HelperCompensationTests
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsQualityControl();
        }

        [Test]
        public void GetCompensationModel()
        {
            var helperCompensation = new Helpers.HelperCompensation();
            var emp = Employee.Helper.Find(x => x.PayrollId == "CA00057").FirstOrDefault();
            var compModel = helperCompensation.GetCompensationForEmployee(emp.Id.ToString());

            Console.WriteLine(compModel.AnnualSalaryWithAllowances);
            Console.WriteLine(compModel.MonthlyCompensation);
            Console.WriteLine(compModel.AnnualSalary);
        }
    }
}
