using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HRS.Web.Client.CSharp.Client;
using HRS.Web.Client.CSharp.Referenced_Classes;
using NUnit.Framework;

namespace HRS.Web.Client.CSharp.Test
{
    [TestFixture()]
    public class HrsDynamicFetcherExamples
    {

        [Test]
        public void GetHrsSecurityForUser()
        {
            var fetcher = new HrsDynamicFetcher(WebApiSource.QngProd);
            var testUser = "c8a432a6-20ed-483a-b4c2-fb26a6c0eba0"; // Lori
            //var testUser = "83bb4a6e-678c-4799-8f70-a8ec1db49050"; // Marc
            //var testUser = "b6801014-3270-4ed4-92a3-7f2d2b542a38"; // Nick Hartle
            //var testUser = "ForceErrorToTestException";

            Stopwatch sw = new Stopwatch();
            try
            {
                sw.Start();
                var hrsSecurity = fetcher.GetHrsSecurityForUser(testUser);
                sw.Stop();
                Console.WriteLine(new String('-', 30));
                Console.WriteLine($"Elapsed: {sw.Elapsed}");
                Console.WriteLine($"User Role: {hrsSecurity.Role.RoleType.Name}");
                Console.WriteLine(new String('-', 30));
                foreach (var module in hrsSecurity.Role.Modules)
                {
                    Console.WriteLine($"Module: {module.ModuleType.Name} View: {module.View}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }

        [Test]
        public void GetHseSecurityForUser()
        {
            var fetcher = new HrsDynamicFetcher(WebApiSource.Development);
            var testUser = "c8a432a6-20ed-483a-b4c2-fb26a6c0eba0"; // Lori

            Stopwatch sw = new Stopwatch();
            try
            {
                sw.Start();
                var hseSecurity = fetcher.GetHseSecurityForUser(testUser);
                sw.Stop();

                Console.WriteLine(new String('-', 30));
                Console.WriteLine($"Elapsed: {sw.Elapsed}");
                Console.WriteLine($"User Role: {hseSecurity.Role.RoleType.Name}");
                Console.WriteLine(new String('-', 30));
                foreach (var module in hseSecurity.Role.Modules)
                {
                    Console.WriteLine($"Module: {module.ModuleType.Name} View: {module.View}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        [Test]
        public void GetEmployeeList()
        {
            var fetcher = new HrsDynamicFetcher(WebApiSource.Development);

            Stopwatch sw = new Stopwatch();
            try
            {
                sw.Start();
                var employees = fetcher.GetEmployeeList();
                sw.Stop();

                Console.WriteLine(new String('-', 60));
                Console.WriteLine($"Rows found: {employees.Count} Elapsed: {sw.Elapsed}");
                Console.WriteLine(new String('-', 60));
                foreach (var emp in employees.Where(x => x.IsActive))
                {
                    Console.WriteLine($"{emp.LastName}, {emp.FirstName} is a {emp.JobTitle.Name} in CC: {emp.CostCenterCode.Code} Active: {emp.IsActive}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [Test]
        public void GetAllLocations()
        {
            var fetcher = new HrsDynamicFetcher(WebApiSource.Development);

            Stopwatch sw = new Stopwatch();
            try
            {
                sw.Start();
                var locations = fetcher.GetAllLocations();
                sw.Stop();

                Console.WriteLine(new String('-', 60));
                Console.WriteLine($"Rows found: {locations.Count} Elapsed: {sw.Elapsed}");
                Console.WriteLine(new String('-', 60));
                foreach (var location in locations)
                {
                    Console.WriteLine($"Office: {location.Office} Coid: {location.Coid} Branch: {location.Branch} Region: {location.Region} Entity: {location.Entity.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        /*
        Missing fields in this version:
        --------------------------------------------
        JobCategory
        Shift
        DisciplinaryActionType
        NatureOfViolation
        DateOfViolation
        DateOfAction
        --------------------------------------------
         */

        [Test]
        public void GetEmployeeDetailsForQng()
        {
            var fetcher = new HrsDynamicFetcher(WebApiSource.QngProd);

            Stopwatch sw = new Stopwatch();
                sw.Start();
                var testUser = "46e555a2-52ee-4e6d-ad75-dfa0e293d568"; // Denise
                //var testUser = "9ca0df8f-3854-4acc-801d-e49f79413ccb"; // Isidro
                var employees = fetcher.GetEmployeeDetailsForQng(DateTime.Now, testUser);
                sw.Stop();
                Console.WriteLine($"Elapsed {sw.Elapsed} Total rows {employees.Count}");
                Console.WriteLine($"Terminated: {employees.Count(x => x.IsActive == false)}");
                Console.WriteLine(employees.Count);
                foreach (var emp in employees.Where(x=>x.PayrollId == "A102019"))
                {
                    Console.WriteLine($"{emp.FullName} - PayrollId: {emp.PayrollId} TerminationDate: {emp.TerminationDate} KronosLaborLevelForLocation: {emp.KronosLaborLevelForLocation ?? "null"} - Shift: {emp.ShiftCode?.Code ?? "null"} BusinessUnit: {emp.BusinessUnitCode?.Code ?? "null"} CompanyNumber: {emp.CompanyNumberCode?.Code ?? "null"} {emp.IsActive}");
                }
        }

        [Test]
        public void GetCompensationForQng()
        {
            var fetcher = new HrsDynamicFetcher(WebApiSource.QngProd);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var testUser = "c8a432a6-20ed-483a-b4c2-fb26a6c0eba0"; // Lori
            var compansationValues = fetcher.GetCompensationForQng(DateTime.Now.Date, testUser, false);
            sw.Stop();

            Console.WriteLine($"Total rows {compansationValues.Count}");
            Console.WriteLine($"Terminated: {compansationValues.Count(x=>x.IsActive == false)}");

            var emp = compansationValues.FirstOrDefault(x => x.PayrollId == "CA00057");

            Console.WriteLine($"PayrollId: {emp.PayrollId} {emp.PayRateType.Code} {emp.PayRateAmount} {emp.BaseHours} {emp.AnnualSalary} {emp.MonthlyCompensation}");

            Console.WriteLine($"Elapsed: {sw.Elapsed} Rows: {compansationValues.Count}");
        }

        [Test]
        public void GetHseIncidentsByVarDataField()
        {
            var fetcher = new HrsDynamicFetcher(WebApiSource.QngProd);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var testUser = "46e555a2-52ee-4e6d-ad75-dfa0e293d568"; // Denise
            //var varDataField = "Body Part";
            //var varDataField = "Type of Contact";
            var varDataField = "Nature of Injury";
            //var varDataField = "Root Cause";
            //var varDataField = "Lack of Control";
            //var varDataField = "Personal Factors";
            //var varDataField = "Job Factors";
            var minDate = DateTime.Parse("12/31/2019");
            var maxDate = DateTime.Now.Date;
            var hseIncidentsByVarData = fetcher.GetEmployeeIncidentsByVarDataField(varDataField,minDate,maxDate, testUser);
            sw.Stop();

            Console.WriteLine($"Total rows {hseIncidentsByVarData.Count}");
            Console.WriteLine(string.Empty);

            foreach (var qngHseViewModel in hseIncidentsByVarData)
            {
                Console.WriteLine($"{varDataField}: {qngHseViewModel.VarDataFieldName} {varDataField} Comments: {qngHseViewModel.VarDataFieldComment} IncidentDate: {qngHseViewModel.IncidentDate} Incident Id: {qngHseViewModel.IncidentId}");
            }

        }

        [Test]
        public void GetEmployeeIncidents()
        {
            var fetcher = new HrsDynamicFetcher(WebApiSource.Development);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var testUser = "46e555a2-52ee-4e6d-ad75-dfa0e293d568"; // Denise
            var minDate = DateTime.Parse("1/1/2020");
            var maxDate = DateTime.Now.Date;
            var hseIncidentsByVarData = fetcher.GetEmployeeIncidents( minDate, maxDate, testUser);
            sw.Stop();

            Console.WriteLine($"Total rows {hseIncidentsByVarData.Count}");
            Console.WriteLine(string.Empty);

            foreach (var qngHseViewModel in hseIncidentsByVarData)
            {
                Console.WriteLine($"IncidentDate: {qngHseViewModel.IncidentDate} Incident Id: {qngHseViewModel.IncidentId} Medical Notes: {qngHseViewModel.MedicalNotes}");
            }

        }

        [Test]
        public void GetTrainingInfo()
        {
            var fetcher = new HrsDynamicFetcher(WebApiSource.QngProd);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var testUser = "46e555a2-52ee-4e6d-ad75-dfa0e293d568"; // Denise
            var minDate = DateTime.Parse("10/1/2020");
            var maxDate = DateTime.Now;
            var trainingInfo = fetcher.GetTrainingInfo(minDate, maxDate, testUser);
            sw.Stop();

            Console.WriteLine($"Total rows {trainingInfo.Count} Elapsed: {sw.Elapsed}");
            Console.WriteLine(string.Empty);

            foreach (var t in trainingInfo.Where(x=>x.TrainingHours > 0))
            {
                Console.WriteLine($"Name: {t.FullName} PayrollId: {t.PayrollId} CourseName: {t.CourseName} IsActive: {t.IsActive} Training Hours: {t.TrainingHours}");
            }

        }


        public string FormatPhoneNumbers(List<EmployeePhoneNumber> phoneNumbers)
        {
            StringBuilder sb = new StringBuilder();
            foreach (dynamic phoneNumber in phoneNumbers)
            {
                if ((phoneNumber.PhoneType != null) && (phoneNumber.PhoneNumber != null))
                {
                    sb.AppendLine($"({phoneNumber.PhoneType.Code}) {phoneNumber.PhoneNumber}");
                }
            }

            return sb.ToString();
        }

    }
}
