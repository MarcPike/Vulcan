using System;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.AuditTrails;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperEmployee : HelperBase, IHelperEmployee
    {
        private readonly HelperRequiredActivity _helperRequiredActivity = new HelperRequiredActivity();


        public EmployeeModel GetNewEmployeeModel()
        {
            return new EmployeeModel(new Employee());
        }

        public EmployeeModel GetEmployeeModel(string id)
        {
            var rep = new RepositoryBase<Employee>();

            return new EmployeeModel(rep.Find(id));
        }

        public Employee GetEmployee(string id)
        {
            return new RepositoryBase<Employee>().Find(id);
        }


        public List<EmployeeModel> GetEmployeeDirectReports(string employeeId)
        {
            var result = new List<EmployeeModel>();
            var employee = GetEmployeeModel(employeeId);
            foreach (var employeeRef in employee.DirectReports)
            {
                var emp = Employee.Helper.FindById(employeeRef.Id);
                if (emp == null)
                {
                    var filterPayrollId =
                        Employee.Helper.FilterBuilder.Where(x => x.PayrollId == employeeRef.PayrollId);
                    emp = Employee.Helper.Find(filterPayrollId).FirstOrDefault();
                }

                if (emp != null)
                    try
                    {
                        if (result.All(x => x.PayrollId != employeeRef.PayrollId)) result.Add(new EmployeeModel(emp));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error with {emp.PayrollId} - {emp.FirstName} {emp.LastName} {e.Message}");
                        throw;
                    }
            }

            return result.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
        }

        public List<EmployeeRef> GetAllMyEmployeeReferencesForDirectReports(string employeeId)
        {
            var result = new List<EmployeeRef>();
            var employee = GetEmployeeModel(employeeId);
            foreach (var employeeRef in employee.DirectReports) result.Add(employeeRef);

            return result;
        }

        public List<EmployeeRef> GetAllEmployeeReferencesForLocation(string locationId)
        {
            var result = new List<EmployeeRef>();

            var employees = new RepositoryBase<Employee>().AsQueryable().Where(x => x.Location.Id == locationId)
                .ToList();
            foreach (var employeeRef in employees.Select(x => x.AsEmployeeRef())) result.Add(employeeRef);

            return result;
        }

        public List<EmployeeRef> GetAllEmployeeReferences()
        {
            try
            {
                var filter = Employee.Helper.FilterBuilder.Empty;

                var projection = Employee.Helper.ProjectionBuilder.Expression(x => new EmployeeRef
                {
                    Id = x.Id.ToString(),
                    LastName = x.LastName,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    PreferredName = x.PreferredName,
                    Location = x.Location,
                    PayrollRegion = x.PayrollRegion,
                    PayrollId = x.PayrollId,
                    Login = x.Login
                });

                return Employee.Helper.FindWithProjection(filter, projection).OrderBy(x => x.LastName)
                    .ThenBy(x => x.PreferredName).ThenBy(x => x.FirstName).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public List<EmployeeRef> GetAllEmployeeReferencesBasedOnSecurityRoleModule(string employeeId,
            string moduleTypeName, bool hrsRole)
        {
            var result = new List<EmployeeRef>();

            var employees = GetAllMyEmployeesForModule(employeeId, moduleTypeName, hrsRole);
            foreach (var employeeRef in employees.Select(x => x.AsEmployeeRef())) result.Add(employeeRef);

            return result;
        }

        public List<EmployeeRef> GetAllEmployeeReferencesOfPossibleManagers(string employeeId)
        {
            var result = new List<EmployeeRef>();

            var employee = GetEmployee(employeeId);

            while (employee.Manager != null && employee.Manager.Id != employee.Id.ToString())
            {
                result.Add(employee.Manager);
                employee = GetEmployee(employee.Manager.Id);
            }

            return result;
        }


        public void RemoveEmployee(string employeeId)
        {
            var employee = Employee.Helper.FindById(employeeId);
            if (employee == null) throw new Exception("Employee not found");

            if (employee.DirectReports.Any())
                throw new Exception($"Employee has {employee.DirectReports.Count} direct reports. Cannot remove.");

            var manager = employee.Manager;

            Employee.Helper.DeleteOne(employeeId);

            if (manager != null)
            {
                var employeeManager = manager.AsEmployee();
                foreach (var employeeRef in employeeManager.DirectReports.Where(x => x.Id == employeeId).ToList())
                    employeeManager.DirectReports.Remove(employeeRef);

                Employee.Helper.Upsert(employeeManager);
            }
        }

        public EmployeeModel SaveEmployee(EmployeeModel model)
        {
            if (model.Location == null) throw new Exception("Location is missing");

            var filter = Employee.Helper.FilterBuilder.Empty;
            var project =
                Employee.Helper.ProjectionBuilder.Expression(x =>
                    new
                    {
                        Id = x.Id.ToString(),
                        x.PayrollId,
                        GovernmentId = _encryption.Decrypt<string>(x.GovernmentId),
                        x.FirstName,
                        x.LastName
                    });

            var employeeGovernmentIds = Employee.Helper.FindWithProjection(filter, project).ToList();
            var duplicateFound = employeeGovernmentIds.FirstOrDefault(x =>
                x.GovernmentId == model.GovernmentId && x.Id.ToString() != model.EmployeeId);

            if (duplicateFound != null)
                throw new Exception(
                    $"Duplicate Government ID found for Employee: {duplicateFound.PayrollId} - {duplicateFound.FirstName} {duplicateFound.LastName}");


            if (model.ModifiedByUser == null) throw new Exception("Modified User is missing");

            if (model.LdapUser == null && !string.IsNullOrEmpty(model.Login))
            {
                var thisLogin = model.Login.ToLower();
                var thisEmployeeId = ObjectId.Parse(model.EmployeeId);
                var empWithThisLogin = Employee.Helper
                    .Find(x => x.LdapUser.NetworkId.ToLower() == thisLogin && x.Id != thisEmployeeId).FirstOrDefault();
                if (empWithThisLogin != null)
                    throw new Exception(
                        $"Login [{model.Login}] is already being user by [{empWithThisLogin.PayrollId} - {empWithThisLogin.FirstName} {empWithThisLogin.LastName}");

                empWithThisLogin = Employee.Helper.Find(x => x.Login.ToLower() == thisLogin && x.Id != thisEmployeeId)
                    .FirstOrDefault();
                if (empWithThisLogin != null)
                    throw new Exception(
                        $"Login [{model.Login}] is already being user by [{empWithThisLogin.PayrollId} - {empWithThisLogin.FirstName} {empWithThisLogin.LastName}");
            }

            EmployeeAuditTrail audit = null;
            //var enc = Encryption.NewEncryption;
            var helper = new MongoRawQueryHelper<Employee>();
            var newRow = false;
            JobTitleRef oldJobTitle = null;
            var newJobTitle = model.JobTitle;
            var employee = helper.FindById(model.EmployeeId);
            if (employee != null)
            {
                oldJobTitle = employee.JobTitle;
                audit = new EmployeeAuditTrail(employee, model.ModifiedByUser);
            }
            else
            {
                employee = new Employee
                {
                    Id = ObjectId.Parse(model.EmployeeId)
                };
                newRow = true;
            }

            var employeeId = ObjectId.Parse(model.EmployeeId);
            var duplicatePayrollIds = helper
                .Find(x => x.PayrollId == model.PayrollId && x.Id != employeeId).ToList();
            if (duplicatePayrollIds.Any())
                throw new Exception(
                    $"PayrollId: {model.PayrollId} has already been used {duplicatePayrollIds.Count} times(s)");

            var origManagerRef = employee.Manager;

            employee.OldHrsId = model.OldHrsId;
            employee.PayrollId = model.PayrollId;
            employee.FirstName = model.FirstName;
            employee.MiddleName = model.MiddleName;
            employee.LastName = model.LastName;
            employee.Birthday = _encryption.Encrypt(model.Birthday.Date);
            employee.Address1 = model.Address1;
            employee.Address2 = model.Address2;
            employee.Address3 = model.Address3;
            employee.BusinessRegionCode = model.BusinessRegionCode;
            employee.City = model.City;
            //employee.Country = model.Country;
            employee.ContactCountry = model.ContactCountry;
            employee.ConfirmationDate = model.ConfirmationDate?.Date;
            employee.CostCenterCode = model.CostCenterCode;
            employee.JobTitle = model.JobTitle;
            employee.WorkAreaCode = model.WorkAreaCode;
            employee.Location = model.Location;
            employee.ExternalLocationText = model.ExternalLocationText;
            employee.DeviceGroup = model.DeviceGroup;
            employee.Entity = model.Location.Entity;

            employee.ExternalLoginId = model.ExternalLoginId;

            employee.CompanyNumberCode = model.CompanyNumberCode;
            employee.BusinessUnitCode = model.BusinessUnitCode;

            employee.CountryOfOriginCode = model.CountryOfOriginCode;
            employee.EmergencyContacts = model.EmergencyContacts;

            employee.EmployeeVerifications =
                EmployeeVerificationModel.ConvertModelListToBaseList(model.EmployeeVerifications);
            employee.EducationCertifications = model.EducationCertifications;
            employee.EthnicityCode = model.EthnicityCode;
            employee.GenderCode = model.GenderCode;
            employee.GovernmentId = _encryption.Encrypt(model.GovernmentId);
            employee.PhoneNumbers.Clear();
            foreach (var modelPhoneNumber in model.PhoneNumbers)
                employee.PhoneNumbers.Add(new EmployeePhoneNumber
                {
                    Id = modelPhoneNumber.Id == null ? Guid.NewGuid() : Guid.Parse(modelPhoneNumber.Id),
                    Country = modelPhoneNumber.Country,
                    PhoneNumber = modelPhoneNumber.PhoneNumber,
                    PhoneType = modelPhoneNumber.PhoneType
                });

            employee.EmailAddresses.Clear();
            if (!string.IsNullOrEmpty(model.WorkEmailAddress))
                employee.EmailAddresses.Add(new EmployeeEmailAddress("Work", model.WorkEmailAddress));
            if (!string.IsNullOrEmpty(model.PersonalEmailAddress))
                employee.EmailAddresses.Add(new EmployeeEmailAddress("Personal", model.PersonalEmailAddress));

            //foreach (var emailAddressModel in model.EmailAddresses)
            //{
            //    employee.EmailAddresses.Add(new EmployeeEmailAddress()
            //    {
            //        Id = (emailAddressModel.Id == null) ? Guid.NewGuid() : Guid.Parse(emailAddressModel.Id),
            //        EmailAddress = emailAddressModel.EmailAddress,
            //        EmailType = emailAddressModel.EmailType
            //    });
            //}

            employee.LastRehireDate = model.LastRehireDate?.Date;
            employee.Login = model.Login;
            employee.MaritalStatusCode = model.MaritalStatusCode;
            employee.Manager = model.Manager;

            employee.NationalityCode = model.NationalityCode;
            employee.OriginalHireDate = model.OriginalHireDate?.Date;
            employee.PriorServiceDate = model.PriorServiceDate?.Date;

            employee.PayrollRegion = model.PayrollRegion;
            employee.PostalCode = model.PostalCode;
            employee.PreferredName = model.PreferredName;
            employee.State = model.State;
            employee.Status1Code = model.Status1Code;
            employee.Status2Code = model.Status2Code;
            employee.TerminationCode = model.TerminationCode;
            employee.TerminationDate = model.TerminationDate?.Date;
            employee.TerminationExplanation = model.TerminationExplanation;
            employee.TimeTrackingAccrualProfile = model.TimeTrackingAccrualProfile;
            employee.TimeTrackingAccrualProfileEffectiveDate = model.TimeTrackingAccrualProfileEffectiveDate?.Date;
            employee.EEOCode = model.EEOCode;
            employee.KronosDepartmentCode = model.KronosDepartmentCode;
            employee.KronosManager = model.KronosManager;
            employee.LdapUser = model.LdapUser;
            employee.Lms = model.Lms;


            employee.WorkStatusHistory = model.WorkStatusHistory;

            employee.ModifiedByUserId = model.ModifiedByUser.UserId;
            employee.PerformanceEvaluationType = model.PerformanceEvaluationType;


            employee = helper.Upsert(employee);

            audit?.Save(employee);


            _helperRequiredActivity.CreateRequiredActivitiesForJobTitle(employee.AsEmployeeRef(), oldJobTitle,
                newJobTitle);

            // Add to current Manager
            AddToDirectReportForCurrentManager();

            // Remove from Old Manager if required
            RemoveDirectReportFromOriginalManager();

            if (model.Compensation != null && model.Compensation.IsDirty)
            {
                var hrsUser = model.ModifiedByUser.AsHrsUser();
                if (hrsUser.HrsSecurity.GetRole().Modules.All(x => x.ModuleType.Name != "Compensation"))
                    throw new Exception(
                        "Employee information was saved however Compensation changes are ignored due to security");

                model.Compensation.Employee = employee.AsEmployeeRef();

                var helperCompensation = new HelperCompensation();
                model.Compensation.ModifiedBy = model.ModifiedByUser;
                helperCompensation.SaveCompensation(model.Compensation, newRow, audit);
            }

            return new EmployeeModel(employee);

            void AddToDirectReportForCurrentManager()
            {
                if (employee.Manager != null)
                {
                    var manager = employee.Manager.AsEmployee();
                    if (manager == null) throw new Exception("Could not find Manager");

                    if (manager.DirectReports.All(x => x.Id != employee.Id.ToString()))

                    {
                        manager.DirectReports.Add(employee.AsEmployeeRef());
                        manager.SaveToDatabase();
                    }
                }
            }

            void RemoveDirectReportFromOriginalManager()
            {
                if (origManagerRef != null)
                    if (employee.Manager != null && employee.Manager.Id != origManagerRef.Id)
                    {
                        var origManager = origManagerRef.AsEmployee();
                        var removeDirectReports =
                            origManager.DirectReports.Where(x => x.PayrollId == employee.PayrollId).ToList();
                        foreach (var removeDirectReport in removeDirectReports)
                            origManager.DirectReports.Remove(removeDirectReport);

                        origManager.SaveToDatabase();
                    }
            }
        }

        public List<EmployeeAuditTrailModel> GetAuditTrailForEmployee(string id)
        {
            var employeeId = ObjectId.Parse(id);

            var result = EmployeeAuditTrail.Helper.Find(x => x.Original.Id == employeeId)
                .Project(x => new EmployeeAuditTrailModel(x)).ToList().OrderByDescending(x => x.UpdatedAt).ToList();
            return result.Where(x => x.AnyChanges()).ToList();
        }

        public List<EmployeeDetailsGridModel> GetEmployeeDetailsGrid(string userId)
        {
            var employees = GetAllMyEmployeeGridModelsForModule(userId, "Employee Details", true);
            return employees.Select(x => new EmployeeDetailsGridModel(x)).ToList();
        }

        public List<Employee> GetAllMyEmployeesForHrsModule(string userId, string moduleTypeName)
        {
            return GetAllMyEmployeesForModule(userId, moduleTypeName, true);
        }


        public List<Employee> GetAllMyEmployeesForHseModule(string userId, string moduleTypeName)
        {
            return GetAllMyEmployeesForModule(userId, moduleTypeName, false);
        }


        public List<EmployeeModel> GetAllMyEmployees(HrsUser hrsUser, SecurityRole role, SystemModule module)
        {
            var rep = new RepositoryBase<Employee>();
            var employees = new List<Employee>();

            var locations = module.ModuleType.IsHrsModule
                ? hrsUser.HrsSecurity.Locations
                : hrsUser.HseSecurity.Locations;

            if (!role.DirectReportsOnly)
            {
                if (locations.Any())
                    employees = rep.AsQueryable().Where(x => locations.Any(l => l.Id == x.Location.Id)).ToList();
                else
                    employees = rep.AsQueryable().ToList();
            }
            else
            {
                var employee = FindEmployeeForHrsUser(hrsUser);
                if (employee == null) throw new Exception("Could not find Employee for Hrs User");

                employees = GetAllDecendants(employee.DirectReports.Select(x => x.AsEmployee()).ToList());
            }

            return employees.Select(x => new EmployeeModel(x)).ToList();
        }

        public Employee FindEmployeeForHrsUser(HrsUser user)
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();

            var employeeFound = queryHelper.FindById(user.Employee?.Id);

            //var filterActiveDirectoryId = queryHelper.FilterBuilder.Where(x =>
            //    x.LdapUser.ActiveDirectoryId == user.User.ActiveDirectoryId);

            //var employeeFound = queryHelper.Find(filterActiveDirectoryId).FirstOrDefault();

            if (employeeFound == null)
            {
                var filterLastNameFirstName = queryHelper.FilterBuilder.Where(x =>
                    x.FirstName == user.FirstName && x.LastName == user.LastName && x.Entity.Id == user.Entity.Id);
                employeeFound = queryHelper.Find(filterLastNameFirstName).FirstOrDefault();
            }

            return employeeFound;
        }

        public List<JobTitleRef> GetAllJobTitles()
        {
            //return new RepositoryBase<JobTitle>().AsQueryable().ToList().Select(x => x.AsJobTitleRef()).OrderBy(x => x.Name).ThenBy(x => x.IsActive == true).ToList();
            return new RepositoryBase<JobTitle>()
                .AsQueryable().ToList()
                .Select(x => x.AsJobTitleRef())
                .OrderBy(x => x.IsActive == false)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.IsActive)
                .ThenBy(x => x.Name).ToList();
        }

        public JobTitleModel GetJobTitleModel(string jobTitleId)
        {
            var jobTitle = new RepositoryBase<JobTitle>().Find(jobTitleId);
            if (jobTitle == null) throw new Exception("JobTitle not found");

            return new JobTitleModel(jobTitle);
        }

        public JobTitleModel SaveJobTitle(JobTitleModel model)
        {
            var rep = new RepositoryBase<JobTitle>();
            var jobTitle = rep.Find(model.Id) ?? new JobTitle
            {
                Id = ObjectId.Parse(model.Id)
            };
            jobTitle.Name = model.Name;
            jobTitle.Notes = model.Notes;
            jobTitle.IsActive = model.IsActive;
            jobTitle.TrainingScheduledCourses.Clear();
            foreach (var jobTitleTrainingScheduledCourse in model.TrainingScheduledCourses)
                jobTitle.TrainingScheduledCourses.Add(new TrainingScheduledCourse
                {
                    Id = Guid.Parse(jobTitleTrainingScheduledCourse.Id),
                    TrainingCourse = jobTitleTrainingScheduledCourse.TrainingCourse,
                    RepeatEveryNumberOfMonths = jobTitleTrainingScheduledCourse.RepeatEveryNumberOfMonths
                });
            jobTitle.TrainingInitialCourses.Clear();
            foreach (var initialCourse in model.TrainingInitialCourses)
                jobTitle.TrainingInitialCourses.Add(new TrainingInitialCourse
                {
                    Id = Guid.Parse(initialCourse.Id),
                    TrainingCourse = initialCourse.TrainingCourse,
                    DaysToComplete = initialCourse.DaysToComplete,
                    Comments = initialCourse.Comments,
                    Description = initialCourse.Description
                });

            rep.Upsert(jobTitle);
            return new JobTitleModel(jobTitle);
        }

        public List<GlobalHeadcountModel> GetGlobalHeadCount()
        {
            var result = new List<GlobalHeadcountModel>();

            var queryHelper = new MongoRawQueryHelper<Employee>();
            var filter = queryHelper.FilterBuilder.Where(x =>
                x.Location.Entity.Name == "Howco" && (x.TerminationDate == null || x.TerminationDate > DateTime.Now));
            var project = queryHelper.ProjectionBuilder.Expression(x => new {x.Location.Office, x.WorkAreaCode.Code});
            var employees = queryHelper.FindWithProjection(filter, project).OrderBy(x => x.Office).ToList();


            var grouped = employees.GroupBy(x => x.Office).ToList();

            foreach (var office in grouped)
            {
                var direct = office.Where(x => x.Code == "Direct").ToList();
                var indirect = office.Where(x => x.Code == "Indirect").ToList();
                var series = new List<GlobalHeadcountStatusModel>
                {
                    new GlobalHeadcountStatusModel {Name = "Direct", Value = direct.Count()},
                    new GlobalHeadcountStatusModel {Name = "Indirect", Value = indirect.Count()}
                };

                result.Add(new GlobalHeadcountModel
                {
                    Name = office.Key,
                    Series = series.ToArray()
                });
            }

            return result;
        }

        public List<Employee> GetAllMyEmployeesForModule(string userId, string moduleTypeName, bool hrsRole)
        {
            var employees = new List<Employee>();
            var rep = new RepositoryBase<Employee>();

            var helperUser = new HelperUser();
            var hrsUser = helperUser.GetHrsUser(userId);

            if (hrsUser == null) throw new Exception("User not found");

            var role = hrsRole ? hrsUser.HrsSecurity.GetRole() : hrsUser.HseSecurity.GetRole();
            if (role == null) throw new Exception("No HRS Security role defined for this user");

            var module = role.Modules.FirstOrDefault(x => x.ModuleType.Name == moduleTypeName);
            if (module == null) throw new Exception("Module not found for this Users role");

            var locations = hrsRole ? hrsUser.HrsSecurity.Locations : hrsUser.HseSecurity.Locations;

            if (!role.DirectReportsOnly)
            {
                if (locations.Any())
                    foreach (var locationRef in locations)
                        employees.AddRange(rep.AsQueryable().Where(x => x.Location.Id == locationRef.Id).ToList());
                else
                    employees = rep.AsQueryable().ToList();
            }
            else
            {
                var employee = FindEmployeeForHrsUser(hrsUser);
                if (employee == null) throw new Exception("Could not find Employee for Hrs User");

                employees = GetAllDecendants(employee.DirectReports.Select(x => x.AsEmployee()).ToList());
            }

            return employees;
        }

        public List<BaseGridModel> GetAllMyEmployeeGridModelsForModule(string userId, string moduleTypeName,
            bool hrsRole, bool withPerformance = false, bool withDiscipline = false, bool withBenefits = false,
            bool withPayrollRegion = false)
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();

            FilterDefinition<Employee> additionalFilter = null;
            if (withBenefits)
                additionalFilter = queryHelper.FilterBuilder.Where(x => x.Benefits.BenefitEnrollment.Any());

            if (withDiscipline) additionalFilter = queryHelper.FilterBuilder.Where(x => x.Discipline.Any());

            if (withPerformance)
                additionalFilter = queryHelper.FilterBuilder.Where(x => x.Performance.Any(p => p.Locked));


            var projection = queryHelper.ProjectionBuilder.Expression(x => new BaseGridModel(
                x.TerminationDate == null || x.TerminationDate > DateTime.Now,
                x.Birthday,
                x.Compensation.BaseHours,
                x.BusinessRegionCode,
                x.CostCenterCode,
                x.Id,
                x.FirstName,
                x.GenderCode,
                x.GovernmentId,
                x.OriginalHireDate,
                x.TerminationDate,
                x.JobTitle,
                x.KronosDepartmentCode,
                x.LastName,
                x.Location,
                x.Manager,
                x.MiddleName,
                x.Compensation.PayGradeType,
                x.PayrollId,
                x.PayrollRegion,
                x.PreferredName,
                x.Status1Code,
                x.OldHrsId,
                x.LdapUser,
                x.BusinessUnitCode,
                x.CompanyNumberCode
            ));

            //var collection = new RepositoryBase<Employee>().Collection;

            var gridRows = new List<BaseGridModel>();

            var helperUser = new HelperUser();
            var hrsUser = helperUser.GetHrsUser(userId);

            if (hrsUser == null) throw new Exception("User not found");

            var role = hrsRole ? hrsUser.HrsSecurity.GetRole() : hrsUser.HseSecurity.GetRole();
            if (role == null) throw new Exception("No HRS Security role defined for this user");

            role = new MongoRawQueryHelper<SecurityRole>().FindById(role.Id);

            var module = role.Modules.FirstOrDefault(x => x.ModuleType.Name == moduleTypeName);
            if (module == null) throw new Exception("Module not found for this Users role");


            var locations = hrsRole ? hrsUser.HrsSecurity.Locations : hrsUser.HseSecurity.Locations;

            var payrollRegionsForComp = withPayrollRegion ? hrsUser.HrsSecurity.PayrollRegionsForCompensation : null;


            if (!role.DirectReportsOnly)
            {
                if (withPayrollRegion)
                {
                    if (payrollRegionsForComp != null && payrollRegionsForComp.Any())
                        foreach (var regionRef in payrollRegionsForComp)
                        {
                            var builderEmp = Builders<Employee>.Filter;
                            var filter = builderEmp.Eq(x => x.PayrollRegion.Id, regionRef.Id);

                            gridRows.AddRange(queryHelper.FindWithProjection(filter, projection).ToList());
                        }
                }
                else
                {
                    if (locations.Any())
                        foreach (var locationRef in locations)
                        {
                            var builderEmp = Builders<Employee>.Filter;
                            var filter = builderEmp.Eq(x => x.Location.Id, locationRef.Id);

                            if (additionalFilter != null)
                                gridRows.AddRange(queryHelper.FindWithProjection(filter & additionalFilter, projection)
                                    .ToList());
                            else
                                gridRows.AddRange(queryHelper.FindWithProjection(filter, projection).ToList());
                        }
                    else
                        return gridRows;
                    //var entity = hrsUser.Entity;
                    //var entityFilter = queryHelper.FilterBuilder.Empty;
                    //if (!hrsUser.ViewAllEntities)
                    //{
                    //    entityFilter = queryHelper.FilterBuilder.Eq(x => x.Entity.Id, entity.Id);
                    //}

                    //if (additionalFilter != null)
                    //{
                    //    gridRows = queryHelper.FindWithProjection(entityFilter & additionalFilter, projection)
                    //        .ToList();
                    //}
                    //else
                    //{
                    //    gridRows = queryHelper.FindWithProjection(entityFilter, projection).ToList();
                    //}
                }
            }
            else
            {
                var employee = FindEmployeeForHrsUser(hrsUser);
                if (employee == null) throw new Exception("Could not find Employee for Hrs User");

                var employees = GetAllDecendants(employee.DirectReports.Select(x => x.AsEmployee()).ToList());


                if (withBenefits) employees = employees.Where(x => x.Benefits.BenefitEnrollment.Any()).ToList();

                if (withDiscipline)
                    employees = employees.Where(x => x?.Discipline != null && x.Discipline.Any()).ToList();

                if (withPerformance)
                    employees = employees.Where(x => x?.Performance != null && x.Performance.Any(p => p.Locked))
                        .ToList();


                gridRows.AddRange(employees.Select(x => new BaseGridModel(
                    x.TerminationDate == null || x.TerminationDate > DateTime.Now,
                    x.Birthday,
                    x.Compensation?.BaseHours ?? 0,
                    x.BusinessRegionCode,
                    x.CostCenterCode,
                    x.Id,
                    x.FirstName,
                    x.GenderCode,
                    x.GovernmentId,
                    x.OriginalHireDate,
                    x.TerminationDate,
                    x.JobTitle,
                    x.KronosDepartmentCode,
                    x.LastName,
                    x.Location,
                    x.Manager,
                    x.MiddleName,
                    x.Compensation?.PayGradeType ?? null,
                    x.PayrollId,
                    x.PayrollRegion,
                    x.PreferredName,
                    x.Status1Code,
                    x.OldHrsId,
                    x.LdapUser,
                    x.BusinessUnitCode,
                    x.CompanyNumberCode
                )).Distinct().ToList());
            }

            return gridRows;
        }

        public List<Employee> GetAllDecendants(List<Employee> emps)
        {
            var result = new List<Employee>();
            result.AddRange(emps);
            foreach (var e in emps)
            {
                if (e == null) continue;

                result.AddRange(GetAllDecendants(e.DirectReports.Select(x => x.AsEmployee()).ToList()));
            }

            return result;
        }
    }
}