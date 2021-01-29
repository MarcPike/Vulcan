using System;
using System.Linq;
using System.Text.RegularExpressions;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Fixes
{
   [TestFixture]
    public class UpdatePhoneNumbers
    {
        private MongoRawQueryHelper<Employee> Helper;
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
            Helper = new MongoRawQueryHelper<Employee>();
        }



        [Test]
        public void RemoveSpecialCharsFromPhoneNumber()
        {
            var filter = Helper.FilterBuilder.Where(x => x.PhoneNumbers.Any(p => p.PhoneNumber.Contains("+") || p.PhoneNumber.Contains("-") || p.PhoneNumber.Contains(" ")));
            
            //var filter = Helper.FilterBuilder.Where(x => x.PhoneNumbers.Any(p => p.PhoneNumber.Contains(" ")));
            var employeeProject = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.PhoneNumbers });
            var empPhoneNumbers = Employee.Helper.FindWithProjection(filter, employeeProject).ToList();


            foreach (var num in empPhoneNumbers)
            {
                var empId = num.Id;
                var employeeFindFilter = Employee.Helper.FilterBuilder.Where(x => x.Id == empId);
                var emp = Employee.Helper.Find(employeeFindFilter).FirstOrDefault();

                foreach (var item in emp.PhoneNumbers)
                {
                    item.PhoneNumber = item.PhoneNumber.Replace("+", "").Replace("-", "").Replace(" ", "");;
                    //item.PhoneNumber = item.PhoneNumber.Replace(" ", "");

                }

                Employee.Helper.Upsert(emp);
            }


        }
    }
}
