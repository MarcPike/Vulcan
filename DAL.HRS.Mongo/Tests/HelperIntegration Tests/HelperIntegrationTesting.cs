using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperIntegration_Tests
{
    [TestFixture]
    public class HelperIntegrationTesting
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void GetPropertyValue()
        {
            var property = PropertyValue.Helper.FindById("5e505a4c7b2f2613bcc998f4");
            Console.WriteLine(property?.Code);

            property = PropertyValue.Helper.FindById("5e551a6c095f5978fc4db714");
            Console.WriteLine(property?.Code);
        }

        [Test]
        public void UpdateEmployeesWithLogin()
        {
            var payrollIds = new List<string>()
            {
                "EMMAL03",
                "EME22",
                "NOR06",
                "A683007",
                "A633144",
                "A683009",
                "CA00090",
                "EM-99999",
                "S166088",
                "SG358",
                "LOFT-3",
                "LOFT-4",
                "B102095",
                "B102094",
                "A633140",
                "S166099",
                "A633139",
                "C085093",
                "A633141",
                "A633142",
                "B102093",
                "B102096",
                "A633143"
            };

            foreach (var payrollId in payrollIds)
            {
                var emp = Employee.Helper.Find(x => x.PayrollId == payrollId).FirstOrDefault();
                if (emp == null)
                {
                    Console.WriteLine($"Could not find an Employee with PayrollId == {payrollId}");
                    continue;
                }

                if (emp.LdapUser != null)
                {
                    Console.WriteLine($"Employee {emp.PayrollId} has an LdapUser and should have a NetworkId = {emp.LdapUser.NetworkId}");
                    continue;
                }

                emp.Login = emp.FirstName.Substring(0, 1) + emp.LastName.ToLower();
                Employee.Helper.Upsert(emp);
            }
        }

        [Test]
        public void FindBadPhoneNumber()
        {
            var employees = Employee.Helper.Find(x => x.PhoneNumbers.Any(p => p.PhoneType == null)).ToList();
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.PayrollId} {employee.FirstName} {employee.LastName}");
            }
        }

        [Test]
        public void GetKronosExport()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            var helper = new HelperIntegrations();
            var rows = helper.KronosExport("Howco");

            foreach (var kronosExportModel in rows)
            {
                Console.WriteLine($"");
            }


            sw.Stop();

            var active = rows.Count(x => x.IsActive);

            var inActive = rows.Count(x => !x.IsActive);
            var dataErrors = rows.Count(x => x.KronosPayRule != null && x.KronosPayRule.Contains("(Data Error)"));

            var missingKronosPayrule = rows.Count(x => x.KronosPayRule == null);

            Console.WriteLine($"Elapsed: {sw.Elapsed} Total rows: {rows.Count} Active: {active} InActive: {inActive} Data Errors: {dataErrors} Missing KronosPayRule: {missingKronosPayrule}" );

            //foreach (var kronosExportModel in rows.Where(x => x.KronosPayRule != null && x.KronosPayRule.Contains("(Data Error)")).ToList())
            //{
            //    Console.WriteLine($"KronosPayRule: {kronosExportModel.KronosPayRule.Replace(" (Data Error)","")} does not exist for Employee:{kronosExportModel.PayrollId} - {kronosExportModel.FirstName} {kronosExportModel.LastName}");
            //}

            //foreach (var kronosExportModel in rows.Where(x => x.KronosPayRule == null).ToList())
            //{
            //    Console.WriteLine($"Employee has no KronosPayRule :{kronosExportModel.PayrollId} - {kronosExportModel.FirstName} {kronosExportModel.LastName}");
            //}

        }

        [Test]
        public void GetHalogenExport()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var helper = new HelperIntegrations();
            var rows = helper.HalogenExport("Howco");

            foreach (var halogenExportModel in rows)
            {
                Console.WriteLine($"");
            }

            sw.Stop();

            
           Console.WriteLine($"Elapsed: {sw.Elapsed} Total rows: {rows.Count}");
           

        }

    }
}
