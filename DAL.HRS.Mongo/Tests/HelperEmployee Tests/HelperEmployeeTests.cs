using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.AuditTrails;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using DAL.Vulcan.Mongo.Base.Encryption;

using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;

namespace DAL.HRS.Mongo.Tests.HelperEmployee_Tests
{
    [TestFixture]
    public class HelperEmployeeTests
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void GetNewEmployeeTest()
        {
            var helper = new HelperEmployee();
            var newEmployee = helper.GetNewEmployeeModel();
            Console.WriteLine(ObjectDumper.Dump(newEmployee));

        }

        [Test]
        public void RebuildAuditTrailModels()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (var employeeAuditTrail in EmployeeAuditTrail.Helper.GetAll())
            {
                employeeAuditTrail.AuditTrail = null;
                EmployeeAuditTrail.Helper.Upsert(employeeAuditTrail);
            }

            EmployeeAuditTrail.CalculateRequired();
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");

        }

        [Test]
        public void EmployeeAuditTrailHistoryModelTest()
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var employeeAuditHistory = new EmployeeAuditTrailHistoryModel(DateTime.Parse("4/1/2020"), "EmployeeDetails", null);
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");
            foreach (var hist in employeeAuditHistory.AuditTrailHistory)
            {
                Console.WriteLine($"Employee: {hist.Employee.FullName} IsActive: {hist.IsActive} Module: {hist.Module} Field: {hist.FieldName} Old: {hist.OldValue} New: {hist.NewValue} ListName: {hist.ListName} ListAction: {hist.ListAction} List Value: {hist.ListValues}");
            }
        }


        [Test]
        public void TestDates()
        {
            var hrsUser = HrsUser.Helper.Find(x => x.LastName == "Pike").First();

            var helper = new HelperEmployee();
            var model = helper.GetEmployeeModel("5e4e88a3095f5970b8b67b7c");
            model.ModifiedByUser = hrsUser.AsHrsUserRef();

            model.ConfirmationDate = DateTime.Parse("5/29/2020");
            //model.ConfirmationDate = model.ConfirmationDate?.ToUniversalTime();
            model = helper.SaveEmployee(model);
            Console.WriteLine(model.ConfirmationDate);

            var employee = Employee.Helper.Find(x => x.PayrollId == model.PayrollId).FirstOrDefault();
            Assert.AreEqual(model.ConfirmationDate, employee.ConfirmationDate);

        }


        [Test]
        public void DeepDiveEmployeeAuditTrail()
        {
            var emp = Employee.Helper.Find(x => x.PayrollId == "ADD-04").FirstOrDefault();
            var audits = EmployeeAuditTrail.Helper.Find(x => x.Original.PayrollId == "ADD-04").ToList();

            foreach (var audit in audits)
            {
                var auditTrail = new EmployeeAuditTrailModel(audit);
                Console.WriteLine($"{audit.UpdatedBy.FirstName} modified {audit.UpdatedAt}");
                Console.WriteLine($"================================");
                foreach (var valueChange in auditTrail.EmployeeDetailsChanges.ValueChanges)
                {
                    Console.WriteLine($"{valueChange.FieldName} - Old: {valueChange.OldValue} New: {valueChange.NewValue}");
                }
            }

        }

        [Test]
        public void CreateEmployeeAuditTrailToSeeIfIdIsBeingCreated()
        {
            var hrsUser = HrsUser.Helper.Find(x => x.LastName == "Pike").FirstOrDefault().AsHrsUserRef();
            var employee = Employee.Helper.Find(x => x.PayrollId == "ADD-04").FirstOrDefault();
            var helperEmployee = new HelperEmployee();
            var model = helperEmployee.GetEmployeeModel(employee.Id.ToString());
            model.ModifiedByUser = hrsUser;
            helperEmployee.SaveEmployee(model);

            DeepDiveEmployeeAuditTrail();

        }

        [Test]
        public void DuplicateGovernmentIdCodeTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var enc = Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var filter = Employee.Helper.FilterBuilder.Empty;
            var project =
                Employee.Helper.ProjectionBuilder.Expression(x =>
                    new
                    {
                        PayrollId = x.PayrollId,
                        GovernmentId = enc.Decrypt<string>(x.GovernmentId),
                        FirstName = x.FirstName,
                        LastName = x.LastName
                    });

            var employeeGovernmentIds = Employee.Helper.FindWithProjection(filter, project).ToList();

            var emp = employeeGovernmentIds.FirstOrDefault(x => x.GovernmentId == "462-33-6300");
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

        [Test]
        public void FindBonusSchemePayFreqType()
        {
            var employees = Employee.Helper.Find(x => x.Compensation.BonusScheme.Any(b => b.PayFrequencyType != null))
                .ToList();
            foreach (var employee in employees)
            {
                foreach (var bonusScheme in employee.Compensation.BonusScheme.Where(x => x.PayFrequencyType != null).ToList())
                {
                    Console.WriteLine($"{employee.PayrollId} - Bonus Scheme - PayFrequencyType: {bonusScheme.PayFrequencyType}");
                }
            }
        }

        [Test]
        public void FindBonusSchemeTargetPercentOutOfWhack()
        {
            var employees = Employee.Helper.Find(x => x.Compensation.BonusScheme.Any(b => b.TargetPercentage > 1))
                .ToList();
            foreach (var employee in employees)
            {
                foreach (var bonusScheme in employee.Compensation.BonusScheme.Where(x => x.TargetPercentage > 1).ToList())
                {
                    Console.WriteLine($"{employee.PayrollId} - Bonus Scheme - Target%: {bonusScheme.TargetPercentage}");
                }
            }
        }

        [Test]
        public void FixBonusSchemeTargetPercentOutOfWhack()
        {
            var employees = Employee.Helper.Find(x => x.Compensation.BonusScheme.Any(b => b.TargetPercentage > 1))
                .ToList();
            foreach (var employee in employees)
            {
                foreach (var bonusScheme in employee.Compensation.BonusScheme.Where(x => x.TargetPercentage > 1).ToList())
                {
                    bonusScheme.TargetPercentage = bonusScheme.TargetPercentage / 100;
                    Console.WriteLine($"{employee.PayrollId} - Bonus Scheme - New Target%: {bonusScheme.TargetPercentage}");
                }

                Employee.Helper.Upsert(employee);
            }
        }

        //[Test]
        //public void Shannen()
        //{var Shannen = Employee.Helper.Find(x => x.FirstName == "Shannen").FirstOrDefault();
        //    Console.Write(Employee);



        [Test]
        public void TestEmployeeAudit()
        {
            var emp = Employee.Helper.Find(x => x.PayrollId == "ADD-04").FirstOrDefault();
            var helper = new HelperEmployee();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var auditTrail = helper.GetAuditTrailForEmployee(emp.Id.ToString());
            sw.Stop();
            Console.WriteLine($"Elapsed time to execute: {sw.Elapsed}");
            foreach (var employeeAuditTrailModel in auditTrail)
            {
                Console.WriteLine($"{employeeAuditTrailModel.UpdatedBy.FullName} updated @ {employeeAuditTrailModel.UpdatedAt}");
                if (!employeeAuditTrailModel.AnyChanges())
                {
                    Console.WriteLine(@"       No Changes found");
                    continue;
                }

                if (employeeAuditTrailModel.EmployeeDetailsChanges.AnyChanges())
                {
                    Console.WriteLine("       EmployeeDetails was changed");
                    foreach (var valueChange in employeeAuditTrailModel.EmployeeDetailsChanges.ValueChanges)
                    {
                        Console.WriteLine($"              {valueChange.FieldName} was equal to [{valueChange.OldValue}] and was changed to [{valueChange.NewValue}]");
                    }

                    foreach (var listChange in employeeAuditTrailModel.EmployeeDetailsChanges.ListChanges)
                    {
                        Console.WriteLine($"              {listChange.ListName} list value {listChange.Action}");
                        Console.WriteLine($"                     {listChange.ListValues}");
                    }
                }
                if (employeeAuditTrailModel.BenefitsChanges.AnyChanges())
                {
                    Console.WriteLine("       Benefits was changed");
                    foreach (var valueChange in employeeAuditTrailModel.BenefitsChanges.ValueChanges)
                    {
                        Console.WriteLine($"              {valueChange.FieldName} was equal to [{valueChange.OldValue}] and was changed to [{valueChange.NewValue}]");
                    }

                    foreach (var listChange in employeeAuditTrailModel.BenefitsChanges.ListChanges)
                    {
                        Console.WriteLine($"              {listChange.ListName} list value {listChange.Action}");
                        Console.WriteLine($"                     {listChange.ListValues}");
                    }
                }
                if (employeeAuditTrailModel.CompensationChanges.AnyChanges())
                {
                    Console.WriteLine("       Compensation was changed");
                    foreach (var valueChange in employeeAuditTrailModel.CompensationChanges.ValueChanges)
                    {
                        Console.WriteLine($"              {valueChange.FieldName} was equal to [{valueChange.OldValue}] and was changed to [{valueChange.NewValue}]");
                    }

                    foreach (var listChange in employeeAuditTrailModel.CompensationChanges.ListChanges)
                    {
                        Console.WriteLine($"              {listChange.ListName} list value {listChange.Action}");
                        Console.WriteLine($"                     {listChange.ListValues}");
                    }
                }
                if (employeeAuditTrailModel.DisciplineChanges.AnyChanges())
                {
                    Console.WriteLine("       Discipline was changed");
                    foreach (var valueChange in employeeAuditTrailModel.DisciplineChanges.ValueChanges)
                    {
                        Console.WriteLine($"              {valueChange.FieldName} was equal to [{valueChange.OldValue}] and was changed to [{valueChange.NewValue}]");
                    }

                    foreach (var listChange in employeeAuditTrailModel.DisciplineChanges.ListChanges)
                    {
                        Console.WriteLine($"              {listChange.ListName} list value {listChange.Action}");
                        Console.WriteLine($"                     {listChange.ListValues}");
                    }
                }
                if (employeeAuditTrailModel.PerformanceChanges.AnyChanges())
                {
                    Console.WriteLine("       Performance was changed");
                    foreach (var valueChange in employeeAuditTrailModel.PerformanceChanges.ValueChanges)
                    {
                        Console.WriteLine($"              {valueChange.FieldName} was equal to [{valueChange.OldValue}] and was changed to [{valueChange.NewValue}]");
                    }

                    foreach (var listChange in employeeAuditTrailModel.PerformanceChanges.ListChanges)
                    {
                        Console.WriteLine($"              {listChange.ListName} list value {listChange.Action}");
                        Console.WriteLine($"                     {listChange.ListValues}");
                    }
                }
            }

        }

        [Test]
        public void TestAuditClone()
        {
            var emp = Employee.Helper.Find(x => x.LastName == "Pike").First();
            var hrsUser = HrsUser.Helper.Find(x => x.Employee.LastName == "Pike").First();

            var audit = new EmployeeAuditTrail(emp, hrsUser.AsHrsUserRef());


            Console.WriteLine($"Employee originally has {emp.PhoneNumbers.Count} phone numbers");
            Console.WriteLine($"Audit Trail original has {audit.Original.PhoneNumbers.Count} phone numbers");

            Console.WriteLine();
            Console.WriteLine("Adding a phone number to the Employee, Audit.Original should not change...");
            emp.PhoneNumbers.Add(new EmployeePhoneNumber("Mobile", "111-222-3333", "United States"));

            Console.WriteLine($"Employee now has {emp.PhoneNumbers.Count} phone numbers");
            Console.WriteLine($"Audit Trail original now has {audit.Original.PhoneNumbers.Count} phone numbers");

            Assert.AreNotEqual(emp.PhoneNumbers.Count, audit.Original.PhoneNumbers.Count);



        }

        [Test]
        public void FindShannen()
        {
            var shannen = Employee.Helper.Find(x => x.FirstName == "Shannen").FirstOrDefault();
            Console.WriteLine(shannen.PayrollId);
        }


        [Test]
        public void GetDavidMeyerDirectReports()
        {
            var helperEmployee = new HelperEmployee();
            var emp = Employee.Helper.Find(x => x.PayrollId == "000155").FirstOrDefault();

            var directReports = helperEmployee.GetEmployeeDirectReports(emp.Id.ToString());
            foreach (var employeeModel in directReports)
            {
                Console.WriteLine($"PayrollId: {employeeModel.PayrollId} Name: {employeeModel.FirstName} {employeeModel.LastName}");
            }

        }



        [Test]
        public void DoesToLowerWork()
        {
            var empWithThisLogin = Employee.Helper.Find(x => x.LdapUser.NetworkId.ToLower() == "mpike").FirstOrDefault();
            Console.WriteLine(empWithThisLogin.PayrollId);
        }

        [Test]
        public void FixHrsUserEmployeeProblem()
        {
            var hrsUsers = HrsUser.Helper.GetAll();
            foreach (var hrsUser in hrsUsers)
            {
                var employee = Employee.Helper.FindById(hrsUser.Employee.Id);
                if (employee == null)
                {
                    var payrollFilter =
                        Employee.Helper.FilterBuilder.Where(x => x.PayrollId == hrsUser.Employee.PayrollId);
                    var correctEmployee = Employee.Helper.Find(payrollFilter).Single();
                    hrsUser.Employee = correctEmployee.AsEmployeeRef();
                    HrsUser.Helper.Upsert(hrsUser);
                    Console.WriteLine($"{hrsUser.FullName} corrected");

                }
            }
        }


        [Test]
        public void TestDirectReports()
        {
            var filter = Employee.Helper.FilterBuilder.Where(x => x.DirectReports.Any() && x.Entity.Name == "Howco");
            var project = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.DirectReports, x.FirstName, x.LastName, x.PayrollId });
            var employeesWithDirectReports = Employee.Helper.FindWithProjection(filter, project).ToList();
            var corrections = 0;
            var cannotFix = 0;
            foreach (var employee in employeesWithDirectReports)
            {
                foreach (var directReport in employee.DirectReports)
                {
                    var empFound = Employee.Helper.FindById(directReport.Id);
                    if (empFound == null)
                    {
                        //Console.WriteLine($"Employee {employee.FirstName} {employee.LastName} is invalid DirectReport {directReport.FullName}");

                        var filterPayrollId =
                            Employee.Helper.FilterBuilder.Where(x => x.PayrollId == directReport.PayrollId);
                        var correctDirectReport = Employee.Helper.Find(filterPayrollId).FirstOrDefault();
                        if (correctDirectReport != null)
                        {
                            FixEmployeeDirectReport(employee.Id, directReport, correctDirectReport);
                        }
                        else
                        {
                            Console.WriteLine($"Employee {employee.FirstName} {employee.LastName} cannot find DirectReport {directReport.FullName} PayrollId: {directReport.PayrollId}");
                            ++cannotFix;
                        }


                    }
                }

            }
            Console.WriteLine($"Corrections I can make: {corrections} Cannot Fix: {cannotFix}");
        }

        private void FixEmployeeDirectReport(ObjectId id, EmployeeRef badDirectReport, Employee correctDirectReport)
        {
            var employee = Employee.Helper.FindById(id);

            var directReport = employee.DirectReports.Single(x => x.Id == badDirectReport.Id);
            var indexOf = employee.DirectReports.IndexOf(directReport);
            employee.DirectReports[indexOf] = correctDirectReport.AsEmployeeRef();
            Employee.Helper.Upsert(employee);

        }


        [Test]
        public void TestJessy()
        {

            var filter = HrsUser.Helper.FilterBuilder.Where(x => x.LastName == "Tan" && x.FirstName == "Jessy");
            var hrsUser = HrsUser.Helper.Find(filter).First();

            var employee = hrsUser.Employee.AsEmployee();

            if (employee == null)
            {
                employee = Employee.Helper.FindById(hrsUser.Employee.Id);
            }


            var payRollRegion = hrsUser.Employee.AsEmployee().PayrollRegion;
            Console.WriteLine(payRollRegion);
        }

        [Test]
        public void FindEmployeesWithThisLastName()
        {
            var dups = Employee.Helper.Find(x => x.LastName == "Reese" && x.FirstName == "Shannen").ToList();
            foreach (var employee in dups)
            {
                Console.WriteLine($"{employee.PayrollId} - {employee.FirstName} {employee.LastName}");
            }
        }

        [Test]
        public void GetEmployeeDetailsGrid()
        {
            var helper = new HelperEmployee();


            var hrsUser = HrsUser.Helper.Find(x => x.Employee.PayrollId == "000995").First();

            var results = helper.GetEmployeeDetailsGrid(hrsUser.UserId);

        }

        [Test]
        public void GetEmployeeModel()
        {
            var helper = new HelperEmployee();


            //var emp = Employee.Helper.Find(x => x.PayrollId == "ODD-02").FirstOrDefault();
            var emp = Employee.Helper.Find(x => x.PayrollId == "US1463").FirstOrDefault();
            var model = helper.GetEmployeeModel(emp.Id.ToString());

            Console.WriteLine(model.TerminationDate);
            //foreach (var directReport in model.DirectReports)
            //{
            //    Console.WriteLine($"PayrollId: {directReport.PayrollId} - First: {directReport.FirstName} Last: {directReport.LastName} ");
            //}
        }

        [Test]
        public void FindDuplicateGovId()
        {

            Vulcan.Mongo.Base.Encryption.Encryption encryption = Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;
            var govId = "111-11-0245";

            var filter = Employee.Helper.FilterBuilder.Empty;
            var project =
                Employee.Helper.ProjectionBuilder.Expression(x =>
                    new
                    {
                        Id = x.Id.ToString(),
                        PayrollId = x.PayrollId,
                        GovernmentId = encryption.Decrypt<string>(x.GovernmentId),
                        FirstName = x.FirstName,
                        LastName = x.LastName
                    });

            var employeeGovernmentIds = Employee.Helper.FindWithProjection(filter, project).ToList();
            var duplicateFound = employeeGovernmentIds.Where(x => x.GovernmentId == govId).ToList();
            foreach (var dup in duplicateFound)
            {
                Console.WriteLine($"{dup.Id} - {dup.PayrollId}");
            }

        }


        [Test]
        public void ForceSaveEveryEmployeeToFixIsActive()
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();

            foreach (var employee in queryHelper.GetAll())
            {
                queryHelper.Upsert(employee);
            }
        }

        [Test]
        public void RemovePlusSymbolFromAllPhoneNumbers()
        {
            var filter = Employee.Helper.FilterBuilder.Where(x => x.PhoneNumbers.Any());
            var project = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.PhoneNumbers });
            var values = Employee.Helper.FindWithProjection(filter, project).ToList();
            foreach (var value in values)
            {
                foreach (var employeePhoneNumber in value.PhoneNumbers.Where(x => x.PhoneNumber.Contains("+")))
                {
                    SetPhoneNumber(value.Id, employeePhoneNumber.Id, employeePhoneNumber.PhoneNumber.Replace("+", ""));
                }
            }

        }

        [Test]
        public void AddPlusSymbolToAllPhoneNumbers()
        {
            var filter = Employee.Helper.FilterBuilder.Where(x => x.PhoneNumbers.Any());
            var project = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.PhoneNumbers });
            var values = Employee.Helper.FindWithProjection(filter, project).ToList();
            foreach (var value in values)
            {
                foreach (var employeePhoneNumber in value.PhoneNumbers.Where(x => !x.PhoneNumber.Contains("+")))
                {
                    SetPhoneNumber(value.Id, employeePhoneNumber.Id, "+" + employeePhoneNumber.PhoneNumber);
                }
            }

        }


        private void SetPhoneNumber(ObjectId employeeId, Guid phoneNumberId, string newValue)
        {
            var emp = Employee.Helper.FindById(employeeId);
            var phoneNumberToChange = emp.PhoneNumbers.First(x => x.Id == phoneNumberId);
            phoneNumberToChange.PhoneNumber = newValue;
            Employee.Helper.Upsert(emp);
        }

        [Test]
        public void RemovePlusSymbolFromAllEmergencyContacts()
        {
            var filter = Employee.Helper.FilterBuilder.Where(x => x.EmergencyContacts.Any());
            var project = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.EmergencyContacts });
            var values = Employee.Helper.FindWithProjection(filter, project).ToList();
            foreach (var value in values)
            {
                foreach (var emergencyContact in value.EmergencyContacts.Where(x => x.PhoneNumber.Contains("+")))
                {
                    SetEmergencyContactPhoneNumber(value.Id, emergencyContact.Name, emergencyContact.PhoneNumber.Replace("+", ""));
                }
            }

        }

        [Test]
        public void AddPlusSymbolToAllEmergencyContacts()
        {
            var filter = Employee.Helper.FilterBuilder.Where(x => x.EmergencyContacts.Any());
            var project = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.EmergencyContacts });
            var values = Employee.Helper.FindWithProjection(filter, project).ToList();
            foreach (var value in values)
            {
                foreach (var emergencyContact in value.EmergencyContacts.Where(x => !x.PhoneNumber.Contains("+")))
                {
                    SetEmergencyContactPhoneNumber(value.Id, emergencyContact.Name, "+" + emergencyContact.PhoneNumber);
                }
            }

        }

        private void SetEmergencyContactPhoneNumber(ObjectId employeeId, string name, string newValue)
        {
            var emp = Employee.Helper.FindById(employeeId);
            var emergencyContactToChange = emp.EmergencyContacts.First(x => x.Name == name);
            emergencyContactToChange.PhoneNumber = newValue;
            Employee.Helper.Upsert(emp);
        }

        [Test]
        public void GetEmployeeTest()
        {
            var employeeFilter = Employee.Helper.FilterBuilder.Where(x => x.LastName == "Fraser");
            var employee = Employee.Helper.Find(employeeFilter).FirstOrDefault();

            var helperEmployee = new HelperEmployee();
            var model = helperEmployee.GetEmployeeModel(employee.Id.ToString());
            Console.WriteLine(ObjectDumper.Dump(model.WorkEmailAddress));
            Console.WriteLine(ObjectDumper.Dump(model.PersonalEmailAddress));
        }
    }
}
