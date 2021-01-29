using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.MedicalExams;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Models;
using DAL.HRS.SqlServer;
using DAL.Vulcan.Mongo.Base.Queries;
using DocumentFormat.OpenXml.Wordprocessing;
using MongoDB.Bson;
using MongoDB.Driver;
using MedicalExam = DAL.HRS.SqlServer.Model.MedicalExam;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperMedicalInfo : IHelperMedicalInfo
    {

        public HelperMedicalInfo()
        {
        }
        public List<MedicalInfoGridModel> GetMedicalInfoGrid(HrsUser hrsUser)
        {
            var gridRows = new List<MedicalInfoGridModel>();

            
            var helperEmployee = new HelperEmployee();

            var queryHelper = Employee.Helper;
            var moduleName = "Employee Medical Info";
            var role = SecurityRoleHelper.GetHrsSecurityRole(hrsUser);
            var module = SecurityRoleHelper.GetSecurityModule(role, moduleName);

            var employee = hrsUser.Employee.AsEmployee();

            if (!module.View) throw new Exception($"User does not have View rights for [{moduleName}]");

            FilterDefinition<Employee> filter = Employee.Helper.FilterBuilder.Empty;
            
            if (role.DirectReportsOnly)
            {
                filter = EmployeeDirectReportsFilterGenerator.GetDirectReportsOnlyFilter(queryHelper, role, employee, filter);
            }
            else
            {
                //var locations = hrsUser.HrsSecurity.Locations;
                var locations = hrsUser.HrsSecurity.MedicalLocations;
                if (!locations.Any())
                {
                    return gridRows;
                }

                filter = EmployeeLocationsFilterGenerator.GetLocationsFilter(queryHelper, locations, filter);
            }


            //filter = filter & queryHelper.FilterBuilder
            //    .Where(x => x.MedicalInfo.DrugTests.Any() || 
            //        x.MedicalInfo.LeaveHistory.Any() || 
            //        x.MedicalInfo.MedicalExams.Any() || 
            //        x.MedicalInfo.OtherMedicalInfo.Any());

            //var enc = Encryption.NewEncryption;

            var project = queryHelper.ProjectionBuilder.Expression(x => new { x.Id } );
            var empResults = queryHelper.FindWithProjection(filter, project).ToList();

            var medicalProject = queryHelper.ProjectionBuilder.Expression(x => new
            {
                x.Id,
                x.PayrollId,
                x.FirstName,
                x.LastName,
                x.CostCenterCode,
                x.Location,
                HasDrugTests = x.MedicalInfo.DrugTests.Any(),
                HasLeaveHistory = x.MedicalInfo.LeaveHistory.Any(),
                HasMedicalExams = x.MedicalInfo.MedicalExams.Any(),
                HasOtherMedicalInfo = x.MedicalInfo.OtherMedicalInfo.Any()
            });

            Parallel.ForEach(empResults, emp =>
            {
                var empFilter = queryHelper.FilterBuilder.Where(x => x.Id == emp.Id);
                var filteredEmp = queryHelper.FindWithProjection(empFilter, medicalProject).FirstOrDefault();

                gridRows.Add(new MedicalInfoGridModel(
                        filteredEmp.Id,
                        filteredEmp.PayrollId,
                        filteredEmp.FirstName,
                        filteredEmp.LastName,
                        filteredEmp.CostCenterCode,
                        filteredEmp.Location,
                        filteredEmp.HasDrugTests,
                        filteredEmp.HasMedicalExams,
                        filteredEmp.HasOtherMedicalInfo,
                        filteredEmp.HasLeaveHistory
                    ));

            });

            return gridRows;


        }

        public MedicalInfoModel GetMedicalInfo(string employeeId, HrsUserRef hrsUser)
        {
            var emp = Employee.Helper.FindById(employeeId);

            return new MedicalInfoModel(emp.AsEmployeeRef(), hrsUser);
        }

        public MedicalInfoModel SaveMedicalInfo(MedicalInfoModel model)
        {
            var enc = Encryption.NewEncryption;
            var emp = model.Employee.AsEmployee();
            var mi = emp.MedicalInfo;

            AddNewDrugTests();
            RemoveDeletedDrugTests();

            AddNewMedicalExams();
            RemoveDeletedMedicalExams();

            AddNewMedicalLeaves();
            RemoveDeletedMedicalLeaves();

            AddNewOtherMedicalInfo();
            RemoveDeletedOtherMedicalInfo();


            Employee.Helper.Upsert(emp);
            return new MedicalInfoModel(emp.AsEmployeeRef(),model.HrsUser);

            void AddNewOtherMedicalInfo()
            {
                foreach (var m in model.OtherMedicalInfo)
                {
                    var newRow = false;
                    var d = mi.OtherMedicalInfo.FirstOrDefault(x => x.Id == Guid.Parse(m.Id));

                    if (d == null)
                    {
                        d = new EmployeeOtherMedicalInfo() { Id = Guid.Parse(m.Id) };
                        newRow = true;
                    }

                    d.Comments = m.Comments;
                    d.Date = m.Date;


                    if (newRow) mi.OtherMedicalInfo.Add(d);
                }
            }

            void RemoveDeletedOtherMedicalInfo()
            {
                var removeOtherMedicalInfo = mi.OtherMedicalInfo.
                    Where(info => model.OtherMedicalInfo.All(x => x.Id != info.Id.ToString()))
                    .ToList();
                foreach (var info in removeOtherMedicalInfo)
                {
                    mi.OtherMedicalInfo.Remove(info);
                }
            }


            void AddNewDrugTests()
            {
                foreach (var m in model.DrugTests)
                {
                    var newRow = false;
                    var d = mi.DrugTests.FirstOrDefault(x => x.Id == Guid.Parse(m.Id));

                    if (d == null)
                    {
                        d = new EmployeeDrugTest() {Id = Guid.Parse(m.Id)};
                        newRow = true;
                    }

                    d.Comments = m.Comments;
                    d.DrugTestResult = m.DrugTestResult;
                    d.DrugTestType = m.DrugTestType;
                    d.ResultDate = m.ResultDate;
                    d.TestDate = m.TestDate;


                    if (newRow) mi.DrugTests.Add(d);
                }
            }

            void RemoveDeletedDrugTests()
            {
                var removeDrugTest = mi.DrugTests.Where(drugTest => model.DrugTests.All(x => x.Id != drugTest.Id.ToString()))
                    .ToList();
                foreach (var drugTest in removeDrugTest)
                {
                    mi.DrugTests.Remove(drugTest);
                }
            }

            void AddNewMedicalExams()
            {
                foreach (var m in model.MedicalExams)
                {
                    var newRow = false;
                    var d = mi.MedicalExams.FirstOrDefault(x => x.Id == Guid.Parse(m.Id));

                    if (d == null)
                    {
                        d = new EmployeeMedicalExam() {Id = Guid.Parse(m.Id)};
                        newRow = true;
                    }

                    d.Comments = m.Comments;
                    d.MedicalExamType = m.MedicalExamType;
                    d.DueDate = m.DueDate;
                    d.Completed = m.Completed;
                    d.ExamDate = m.ExamDate;
                    d.IsCompleted = m.IsCompleted;
                    d.Facility = m.Facility;
                    d.Doctor = m.Doctor;
                    d.RequiredActivity = m.RequiredActivity;
                    d.RepeatMonths = m.RepeatMonths;


                    if (newRow) mi.MedicalExams.Add(d);
                }
            }

            void RemoveDeletedMedicalExams()
            {
                var removeMedicalExams =
                    mi.MedicalExams.Where(exam => model.MedicalExams.All(x => x.Id != exam.Id.ToString())).ToList();
                foreach (var exam in removeMedicalExams)
                {
                    mi.MedicalExams.Remove(exam);
                }
            }

            void AddNewMedicalLeaves()
            {
                foreach (var m in model.LeaveHistory)
                {
                    var newHist = false;
                    var d = mi.LeaveHistory.FirstOrDefault(x => x.Id == Guid.Parse(m.Id));

                    if (d == null)
                    {
                        d = new EmployeeMedicalLeaveHistory() {Id = Guid.Parse(m.Id)};
                        newHist = true;
                    }

                    d.EligibleMedicalLeave = m.EligibleMedicalLeave;
                    d.FromDate = enc.Encrypt(m.FromDate);
                    d.ToDate = enc.Encrypt(m.ToDate);
                    d.MedicalLeaveReason = m.MedicalLeaveReason;
                    d.MedicalLeaveType = m.MedicalLeaveType;
                    d.Notes = enc.Encrypt(m.Notes);
                    if (newHist) mi.LeaveHistory.Add(d);
                }
            }

            void RemoveDeletedMedicalLeaves()
            {
                var removeLeaveHistory = mi.LeaveHistory.Where(leave => model.LeaveHistory.All(x => x.Id != leave.Id.ToString()))
                    .ToList();
                foreach (var leave in removeLeaveHistory)
                {
                    mi.LeaveHistory.Remove(leave);
                }
            }
        }

        public EmployeeDrugTest GetNewDrugTest()
        {
            return new EmployeeDrugTest();
        }

        public EmployeeMedicalLeave GetNewMedicalLeave()
        {
            return new EmployeeMedicalLeave();
        }

        public MedicalExam GetNewMedicalExam()
        {
            return new MedicalExam();
        }

        public EmployeeOtherMedicalInfo GetNewOtherMedicalInfo()
        {
            return new EmployeeOtherMedicalInfo();
        }

        public List<MedicalLeaveHistoryGridModel> GetMedicalLeaves(FilterDefinition<Employee> filter)
        {
            var results = new List<MedicalLeaveHistoryGridModel>();

            var queryHelper = new MongoRawQueryHelper<Employee>();
           // var moduleName = "Employee Medical Info";

            var project = queryHelper.ProjectionBuilder.Expression(x => new 
            {

                x.FirstName,
                x.LastName,
                Location = x.Location.Office,
                History = x.MedicalInfo.LeaveHistory

            });

            var empResults = queryHelper.FindWithProjection(filter, project).ToList();

            var enc = Encryption.NewEncryption;

            foreach (var emp in empResults)
                foreach (var history in emp.History)
                {
                   
                    var newDate = enc.Decrypt<DateTime?>(history.ToDate);
                    if (newDate >= DateTime.Now || newDate == null)
                    {
                        results.Add(new MedicalLeaveHistoryGridModel
                        {
                            FirstName = emp.FirstName,
                            LastName = emp.LastName,
                            Location = emp.Location,
                            FromDateConverted = enc.Decrypt<DateTime?>(history.FromDate),
                            MedicalLeaveType = history.MedicalLeaveType
                        });
                    }
                }
            return results;
        }
    }
}
