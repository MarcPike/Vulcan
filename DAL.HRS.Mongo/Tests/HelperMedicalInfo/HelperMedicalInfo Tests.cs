using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperMedicalInfo
{
    [TestFixture]
    public class HelperMedicalInfoTests
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void AddMissingMedicalInfo()
        {
            var filter = Employee.Helper.FilterBuilder.Where(x => x.MedicalInfo == null);
            var employeesWithNoMedicalInfo = Employee.Helper.Find(filter);
            foreach (var employee in employeesWithNoMedicalInfo)
            {
                employee.MedicalInfo = new EmployeeMedicalInfo();
                Employee.Helper.Upsert(employee);
            }
        }

        [Test]
        public void GetMedicalInfoGrid()
        {
            var filter = HrsUser.Helper.FilterBuilder.Where(x => x.LastName == "Reese" && x.FirstName == "Shannen");
            var hrsUser = HrsUser.Helper.Find(filter).LastOrDefault();

            var helperMedicalInfo = new Helpers.HelperMedicalInfo();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var result = helperMedicalInfo.GetMedicalInfoGrid(hrsUser);
            sw.Stop();
            Console.WriteLine($"{result.Count} rows found : Elapsed: {sw.Elapsed}");


        }

        [Test]
        public void FindLeaveHistory()
        {
            var employees = Employee.Helper.Find(x => x.MedicalInfo != null && x.MedicalInfo.LeaveHistory.Count > 0).Project(x=> new {x.FirstName, x.LastName, x.PayrollId}).ToList();
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.PayrollId}");
                
            }
        }

        [Test]
        public void FindMedicalExamDates()
        {
            var employees = Employee.Helper.Find(x => x.MedicalInfo != null && x.MedicalInfo.MedicalExams.Any(m=>m.ExamDate != null)).Project(x => new { x.FirstName, x.LastName, x.PayrollId }).ToList();
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.PayrollId}");

            }
        }

    }
}
