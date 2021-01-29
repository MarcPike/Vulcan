using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperEmployeeIncidents_Test
{
    [TestFixture]
    public class HelperEmployeeIncidentTests
    {
        private HelperEmployeeIncidents _helperEmployeeIncidents;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
            _helperEmployeeIncidents = new HelperEmployeeIncidents();
        }


        [Test]
        public void CopyIncidentDateFor2906ToDev()
        {
            EnvironmentSettings.HrsProduction();
            var incidentProd = EmployeeIncident.Helper.Find(x => x.IncidentId == 2906 && x.NearMissTypeCode.Code == "PD").FirstOrDefault();
            EnvironmentSettings.HrsQualityControl();
            var incidentQuality = EmployeeIncident.Helper.Find(x => x.IncidentId == 2906 && x.NearMissTypeCode.Code == "PD").FirstOrDefault();

            Assert.IsNotNull(incidentProd);
            Assert.IsNull(incidentQuality);

            EmployeeIncident.Helper.Upsert(incidentProd);

            //Assert.AreNotEqual(incidentProd.IncidentDate, incidentDev.IncidentDate);


        }

        [Test]
        public void GetEmployeeIncidentModel()
        {
            var helperEmployee = new HelperEmployee();
            var employeeProd = Employee.Helper.Find(x => x.PayrollId == "US1477").Single();


            //var model = helperEmployee.GetEmployeeModel(employee.Id.ToString());
            //Console.WriteLine($"PayrollId: {employee.PayrollId}");
            //Console.WriteLine($"Termination Date: {model.TerminationDate} As UTC: {model.TerminationDate?.ToUniversalTime()}");

            EnvironmentSettings.HrsDevelopment();

            var employeeDev = Employee.Helper.Find(x => x.PayrollId == "US1477").Single();

            if (employeeProd.TerminationDate != employeeDev.TerminationDate)
            {
                Console.WriteLine($"Changed PayrollId: {employeeDev.PayrollId} Term Date From: {employeeDev.TerminationDate} To: {employeeProd.TerminationDate}");
                employeeDev.TerminationDate = employeeProd.TerminationDate;
                Employee.Helper.Upsert(employeeDev);
            }


            //Console.WriteLine("");
            //employee = Employee.Helper.Find(x => x.PayrollId == "US1477").Single();
            //model = helperEmployee.GetEmployeeModel(employee.Id.ToString());
            //Console.WriteLine($"PayrollId: {employee.PayrollId}");
            //Console.WriteLine($"Termination Date: {model.TerminationDate} As UTC: {model.TerminationDate?.ToUniversalTime()}");


        }


        [Test]
        public void GetAllEmployeeIncidents()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var results = _helperEmployeeIncidents.GetAllEmployeeIncidents();

            sw.Stop();
            Console.WriteLine($"{results.Count} results took {sw.Elapsed}");
        }

        [Test]
        public void GetEmployeeIncidentGridRows()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var results = _helperEmployeeIncidents.GetEmployeeIncidentGridRows();

            sw.Stop();
            Console.WriteLine($"{results.Count} results took {sw.Elapsed}");
            foreach (var doc in results.Where(x=>x.Employee != null))
            {
                Console.WriteLine($"{doc.IncidentId} - Employee: {doc.Employee.FullName}");
            }
        }

        [Test]
        public void GetEmployeeIncidentsWithMedicalLeaveHoursAway()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var enc = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var results = EmployeeIncident.Helper.Find(x => x.MedicalLeaves.Any(h => h.HoursAway != null)).ToList();

            foreach (var doc in results.Where(x => x.Employee != null))
            {
                foreach (var medicalLeave in doc.MedicalLeaves.Where(x=>x.HoursAway != null))
                {
                    Console.WriteLine($"{doc.IncidentId} has HoursAway == {enc.Decrypt<decimal?>(medicalLeave.HoursAway)}");

                }
            }
        }

    }
}
