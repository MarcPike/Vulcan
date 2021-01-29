using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperExternalApi_Tests
{
    [TestFixture]
    public class HelperExternalApiTests
    {
        private HelperExternalApi _helperExternalApi;
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
            _helperExternalApi = new HelperExternalApi();
        }

        [Test]
        public void GetHrsSecurityForUser()
        {
            var activeDirectoryId = "9daf149d-942b-4aa4-8448-9e9c75f9f11a";
            var result = _helperExternalApi.GetHrsSecurityForUser(activeDirectoryId);
        }


        [Test]
        public void GetEmployeeList()
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var results = _helperExternalApi.GetEmployeeList();
            sw.Stop();
            Console.WriteLine($"Found: {results.Count} Elapsed: {sw.Elapsed}");
            foreach (var employeeDetailsGridModel in results.Where(x=>x.PayrollId == "B10049"))
            {
                Console.WriteLine($"{employeeDetailsGridModel.LastName}  IsActive: {employeeDetailsGridModel.IsActive} Loc: {employeeDetailsGridModel.Location.Office}");
            }
        }


        [Test]
        public void GetEmployeeBasicDateForQng()
        {

            var emp = Employee.Helper.Find(x => x.PayrollId == "A102019").FirstOrDefault();

            var empModel = new EmployeeModel(emp);

            var termDateNotUtc = empModel.TerminationDate;
            var frontEnd = empModel.TerminationDate?.ToUniversalTime();


            Stopwatch sw = new Stopwatch();
            //var testUser = "7e44d359-f31c-41e0-b86a-660b88d6810a"; // J Michael
            var testUser = "9ca0df8f-3854-4acc-801d-e49f79413ccb"; // Isidro
            //var testUser = "46e555a2-52ee-4e6d-ad75-dfa0e293d568"; // Denise
            sw.Start();
            var results = _helperExternalApi.GetEmployeeDetailsForQng(DateTime.Now.Date, testUser);
            sw.Stop();
            Console.WriteLine($"Total rows {results.Count}");
            Console.WriteLine($"Terminated: {results.Count(x => x.IsActive == false)}");


            Console.WriteLine($"Found: {results.Count} Elapsed: {sw.Elapsed}");
            foreach (var qngEmployeeBasicDataModel in results.Where(x=>x.PayrollId == "B10049"))
            {
                var terminationDate = qngEmployeeBasicDataModel.TerminationDate;
                var convertedDate = terminationDate?.ToUniversalTime();
                Console.WriteLine($"{qngEmployeeBasicDataModel.FullName} TerminationDate: {qngEmployeeBasicDataModel.TerminationDate}");
            }
        }

        [Test]
        public void RawTestTerminationDate()
        {
            var dateOf = DateTime.Now.Date;
            var filter = Employee.Helper.FilterBuilder.Where(x => x.PayrollId == "A102019");
            var project = Employee.Helper.ProjectionBuilder.
                Expression(x => new QngEmployeeBasicDataModel(
                    dateOf,
                    x.Id,
                    x.PayrollId,
                    x.GovernmentId,
                    x.FirstName,
                    x.LastName,
                    x.MiddleName,
                    x.PreferredName,
                    x.LdapUser,
                    x.Birthday,
                    x.JobTitle,
                    x.EEOCode,
                    x.EthnicityCode,
                    x.GenderCode,
                    x.Location,
                    x.Manager,
                    x.CountryCode,
                    x.CountryOfOriginCode,
                    x.Address1, x.Address2, x.Address3,
                    x.City, x.State, x.Country, x.PostalCode,
                    x.PhoneNumbers ?? new List<EmployeePhoneNumber>(),
                    x.EmailAddresses ?? new List<EmployeeEmailAddress>(),
                    x.EmergencyContacts ?? new List<EmergencyContact>(),
                    x.Status1Code, x.Status2Code, x.PayrollRegion, x.BusinessRegionCode,
                    x.PriorServiceDate, x.OriginalHireDate, x.LastRehireDate,
                    x.WorkAreaCode,
                    x.Terminations ?? new List<Termination>(),
                    x.TerminationDate, x.TerminationCode, x.TerminationExplanation,
                    x.RehireStatusCode, x.KronosLaborLevelCode, x.KronosDepartmentCode, x.NationalityCode,
                    x.Discipline.LastOrDefault(),
                    x.CostCenterCode, x.ShiftCode, x.Login, x.ExternalLoginId, x.CompanyNumberCode, x.BusinessUnitCode, x.ConfirmationDate
                ));

            var result = Employee.Helper.FindWithProjection(filter, project);
        }

        [Test]
        public void MakeSureWeRemovedNotHiredYet()
        {

            Stopwatch sw = new Stopwatch();
            //var testUser = "7e44d359-f31c-41e0-b86a-660b88d6810a"; // J Michael
            var testUser = "46e555a2-52ee-4e6d-ad75-dfa0e293d568"; // -Lori
            sw.Start();
            var results = _helperExternalApi.GetEmployeeDetailsForQng(DateTime.Parse("2/22/2020"), testUser);
            sw.Stop();
            Console.WriteLine($"Found: {results.Count} Elapsed: {sw.Elapsed}");
            foreach (var qngEmployeeBasicDataModel in results.Where(x=>x.PayrollId == "SG313" || x.PayrollId == "SG357"))
            {
                Console.WriteLine($"{qngEmployeeBasicDataModel.FullName} OHD: {qngEmployeeBasicDataModel.OriginalHireDate?.ToShortDateString()} LHD: {qngEmployeeBasicDataModel.LastRehireDate?.ToShortDateString()} TD: {qngEmployeeBasicDataModel.TerminationDate} IsActive: {qngEmployeeBasicDataModel.IsActive} Loc: {qngEmployeeBasicDataModel.Location.Office}");
            }
        }

        [Test]
        public void GetEmployeeBasicDataForQngSingleEmployee()
        {
            var dateOf = DateTime.Parse("2/22/2020");

            var filter = Employee.Helper.FilterBuilder.Where(x => x.PayrollId == "SG313");
            var project = Employee.Helper.ProjectionBuilder.
                Expression(x => new QngEmployeeBasicDataModel(
                    dateOf,
                    x.Id,
                    x.PayrollId,
                    x.GovernmentId,
                    x.FirstName,
                    x.LastName,
                    x.MiddleName,
                    x.PreferredName,
                    x.LdapUser,
                    x.Birthday,
                    x.JobTitle,
                    x.EEOCode,
                    x.EthnicityCode,
                    x.GenderCode,
                    x.Location,
                    x.Manager,
                    x.CountryCode,
                    x.CountryOfOriginCode,
                    x.Address1, x.Address2, x.Address3,
                    x.City, x.State, x.Country, x.PostalCode,
                    x.PhoneNumbers ?? new List<EmployeePhoneNumber>(),
                    x.EmailAddresses ?? new List<EmployeeEmailAddress>(),
                    x.EmergencyContacts ?? new List<EmergencyContact>(),
                    x.Status1Code, x.Status2Code, x.PayrollRegion, x.BusinessRegionCode,
                    x.PriorServiceDate, x.OriginalHireDate, x.LastRehireDate,
                    x.WorkAreaCode,
                    x.Terminations ?? new List<Termination>(),
                    x.TerminationDate, x.TerminationCode, x.TerminationExplanation,
                    x.RehireStatusCode, x.KronosLaborLevelCode, x.KronosDepartmentCode, x.NationalityCode,
                    x.Discipline.LastOrDefault(),
                    x.CostCenterCode, x.ShiftCode, x.Login, x.ExternalLoginId, x.CompanyNumberCode, x.BusinessUnitCode, x.ConfirmationDate
                ));

            var result = Employee.Helper.FindWithProjection(filter, project).FirstOrDefault();
            Assert.IsNotNull(result.IsActive);

        }

        [Test]
        public void GetCompensationForQng()
        {

            Stopwatch sw = new Stopwatch();
            //var testUser = "7e44d359-f31c-41e0-b86a-660b88d6810a"; // J Michael
            var testUser = "9ca0df8f-3854-4acc-801d-e49f79413ccb"; // Isidro
            //var testUser = "46e555a2-52ee-4e6d-ad75-dfa0e293d568"; // Denise
            //var testUser = "c8a432a6-20ed-483a-b4c2-fb26a6c0eba0"; // Lori
            sw.Start();
            var results = _helperExternalApi.GetCompensationForQng(DateTime.Now.Date, testUser);
            sw.Stop();

            foreach (var qngCompensationModel in results.Where(x=> x.PayrollId == "A102019"))
            {
                Console.WriteLine($"PayrollId: {qngCompensationModel.PayrollId} Termination: {qngCompensationModel.TerminationDate} WorkAreaCode: {qngCompensationModel.WorkAreaCode?.Code}");
            }

            //var emp = results.FirstOrDefault(x => x.PayrollId == "A053013");

            //Console.WriteLine($"PayrollId: {emp.PayrollId} LastBonusType: {emp.LastBonusType?.Code} PayRateAmount: {emp.PayRateAmount} {emp.BaseHours} {emp.AnnualSalary} {emp.MonthlyCompensation}");
            //Console.WriteLine($"Elapsed: {sw.Elapsed} Rows: {results.Count}");

        }

        [Test]
        public void GetTrainingInfoForQng()
        {

            Stopwatch sw = new Stopwatch();
            //var testUser = "7e44d359-f31c-41e0-b86a-660b88d6810a"; // J Michael
            var testUser = "46e555a2-52ee-4e6d-ad75-dfa0e293d568"; // Denise
            //var testUser = "c8a432a6-20ed-483a-b4c2-fb26a6c0eba0"; // Lori
            sw.Start();
            var results = _helperExternalApi.GetTrainingInfo(DateTime.Now.AddDays(-180),DateTime.Now,   testUser);
            sw.Stop();
            Console.WriteLine($"Rows: {results.Count} Elapsed: {sw.Elapsed}");
            foreach (var qngTrainingInfoModel in results.Where(x=>x.TrainingHours > 0))
            {
                Console.WriteLine($"PayrollId: {qngTrainingInfoModel.PayrollId} Course: {qngTrainingInfoModel.CourseName} TrainingHours: {qngTrainingInfoModel.TrainingHours} Start: {qngTrainingInfoModel.StartDate} End: {qngTrainingInfoModel.EndDate}");
            }

            //var emp = results.FirstOrDefault(x => x.PayrollId == "A053013");

            //Console.WriteLine($"PayrollId: {emp.PayrollId} LastBonusType: {emp.LastBonusType?.Code} PayRateAmount: {emp.PayRateAmount} {emp.BaseHours} {emp.AnnualSalary} {emp.MonthlyCompensation}");
            //Console.WriteLine($"Elapsed: {sw.Elapsed} Rows: {results.Count}");

        }

        //[Test]
        //public void GetHseViewModelBackendTestOne()
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    var results = _helperExternalApi.GetEmployeeIncidents(DateTime.Parse("7/29/2020"), DateTime.Parse("7/30/2020"),
        //        "b660f40c-fa67-46b3-bf0a-0ec790a38fd2");
        //    sw.Stop();

        //    //var severityTypes = new List<string>() { "First Aid", "Lost Time", "Medical Treatment Only" };

        //    //var filterResults = results.Where(x => severityTypes.Any(s => s == x.SeverityType)).ToList();

        //    foreach (var r in results)
        //    {
        //        Console.WriteLine($"{r}");
        //    }

        //    Console.WriteLine($"{results.Count} rows - Elapsed: {sw.Elapsed}");

        //}


        [Test]
        public void GetHseViewModelBackendTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var results = QngEmployeeIncidentVarDataModel.GetValuesFor("Root Cause", DateTime.Parse("7/29/2015"), DateTime.Now.Date);
            sw.Stop();

            //var severityTypes = new List<string>() { "First Aid", "Lost Time", "Medical Treatment Only" };

            //var filterResults = results.Where(x => severityTypes.Any(s => s == x.SeverityType)).ToList();

            var rootCauses = results.Select(x => x.VarDataFieldName).Distinct().ToList();
            foreach (var r in rootCauses)
            {
                Console.WriteLine($"{r}");
            }

            Console.WriteLine($"{results.Count} rows - Elapsed: {sw.Elapsed}");

        }

        [Test]
        public void GetHseViewModelBackendTestOne()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var results = _helperExternalApi.GetEmployeeIncidents(DateTime.Parse("8/1/2020"), DateTime.Parse("8/30/2020"),
                "b660f40c-fa67-46b3-bf0a-0ec790a38fd2");
            sw.Stop();

            //var severityTypes = new List<string>() { "First Aid", "Lost Time", "Medical Treatment Only" };

            //var filterResults = results.Where(x => severityTypes.Any(s => s == x.SeverityType)).ToList();

            foreach (var r in results)
            {
                Console.WriteLine($"{r}");
            }

            Console.WriteLine($"{results.Count} rows - Elapsed: {sw.Elapsed}");

        }

        [Test]
        public void GetHseViewModelNatureOfInjury()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var results = QngEmployeeIncidentVarDataModel.GetValuesFor("Nature of Injury", DateTime.Parse("1/1/2015"), DateTime.Now.Date);
            sw.Stop();

            //var severityTypes = new List<string>() { "First Aid", "Lost Time", "Medical Treatment Only" };

            //var filterResults = results.Where(x => severityTypes.Any(s => s == x.SeverityType)).ToList();

            var resultsFor2802 = results.Where(x => x.IncidentId == 2802).ToList();
            foreach (var r in resultsFor2802)
            {
                Console.WriteLine($"{r.IncidentId} {r.VarDataFieldName} - {r.VarDataFieldComment}");
            }

            var resultsFor2803 = results.Where(x => x.IncidentId == 2803).ToList();
            foreach (var r in resultsFor2803)
            {
                Console.WriteLine($"{r.IncidentId} {r.VarDataFieldName} - {r.VarDataFieldComment}");
            }

            var resultsFor2784 = results.Where(x => x.IncidentId == 2784).ToList();
            foreach (var r in resultsFor2784)
            {
                Console.WriteLine($"{r.IncidentId} {r.VarDataFieldName} - {r.VarDataFieldComment}");
            }

            Console.WriteLine($"{results.Count} rows - Elapsed: {sw.Elapsed}");

        }


    }
}
