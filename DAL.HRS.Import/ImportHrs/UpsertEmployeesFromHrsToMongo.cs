using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.SqlServer;
using DAL.HRS.SqlServer.Model;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Profiler;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.Vulcan.Mongo.Base.Queries;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;
using HrsContext = DAL.HRS.SqlServer.Model.HrsContext;
using Location = DAL.Common.DocClass.Location;
using PayrollRegion = DAL.Common.DocClass.PayrollRegion;
using RequiredActivity = DAL.HRS.Mongo.DocClass.Training.RequiredActivity;
using TrainingAttendee = DAL.HRS.Mongo.DocClass.Training.TrainingAttendee;
using TrainingCourse = DAL.HRS.Mongo.DocClass.Training.TrainingCourse;
using WorkStatusHistory = DAL.HRS.Mongo.DocClass.Employee.WorkStatusHistory;
using MongoDB.Bson;

namespace DAL.HRS.Import.ImportHrs
{
    [TestFixture()]
    public class UpsertEmployeesFromHrsToMongo
    {
        private CompensationBuilder _compensationBuilder;

        private Encryption _encryption = Encryption.NewEncryption;

        private bool _onlyAddNewEmployees = true;
        private bool _workStatusHistory = false;
        private bool _compensation = true;
        private bool _performance = false;
        private bool _discipline = false;
        private bool _benefits = false;
        private bool _sensatizeData = false;
        private bool _buildRelationships = false;
        private bool _loadData = true;

        private bool _training = false;
        private bool _medicalInfo = false;
        private bool _requiredActivities = false;
        private bool _getLdapUser = false;
        private bool _removeSoftDeletes = true;
        private bool _checkIsActive = true;
        private bool _getDeviceGroups = false;

        RepositoryBase<TrainingCourse> _repTrainingCourse;
        RepositoryBase<TrainingEvent> _repTrainingEvent;


        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
            //_compensationBuilder = new CompensationBuilder();
            //_repTrainingCourse = new RepositoryBase<TrainingCourse>();
            //_repTrainingEvent = new RepositoryBase<TrainingEvent>();

            //InitializeLocations();
        }

        [Test]
        public void CheckLMS()
        {
            var employeesWithNoLMS = Employee.Helper.Find(x => x.Lms == null && !x.PayrollId.StartsWith("EM") && !x.PayrollId.StartsWith("HSP")).ToList().Take(10);
            foreach (var employee in employeesWithNoLMS)
            {
                Console.WriteLine(employee.PayrollId);
            }
        }

        [Test]
        public void LoadLms()
        {
            using (DAL.HRS.SqlServer.Model.HrsContext context = new DAL.HRS.SqlServer.Model.HrsContext())
            {
                var oldEmployees = context.Employee.AsNoTracking().Where(x => x.LMS != null).ToList();
                foreach (var oldEmployee in oldEmployees)
                {
                    var filter = Employee.Helper.FilterBuilder.Where(x => x.OldHrsId == oldEmployee.OID);
                    var propertyValue = PropertyBuilder.New("LMS", "LMSProperty",
                        oldEmployee.LMS.ToString(), oldEmployee.LmsType.Name);
                    var update = Employee.Helper.UpdateBuilder.Set(x => x.Lms, propertyValue);
                    Employee.Helper.UpdateOne(filter, update);
                }
            }
        }

        [Test]
        public void LoadPerformanceEvaluationType()
        {
            using (DAL.HRS.SqlServer.Model.HrsContext context = new DAL.HRS.SqlServer.Model.HrsContext())
            {
                var oldEmployees = context.Employee.AsNoTracking().Where(x => x.PerformanceEvaluationType != null).ToList();
                foreach (var oldEmployee in oldEmployees)
                {
                    var filter = Employee.Helper.FilterBuilder.Where(x => x.OldHrsId == oldEmployee.OID);

                    var propertyValue = PropertyBuilder.New("PerformanceEvaluationType", "Type of Performance Evaluation",
                        oldEmployee.PerformanceEvaluationType1.Code, oldEmployee.PerformanceEvaluationType1.Description);
                    var update = Employee.Helper.UpdateBuilder.Set(x => x.PerformanceEvaluationType, propertyValue);
                    Employee.Helper.UpdateOne(filter, update);
                }
            }
        }

        //[Test]
        //public void LoadHrRepresentative()
        //{
        //    using (DAL.HRS.SqlServer.Model.HrsContext context = new DAL.HRS.SqlServer.Model.HrsContext())
        //    {
        //        var locations = context.Location.AsNoTracking().ToList();
        //        foreach (var location in locations)
        //        {
        //            location.Name
        //        }
        //    }

        //}


        [Test]
        public void LoadProduction()
        {
            var performanceProfiler = new PerformanceProfiler();

            performanceProfiler.Start("Total Time to Import HRS Data");
            var rep = new RepositoryBase<Employee>();

            if (_loadData)
            {


                performanceProfiler.Start("Get HRS Data");
                var employeeBasicInfoHrs = new EmployeeBasicInfoHrs();
                var employees = employeeBasicInfoHrs.GetEmployeeBasicInfo(0);
                performanceProfiler.Stop("Get HRS Data");

                decimal rows = employees.Count;
                decimal onRow = 0;
                foreach (var employee in employees)
                {
                    onRow++;
                    var emp = GetEmployeeDetails(performanceProfiler, rep, employee);
                    if (_workStatusHistory) GetWorkStatusHistory(performanceProfiler, emp, employee);
                    if (_compensation) GetCompensation(performanceProfiler, employee, emp);
                    if (_performance) GetPerformance(performanceProfiler, emp, employee);
                    if (_discipline) GetDiscipline(performanceProfiler, emp, employee);
                    if (_benefits) GetBenefits(performanceProfiler, emp, employee);
                    //if (!_ignoreMedicalInfo) GetMedicalInfo(performanceProfiler, emp, employee);


                    rep.Upsert(emp);


                    // Post import
                    if (_sensatizeData) SensatizeData(performanceProfiler, rep, emp);

                    var percent = (onRow / rows) * 100;

                }

            }

            if (_getLdapUser) GetLdapUsers();

            if (_removeSoftDeletes) RemoveSoftDeletes();

            //if (_checkIsActive) CheckIsActive();

            if (_buildRelationships) BuildRelationships(performanceProfiler);

            if (_training) GetTraining(performanceProfiler);
            if (_requiredActivities) GetRequiredActivities(performanceProfiler);

            if (_medicalInfo) OnlyGetMedicalInfo();

            if (_getDeviceGroups) GetDeviceGroups();

            performanceProfiler.Stop("Total Time to Import HRS Data");

            var finalProfilerResult = performanceProfiler.GetReport();
            foreach (var activityReport in finalProfilerResult)
            {
                Console.WriteLine($"{activityReport.ActivityName} - {activityReport.TotalDuration}");
            }

        }

        [Test]
        public void GetDeviceGroups()
        {
            using (DAL.HRS.SqlServer.Model.HrsContext context = new DAL.HRS.SqlServer.Model.HrsContext())
            {
                var oldEmployees = context.Employee.AsNoTracking().Where(x => x.KronosClockType1 != null).ToList();
                foreach (var oldEmployee in oldEmployees)
                {
                    var filter = Employee.Helper.FilterBuilder.Where(x => x.OldHrsId == oldEmployee.OID);
                    var propertyValue = PropertyBuilder.New("DeviceGroup", "Device Group",
                        oldEmployee.KronosClockType1.Name, "");
                    var update = Employee.Helper.UpdateBuilder.Set(x => x.DeviceGroup, propertyValue);
                    Employee.Helper.UpdateOne(filter, update);
                }
            }
        }

        //[Test]
        //public void CheckIsActive()
        //{
        //    var filter = Employee.Helper.FilterBuilder.Where(x => x.TerminationDate != null && (x.TerminationDate == null || x.TerminationDate > DateTime.Now));
        //    var employees = Employee.Helper.Find(filter);
        //    foreach (var employee in employees)
        //    {
        //        if (employee.TerminationDate > DateTime.Now)
        //        {
        //            employee.IsActive = false;
        //            Employee.Helper.Upsert(employee);
        //        }
        //    }
        //}

        [Test]
        public void RemoveSoftDeletes()
        {
            using (DAL.HRS.SqlServer.Model.HrsContext context = new DAL.HRS.SqlServer.Model.HrsContext())
            {
                var deletedEmployees = context.Employee.AsNoTracking().Where(x => x.GCRecord != null).ToList();
                foreach (var deletedEmployee in deletedEmployees)
                {
                    var filter = Employee.Helper.FilterBuilder.Where(x => x.OldHrsId == deletedEmployee.OID);
                    Employee.Helper.DeleteOne(filter);
                }

            }
        }

        private void GetLdapUsers()
        {
            var filter = Employee.Helper.FilterBuilder.Where(x => x.LdapUser == null && x.Login != null && x.Login != string.Empty);

            var employees = Employee.Helper.Find(filter);
            foreach (var employee in employees)
            {
                var ldapFilter = LdapUser.Helper.FilterBuilder.Where(x => x.NetworkId == employee.Login);
                var ldapUser = LdapUser.Helper.Find(ldapFilter).FirstOrDefault();
                if (ldapUser != null)
                {
                    employee.LdapUser = ldapUser.AsLdapUserRef();
                    Employee.Helper.Upsert(employee);
                }
            }
        }

        [Test]
        public void AddMissingKronosPayRuleToCompensation()
        {
            var helper = new MongoRawQueryHelper<Employee>();
            using (DAL.HRS.SqlServer.Model.HrsContext context = new DAL.HRS.SqlServer.Model.HrsContext())
            {
                foreach (var employee in context.Employee)
                {
                    var hrsCompensation = employee.Compensation.LastOrDefault();
                    if (hrsCompensation == null) continue;

                    if (hrsCompensation.KronosPayRule1 == null) continue;

                    var filter = helper.FilterBuilder.Where(x => x.OldHrsId == employee.OID);
                    var emp = helper.Find(filter).FirstOrDefault();
                    if (emp != null)
                    {
                        emp.Compensation.KronosPayRuleType = PropertyBuilder.New("KronosPayRuleType",
                            "KronosPayRuleType", hrsCompensation.KronosPayRule1.Name, "");

                        emp.Compensation.KronosPayRuleEffectiveDate = hrsCompensation.KronosPayRuleEffectiveDate;

                        helper.Upsert(emp);
                    }


                }
            }

        }

        [Test]
        public void Execute()
        {
            var performanceProfiler = new PerformanceProfiler();

            performanceProfiler.Start("Total Time to Import HRS Data");
            var rep = new RepositoryBase<Employee>();

            if (_loadData)
            {
                performanceProfiler.Start("Get HRS Data");
                var employeeBasicInfoHrs = new EmployeeBasicInfoHrs();
                var employees = employeeBasicInfoHrs.GetEmployeeBasicInfo(0);
                performanceProfiler.Stop("Get HRS Data");
                foreach (var employee in employees)
                {
                    var emp = GetEmployeeDetails(performanceProfiler, rep, employee);
                    if (_workStatusHistory) GetWorkStatusHistory(performanceProfiler, emp, employee);
                    if (_compensation) GetCompensation(performanceProfiler, employee, emp);
                    if (_performance) GetPerformance(performanceProfiler, emp, employee);
                    if (_discipline) GetDiscipline(performanceProfiler, emp, employee);
                    if (_benefits) GetBenefits(performanceProfiler, emp, employee);
                    //if (!_ignoreMedicalInfo) GetMedicalInfo(performanceProfiler, emp, employee);


                    rep.Upsert(emp);

                    // Post import
                    if (_sensatizeData) SensatizeData(performanceProfiler, rep, emp);

                }

            }

            var onRow = 0;
            onRow++;

            if ((onRow % 100) == 0)
            {
                //var message = "Another 100";
                //var profilerResult = performanceProfiler.GetReport();
                //foreach (var activityReport in profilerResult)
                //{
                //    Console.WriteLine($"{activityReport.ActivityName} - {activityReport.TotalDuration}");
                //}
            }


            if (_buildRelationships) BuildRelationships(performanceProfiler);

            if (_training) GetTraining(performanceProfiler);
            if (_requiredActivities) GetRequiredActivities(performanceProfiler);


            performanceProfiler.Stop("Total Time to Import HRS Data");

            var finalProfilerResult = performanceProfiler.GetReport();
            foreach (var activityReport in finalProfilerResult)
            {
                Console.WriteLine($"{activityReport.ActivityName} - {activityReport.TotalDuration}");
            }


        }




        //private void InitializeLocations()
        //{
        //    if (_reloadLocations)
        //    {
        //        var rep = new RepositoryBase<Location>();
        //        var locations = rep.AsQueryable().ToList();
        //        foreach (var location in locations.ToList())
        //        {
        //            rep.RemoveOne(location);
        //        }

        //        Location.GenerateDefaults();
        //        CreatePayrollRegion();
        //    }
        //}

        private void CreatePayrollRegion()
        {
            PayrollRegion.PopulateValues();
            var payrollRegions = new RepositoryBase<PayrollRegion>().AsQueryable().ToList();

            var repLocations = new RepositoryBase<Location>();
            var locations = repLocations.AsQueryable().ToList();

            foreach (var location in locations.Where(x => x.Country == "China").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("China"));
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Region == "Europe").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Europe"));
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Country == "Malaysia").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Malaysia"));
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Country == "UAE").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Middle East"));
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Country == "Singapore").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Singapore"));
                repLocations.Upsert(location);
            }

            foreach (var location in locations.Where(x => x.Region == "Western Hemisphere").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Western Hemisphere"));
                repLocations.Upsert(location);
            }

            locations = repLocations.AsQueryable().ToList();
            foreach (var location in locations.Where(x => x.PayrollRegions.Count == 0).ToList())
            {
                Console.WriteLine(location.Office);
            }

            PayrollRegionRef GetPayrollRegionFor(string name)
            {
                return payrollRegions.First(x => x.Name == name).AsPayrollRegionRef();
            }
        }



        [Test]
        public void OnlyGetTrainingAndRequiredActivities()
        {
            var performanceProfiler = new PerformanceProfiler();
            GetTraining(performanceProfiler);
            GetRequiredActivities(performanceProfiler);
            GetPlannedActivityRequiredTraining();

            var finalProfilerResult = performanceProfiler.GetReport();
            foreach (var activityReport in finalProfilerResult)
            {
                Console.WriteLine($"{activityReport.ActivityName} - {activityReport.TotalDuration}");
            }

        }

        [Test]
        public void OnlyGetTraining()
        {
            var performanceProfiler = new PerformanceProfiler();
            GetTraining(performanceProfiler);
            GetPlannedActivityRequiredTraining();
            var finalProfilerResult = performanceProfiler.GetReport();
            foreach (var activityReport in finalProfilerResult)
            {
                Console.WriteLine($"{activityReport.ActivityName} - {activityReport.TotalDuration}");
            }
        }

        [Test]
        public void ExecuteGetTraining()
        {
            var performanceProfiler = new PerformanceProfiler();
            GetTraining(performanceProfiler);
        }

        private void GetTraining(PerformanceProfiler performanceProfiler = null)
        {
            if (performanceProfiler == null) performanceProfiler = new PerformanceProfiler();

            performanceProfiler.Start("Getting Training");
            var queryHelperTrainingHours = new MongoRawQueryHelper<TrainingHours>();
            var trainingHours = queryHelperTrainingHours.GetAll().FirstOrDefault() ?? new TrainingHours();


            using (DAL.HRS.SqlServer.Model.HrsContext context = new DAL.HRS.SqlServer.Model.HrsContext())
            {
                var startDate = DateTime.Parse("1/1/2014");

                //    var trainingAttendees = context.TrainingAttendee.AsNoTracking()
                //        .Where(x => x.GCRecord == null && x.Employee != null && x.Training1 != null &&
                //                    x.Training1.StartDate >= startDate).ToList();
                //    var employeeIds = trainingAttendees.Select(x => x.Employee).Distinct();

                //    var employeesCount = employeeIds.Count();
                //    var onEmployee = 0;
                //    var projection = Employee.Helper.ProjectionBuilder.Expression(x => new
                //    {
                //        Id = x.Id, 
                //        x.TrainingEvents, EmployeeRef = new EmployeeRef()
                //        {
                //            Id = x.Id.ToString(),
                //            Location = x.Location,
                //            LastName = x.LastName,
                //            FirstName = x.FirstName,
                //            Login = x.Login,
                //            MiddleName = x.MiddleName,
                //            PayrollId = x.PayrollId,
                //            PayrollRegion = x.PayrollRegion,
                //            PreferredName = x.PreferredName
                //        }
                //    });
                //    FilterDefinition<Employee> filter = null;
                //    UpdateDefinition<Employee> update = null;
                //    foreach (var employeeId in employeeIds)
                //    {
                //        onEmployee++;
                //        var percentComplete = ((decimal)onEmployee / employeesCount) * 100;

                //        // only create once
                //        filter = filter ?? Employee.Helper.FilterBuilder.Where(x => x.OldHrsId == employeeId);

                //        // project the bare minimum
                //        var emp = Employee.Helper.FindWithProjection(filter, projection)
                //            .FirstOrDefault();

                //        if (emp == null) continue;
                //        emp.TrainingEvents.Clear();
                //        foreach (var hrsTrainingAttendee in trainingAttendees.Where(x=>x.Employee == employeeId).ToList())
                //        {
                //            var training = hrsTrainingAttendee.Training1;

                //            var trainingCourse = AddTrainingCourse(training);
                //            var trainingAttendee = AddTrainingAttendee(hrsTrainingAttendee, emp.EmployeeRef);
                //            var trainingEvent = AddTrainingEvent(training, trainingCourse, trainingAttendee,
                //                emp.EmployeeRef);

                //            TrainingEvent.Helper.Upsert(trainingEvent);

                //            emp.TrainingEvents.Add(trainingEvent.AsTrainingEventRef());

                //        }
                //        // only update what we need and only create once
                //        update = update ?? Employee.Helper.UpdateBuilder.Set(x => x.TrainingEvents, emp.TrainingEvents);
                //        Employee.Helper.UpdateOne(filter, update);
                //    }
                //}

                var repEmployee = new RepositoryBase<Employee>();
                var repEvent = new RepositoryBase<TrainingEvent>();

                var trainingAttendees = context.TrainingAttendee.AsNoTracking().Where(x => x.GCRecord == null && x.Employee != null && x.Training1.StartDate >= startDate).ToList();

                //var employeeFilter = Employee.Helper.FilterBuilder.Where(x => x.OriginalHireDate >= startDate && x.OldHrsId > 0 );
                var orginalHireDate = DateTime.Parse("01/01/2020");
                var employeeFilter = Employee.Helper.FilterBuilder.Where(x => x.OldHrsId > 0 && (x.TrainingEvents.Any() || x.OriginalHireDate >= orginalHireDate));

                var employeeProject = Employee.Helper.ProjectionBuilder.Expression(x => x.OldHrsId);

                var employeeOldHrsIds = Employee.Helper.FindWithProjection(employeeFilter, employeeProject).ToList();

                for (int i = 0; i < employeeOldHrsIds.Count; i++)
                {
                    var empId = employeeOldHrsIds.ElementAt(i);

                    //if (empId == 0) 
                    //    continue;

                    var progress = ((decimal)i / employeeOldHrsIds.Count) * 100;

                    var hrsTrainingAttendeeForEmployee = trainingAttendees
                        .Where(x => x.Employee == empId)
                        .ToList();

                    //if (!hrsTrainingAttendeeForEmployee.Any())
                    //     continue;


                    var employeeFindFilter = Employee.Helper.FilterBuilder.Where(x => x.OldHrsId == empId);
                    var emp = Employee.Helper.Find(employeeFindFilter).FirstOrDefault();


                    if (emp == null)
                        continue;

                    var isDirty = false;
                    emp.TrainingEvents.Clear();

                    foreach (var hrsTrainingAttendee in hrsTrainingAttendeeForEmployee)
                    {
                        var training = hrsTrainingAttendee.Training1;

                        if (training == null)
                        {
                            continue;
                        }


                        if (training.StartDate >= startDate)
                        {

                            var trainingCourse = AddTrainingCourse(training);
                            var trainingAttendee = AddTrainingAttendee(hrsTrainingAttendee, emp.AsEmployeeRef());
                            var trainingEvent = AddTrainingEvent(training, trainingCourse, trainingAttendee, emp.AsEmployeeRef());



                            TrainingEvent.Helper.Upsert(trainingEvent);

                            isDirty = true;

                            emp.TrainingEvents.Add(trainingEvent.AsTrainingEventRef());
                        }

                    }

                    if (isDirty)
                    {
                        Employee.Helper.Upsert(emp);
                    }

                }

            }

            performanceProfiler.Stop("Getting Training");

            TrainingAttendee AddTrainingAttendee(SqlServer.Model.TrainingAttendee trainingAttendee, EmployeeRef emp)
            {
                return new TrainingAttendee()
                {
                    DateCompleted = trainingAttendee.DateCompleted,
                    Dismissed = trainingAttendee.Dismissed ?? false,
                    Employee = emp,
                    Reimbursement = trainingAttendee.Reimbursement ?? 0,
                    OldHrsId = trainingAttendee.OID
                };
            }

            TrainingEvent AddTrainingEvent(Training training, TrainingCourse trainingCourse, TrainingAttendee trainingAttendee, EmployeeRef emp)
            {
                LocationRef location = null;
                if (training.Location1 != null)
                {
                    location = GetLocationForHrsLocation(training.Location1.Name);
                }

                var filter = TrainingEvent.Helper.FilterBuilder.Where(x => x.OldHrsId == training.OID);
                var trainingEvent = TrainingEvent.Helper.Find(filter).FirstOrDefault();


                if (trainingEvent == null)
                {
                    EmployeeRef internalInstructor = null;
                    if ((training.InternalInstructor != null) && (training.InternalInstructor > 0))
                    {
                        internalInstructor = new RepositoryBase<Employee>().AsQueryable()
                            .FirstOrDefault(x => x.OldHrsId == training.InternalInstructor)
                            ?.AsEmployeeRef();
                    }

                    trainingEvent = new TrainingEvent()
                    {
                        OldHrsId = training.OID,
                        //Employee = emp,
                        CertificateExpiration = training.CertificateExpiration,
                        Cost = training.Cost ?? 0,
                        StartDate = training.StartDate,
                        EndDate = training.EndDate,
                        ExternalInstructor = training.ExternalInstructor ?? "(n/a)",
                        InternalInstructor = internalInstructor,
                        TrainingHours = training.TrainingHoursType?.Hours ?? 0
                    };

                    if ((training.TrainingHoursType != null) && (training.TrainingHoursType?.Hours != 0))
                    {
                        if (trainingHours.Values.All(x => x != training.TrainingHoursType.Hours))
                        {
                            trainingHours.Values.Add(training.TrainingHoursType.Hours ?? 0);
                            queryHelperTrainingHours.Upsert(trainingHours);
                        }
                    }

                    if (trainingCourse != null)
                    {
                        trainingEvent.TrainingCourse = trainingCourse.AsTrainingCourseRef();
                    }

                }

                if (location != null)
                {
                    trainingEvent.Location = location;
                }

                trainingEvent.Attendees.Add(trainingAttendee);
                return trainingEvent;
            }

            TrainingCourse AddTrainingCourse(Training training)
            {

                if ((training.TrainingCourse == null)) return null;

                var filter = TrainingCourse.Helper.FilterBuilder.Where(x => x.OldHrsId == training.TrainingCourse.OID);
                var trainingCourse = TrainingCourse.Helper.Find(filter).FirstOrDefault();


                var locations = new List<LocationRef>();
                var oldCourseLocations = training.TrainingCourse.TrainingCourse_Locations.ToList();
                foreach (var trainingCourseLocation in oldCourseLocations)
                {
                    locations.Add(GetLocationForHrsLocation(trainingCourseLocation.Location.Name));
                }

                if (trainingCourse == null)
                {
                    trainingCourse = new TrainingCourse()
                    {
                        OldHrsId = training.TrainingCourse.OID,
                        Description = training.TrainingCourse.Description,
                        Name = training.TrainingCourse.Name,
                        CourseType = PropertyBuilder.CreatePropertyValue("TrainingCourseType",
                            "Type of Training Course",
                            training.TrainingCourseType.Name, "").AsPropertyValueRef(),
                        GroupClassification = PropertyBuilder.CreatePropertyValue(
                                "TrainingGroupClassification", "Group Classification",
                                training.TrainingCourse.TrainingGroupClassification.Name, training.TrainingCourse.Description)
                            .AsPropertyValueRef(),
                        VenueType = PropertyBuilder.CreatePropertyValue("TrainingVenueType",
                                "Type of Venue",
                                training.TrainingCourse.TrainingCourseVenueType?.Name ?? "(none)", "")
                            .AsPropertyValueRef(),
                        ExpirationMonths = training.TrainingCourse.ExpirationMonths ?? 0


                    };
                }

                if (locations.Count == 0)
                {
                    locations.Add(GetLocationForHrsLocation("<unknown>"));
                }

                trainingCourse.Locations = locations;

                TrainingCourse.Helper.Upsert(trainingCourse);

                return trainingCourse;
            }
        }

        [Test]
        public void OnlyGetRequiredActivities()
        {
            var performanceProfiler = new PerformanceProfiler();
            GetRequiredActivities(performanceProfiler);
            var finalProfilerResult = performanceProfiler.GetReport();
            foreach (var activityReport in finalProfilerResult)
            {
                Console.WriteLine($"{activityReport.ActivityName} - {activityReport.TotalDuration}");
            }
        }

        [Test]
        public void FindJobTitlesWithTrainingCourses()
        {
            var jobTitles = JobTitle.Helper.Find(x => x.TrainingInitialCourses.Any()).ToList();
            foreach (var jobTitle in jobTitles)
            {
                Console.WriteLine(jobTitle.Name);
            }
        }


        [Test]
        public void GetPlannedActivityRequiredTraining()
        {
            var queryHelperTrainingCourse = new MongoRawQueryHelper<TrainingCourse>();
            var queryHelperJobTitle = new MongoRawQueryHelper<JobTitle>();
            var repEmployee = new RepositoryBase<Employee>();
            using (DAL.HRS.SqlServer.Model.HrsContext context = new DAL.HRS.SqlServer.Model.HrsContext())
            {
                foreach (var titleType in context.TitleType.Where(x => x.PlannedActivity.Any()))
                {
                    var jobTitleFilter = queryHelperJobTitle.FilterBuilder.Where(x => x.Name == titleType.TitleName);
                    var jobTitle = queryHelperJobTitle.Find(jobTitleFilter).SingleOrDefault();
                    if (jobTitle == null) continue;

                    var plannedActivityAdded = false;
                    foreach (var plannedActivity in titleType.PlannedActivity.Where(x => x.TrainingCourse != null))
                    {

                        var trainingCourseOldHrsId = plannedActivity.TrainingCourse;
                        var trainingCourseFilter =
                            queryHelperTrainingCourse.FilterBuilder.Where(x => x.OldHrsId == trainingCourseOldHrsId);
                        var trainingCourse = queryHelperTrainingCourse.Find(trainingCourseFilter).FirstOrDefault();

                        if (trainingCourse == null)
                        {
                            trainingCourse = GetTrainingCourseFor(plannedActivity.TrainingCourse ?? 0, context);
                        }

                        if (jobTitle.TrainingInitialCourses.All(
                            x => x.TrainingCourse.Id != trainingCourse.Id.ToString()))
                        {
                            jobTitle.TrainingInitialCourses.Add(new TrainingInitialCourse()
                            {
                                TrainingCourse = trainingCourse.AsTrainingCourseRef(),
                                Comments = plannedActivity.Comments,
                                DaysToComplete = plannedActivity.DaysToComplete ?? 0,
                                Description = plannedActivity.Description
                            });
                            plannedActivityAdded = true;
                        }
                    }

                    if (plannedActivityAdded)
                    {
                        queryHelperJobTitle.Upsert(jobTitle);
                    }

                }
            }

            TrainingCourse GetTrainingCourseFor(int id, HrsContext context)
            {
                var hrsTrainingCourse = context.TrainingCourse.FirstOrDefault(x => x.OID == id);
                Assert.IsNotNull(hrsTrainingCourse);

                Certification certification = null;


                var newTrainingCourse = new TrainingCourse()
                {
                    CourseType = PropertyBuilder.CreatePropertyValue("TrainingCourseType",
                        "Type of Training Course",
                        hrsTrainingCourse.TrainingCourseType.Name, "").AsPropertyValueRef(),
                    ExpirationMonths = hrsTrainingCourse.ExpirationMonths ?? 0,
                    Description = hrsTrainingCourse.Description,
                    Name = hrsTrainingCourse.Name,
                    OldHrsId = hrsTrainingCourse.OID,
                    GroupClassification = PropertyBuilder.CreatePropertyValue(
                            "TrainingGroupClassification", "Group Classification",
                            hrsTrainingCourse.TrainingGroupClassification.Name, hrsTrainingCourse.Description)
                        .AsPropertyValueRef(),
                    VenueType = PropertyBuilder.CreatePropertyValue("TrainingVenueType",
                            "Type of Venue",
                            hrsTrainingCourse.TrainingCourseVenueType?.Name ?? "(none)", "")
                        .AsPropertyValueRef(),
                };

                queryHelperTrainingCourse.Upsert(newTrainingCourse);

                if (hrsTrainingCourse.ExpirationMonths > 0)
                {
                    var queryHelperCertification = new MongoRawQueryHelper<Certification>();
                    certification = new Certification()
                    {
                        TrainingCourse = newTrainingCourse.AsTrainingCourseRef(),
                        Name = newTrainingCourse.Name,
                        RequiredEveryDays = 0,
                        RequiredEveryWeeks = 0,
                        RequiredEveryMonths = hrsTrainingCourse.ExpirationMonths ?? 0
                    };

                    queryHelperCertification.Upsert(certification);

                    newTrainingCourse.Certification = certification.AsCertificationRef();
                    queryHelperTrainingCourse.Upsert(newTrainingCourse);
                }

                return newTrainingCourse;
            }

        }


        private void GetRequiredActivities(PerformanceProfiler performanceProfiler)
        {
            performanceProfiler.Start("Getting Required Activities");

            var repEmployee = new RepositoryBase<Employee>();
            using (DAL.HRS.SqlServer.Model.HrsContext context = new DAL.HRS.SqlServer.Model.HrsContext())
            {
                var repRequiredActivity = new RepositoryBase<RequiredActivity>();
                var allRequiredActivities = context.RequiredActivity.AsNoTracking().ToList();
                var employeeList = repEmployee.AsQueryable().Select(x => x.OldHrsId).ToList();
                var onRow = 0;
                var count = employeeList.Count;
                foreach (var empId in employeeList)
                {
                    onRow++;
                    var requiredActivities = allRequiredActivities.Where(x => x.Employee == empId).ToList();
                    if (!requiredActivities.Any()) continue;

                    var emp = repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == empId);
                    if (emp == null)
                    {
                        Console.WriteLine($"No employee found for OldHrsId: {empId} ");
                        continue;
                    }
                    var empRef = emp.AsEmployeeRef();

                    var requiredActivitiesForEmployee = repRequiredActivity.AsQueryable().Where(x => x.Employee.Id == empRef.Id)
                        .ToList();
                    foreach (var requiredActivity in requiredActivitiesForEmployee)
                    {
                        repRequiredActivity.RemoveOne(requiredActivity);
                    }

                    foreach (var requiredActivity in requiredActivities)
                    {
                        // ignore duplicates
                        if (repRequiredActivity.AsQueryable().Any(x => x.OldHrsId == requiredActivity.OID)) continue;

                        var newActivity = new RequiredActivity()
                        {
                            OldHrsId = requiredActivity.OID,
                            Employee = emp.AsEmployeeRef(),
                            ActivityType = PropertyBuilder.CreatePropertyValue("RequiredActivityType",
                                    "Type of Required Activity", requiredActivity.RequiredActivityType.Name, "")
                                .AsPropertyValueRef(),
                            Comments = requiredActivity.Comments,
                            CompleteStatus = PropertyBuilder.CreatePropertyValue("RequiredActivityCompleteStatus",
                                    "Completion Status", requiredActivity.CompletionStatusType.Name, "")
                                .AsPropertyValueRef(),
                            CompletionDeadline = requiredActivity.CompletionDeadline,
                            DateCompleted = requiredActivity.DateCompleted,
                            RevisedCompletionDeadline = requiredActivity.RevisedCompletionDeadline,
                            Description = requiredActivity.Description,
                        };

                        if (requiredActivity.TrainingCourse != null)
                        {
                            var thisTrainingCourse =
                                _repTrainingCourse.AsQueryable().FirstOrDefault(x => x.OldHrsId == requiredActivity.TrainingCourse);
                            if (thisTrainingCourse != null)
                                newActivity.TrainingCourse = thisTrainingCourse.AsTrainingCourseRef();

                        }

                        repRequiredActivity.Upsert(newActivity);

                    }
                    emp.SaveToDatabase();
                }
            }
            performanceProfiler.Stop("Getting Required Activities");


        }

        private void GetBenefits(PerformanceProfiler performanceProfiler, Employee emp, EmployeeModelHrs employee)
        {
            performanceProfiler.Start("Get Benefits");
            try
            {
                BenefitsTransformer.TransformBenefits(emp, employee.Benefits);


            }
            catch (Exception)
            {
                Console.WriteLine("Benefits");
            }

            performanceProfiler.Stop("Get Benefits");
        }

        private static void SensatizeData(PerformanceProfiler performanceProfiler, RepositoryBase<Employee> rep, Employee emp)
        {
            if (emp == null)
            {
                return;
            }

            performanceProfiler.Start("Sensitize");

            //rep.Upsert(emp);

            if ((EnvironmentSettings.CurrentEnvironment == Environment.Development) ||
                (EnvironmentSettings.CurrentEnvironment == Environment.QualityControl))
            {
                emp = DataSensitizer.Execute(emp);
            }

            rep.Upsert(emp);

            performanceProfiler.Stop("Sensitize");
        }

        private static void GetDiscipline(PerformanceProfiler performanceProfiler, Employee emp, EmployeeModelHrs employee)
        {
            performanceProfiler.Start("Get Discipline");
            try
            {

                emp.Discipline.Clear();

                if (employee.Discipline != null)
                {
                    foreach (var disc in employee.Discipline)
                    {
                        DisciplineTransformer.TransformDiscipline(emp, disc);
                    }
                }

                if (employee.DisciplineHistory != null)
                {
                    foreach (var disciplineHistoryHrse in employee.DisciplineHistory)
                    {
                        DisciplineTransformer.TransformDisciplineHistory(emp, disciplineHistoryHrse);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Discipline");
            }

            performanceProfiler.Stop("Get Discipline");
        }

        private static void GetPerformance(PerformanceProfiler performanceProfiler, Employee emp, EmployeeModelHrs employee)
        {
            performanceProfiler.Start("Get Performance");
            try
            {
                emp.Performance.Clear();

                if (employee.Performance != null)
                {
                    PerformanceTransformer.TransformPerformance(emp, employee.Performance);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Performance");
            }

            performanceProfiler.Stop("Get Performance");
        }

        private void GetCompensation(PerformanceProfiler performanceProfiler, EmployeeModelHrs employee, Employee emp)
        {
            performanceProfiler.Start("Get Compensation");

            try
            {
                _compensationBuilder.GetBaseCompensationHrs(employee.OldHrsId);
                var compensationHrs = _compensationBuilder.Data;

                CompensationTransformer.TransFormCompensation(emp, compensationHrs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Compensation: {ex.Message}");
            }

            performanceProfiler.Stop("Get Compensation");
        }

        private void GetWorkStatusHistory(PerformanceProfiler performanceProfiler, Employee emp, EmployeeModelHrs employee)
        {
            performanceProfiler.Start("Get WorkStatus History");
            try
            {
                GetWorkStatusHistory(emp, employee);
            }
            catch (Exception)
            {
                Console.WriteLine("WorkStatus");
            }

            performanceProfiler.Stop("Get WorkStatus History");
        }

        //private void GetMedicalInfo(PerformanceProfiler performanceProfiler, Employee emp, EmployeeModelHrs employee)
        //{
        //    performanceProfiler.Start("Get Medical Info");
        //    try
        //    {
        //        LoadMedicalInfo(emp, employee);
        //    }
        //    catch (Exception)
        //    {
        //        Console.WriteLine("Medical Info");
        //    }

        //    performanceProfiler.Stop("Get Medical Info");
        //}

        [Test]
        public void OnlyGetMedicalInfo()
        {
            var performanceProfiler = new PerformanceProfiler();

            performanceProfiler.Start("Getting Medical Info");

            using (DAL.HRS.SqlServer.Model.HrsContext context = new DAL.HRS.SqlServer.Model.HrsContext())
            {
                var project = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.OldHrsId });
                var filter = Employee.Helper.FilterBuilder.Empty;
                var employeeList = Employee.Helper.FindWithProjection(filter, project).ToList();
                var onRow = 0;
                foreach (var ourEmployeeRef in employeeList)
                {
                    if (ourEmployeeRef.OldHrsId == 0) continue;

                    onRow++;
                    var emp = Employee.Helper.FindById(ourEmployeeRef.Id);
                    if (emp == null)
                    {
                        Console.WriteLine($"No employee found for OldHrsId: {ourEmployeeRef} ");
                        continue;
                    }

                    var employee = context.Employee.FirstOrDefault(x => x.OID == ourEmployeeRef.OldHrsId);
                    if (employee == null) continue;

                    LoadMedicalInfo(emp, employee);


                    emp.SaveToDatabase();
                }
            }
            performanceProfiler.Stop("Getting Medical Info");





            var finalProfilerResult = performanceProfiler.GetReport();
            foreach (var activityReport in finalProfilerResult)
            {
                Console.WriteLine($"{activityReport.ActivityName} - {activityReport.TotalDuration}");
            }
        }


        private void LoadMedicalInfo(Employee emp, DAL.HRS.SqlServer.Model.Employee employee)
        {

            var moduleName = "Employee Medical Info";

            emp.MedicalInfo.DrugTests.Clear();
            emp.MedicalInfo.MedicalExams.Clear();
            emp.MedicalInfo.OtherMedicalInfo.Clear();
            emp.MedicalInfo.LeaveHistory.Clear();

            foreach (var medicalInfo in employee.MedicalInfo.Where(x => x.GCRecord == null))
            {
                emp.MedicalInfo = new EmployeeMedicalInfo();
                var thisMedInfo = emp.MedicalInfo;
                foreach (var drugTest in medicalInfo.DrugTest.Where(x => x.GCRecord == null))
                {
                    thisMedInfo.DrugTests.Add(new DAL.HRS.Mongo.DocClass.Employee.EmployeeDrugTest()
                    {
                        DrugTestType = PropertyBuilder.New("DrugTestType", "Type of drug test", drugTest.DrugTestType.Name, ""),
                        DrugTestResult = PropertyBuilder.New("DrugTestResult", "Result of drug test", drugTest.DrugTestResult.Name, ""),
                        ResultDate = drugTest.ResultDate,
                        TestDate = drugTest.TestDate,
                        Comments = drugTest.Comments
                    });
                }

                //foreach (var medLeave in medicalInfo.MedLeave.Where(x => x.GCRecord == null))
                //{

                //    thisMedInfo.MedicalLeaves.Add(new DAL.HRS.Mongo.DocClass.Employee.EmployeeMedicalLeave()
                //    {
                //        MedicalLeaveType = PropertyBuilder.New("MedicalLeaveType", "Type of medical leave", medLeave.MedicalLeaveType.Name, ""),
                //        FromDate = medLeave.FromDate,
                //        ToDate = medLeave.ToDate,
                //        Notes = medLeave.Notes,

                //    });
                //}

                foreach (var exam in medicalInfo.MedicalExam.Where(x => x.GCRecord == null))
                {
                    thisMedInfo.MedicalExams.Add(new DAL.HRS.Mongo.DocClass.MedicalExams.EmployeeMedicalExam()
                    {
                        Employee = emp.AsEmployeeRef(),
                        Doctor = exam.Doctor,
                        Facility = exam.Facility,
                        MedicalExamType = PropertyBuilder.New("MedicalExamType", "Type of medical exam", exam.MedicalExamType.Name, ""),
                        DueDate = exam.NextExamDate,
                        IsCompleted = exam.NextExamCompleted ?? false,

                        ExamDate = exam.ExamDate,
                        Comments = exam.Comments,
                        RepeatMonths = 0
                    });
                }

                foreach (var leave in medicalInfo.LeaveHistory.Where(x => x.GCRecord == null))
                {
                    thisMedInfo.LeaveHistory.Add(new EmployeeMedicalLeaveHistory()
                    {
                        EligibleMedicalLeave = leave.Eligible_Medical_Leave ?? false,
                        FromDate = leave.FromDate,
                        ToDate = leave.ToDate,
                        MedicalLeaveReason = PropertyBuilder.New("MedicalLeaveHistoryReason", "Reason for medical leave",
                            leave.ReasonCodes1?.ReasonName ?? "Unspecified", string.Empty),
                        MedicalLeaveType = PropertyBuilder.New("MedicalLeaveType", "Type of medical leave",
                            leave.MedicalLeaveType1?.Name ?? "Unspecified", ""),
                        Notes = leave.Notes,
                    });
                }

                foreach (var info in medicalInfo.OtherMedicalInfo.Where(x => x.GCRecord == null))
                {
                    thisMedInfo.OtherMedicalInfo.Add(new DAL.HRS.Mongo.DocClass.Employee.EmployeeOtherMedicalInfo()
                    {
                        Date = info.Date,
                        Comments = info.Comments,
                    });
                }

                Employee.Helper.Upsert(emp);

                //ClearOutExistingSupportingDocuments(emp, moduleName);

                using (HrsContext context = new HrsContext())
                {
                    context.Database.CommandTimeout = 120;
                    var encryption = Encryption.NewEncryption;
                    var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                    var firstAdminFound = new RepositoryBase<HrsUser>().AsQueryable().ToList()
                        .First(x => x.SystemAdmin);

                    foreach (var doc in medicalInfo.MedicalInfoSupportingDocument)
                    {

                        byte[] fileBytes;
                        if (doc.MyFileData == null) continue;
                        if (doc.MedicalInfoSupportingDocumentType == null) continue;


                        try
                        {
                            fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            continue;
                        }

                        if (fileBytes == null || fileBytes.Length == 0) continue;


                        var comments = doc.Comments ?? string.Empty;
                        var fileName = doc.MyFileData.FileName;
                        var documentDate = doc.DocumentDate ?? DateTime.Now;
                        var documentType = doc.MedicalInfoSupportingDocumentType.Name.Replace("/", "-");
                        var userId = firstAdminFound.UserId;

                        try
                        {

                            FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate,
                                emp, moduleName, documentType, userId, comments, 0, true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} for Medical Info");
                            Console.WriteLine(ex.Message);
                        }

                    }
                }


            }

        }

        private void ClearOutExistingSupportingDocuments(Employee emp, string moduleName)
        {
            var filter = SupportingDocument.Helper.FilterBuilder.Where(x => x.Module.Name == moduleName && x.BaseDocumentId == emp.Id);
            var supportingDocs = SupportingDocument.Helper.Find(filter).ToList();
            foreach (var supportingDocument in supportingDocs)
            {
                FileAttachmentsHrs.Remove(supportingDocument.FileInfo.Id);
            }
        }

        public Employee GetEmployeeDetails(PerformanceProfiler performanceProfiler, RepositoryBase<Employee> rep,
            EmployeeModelHrs employee)
        {
            performanceProfiler.Start("Search for Employee");

            var emp = rep.AsQueryable().SingleOrDefault(x => x.OldHrsId == employee.OldHrsId);

            performanceProfiler.Stop("Search for Employee");

            if ((emp != null) && (_onlyAddNewEmployees))
            {
                return emp;
            }

            var jobTitles = new RepositoryBase<JobTitle>().AsQueryable().ToList();

            if (emp == null) emp = new Employee()
            {
                PayrollId = employee.PayrollId,
                OldHrsId = employee.OldHrsId
            };

            performanceProfiler.Start("Get Employee Details");


            emp.Entity = Entity.GetRefByName("Howco");
            emp.OldHrsId = employee.OldHrsId;
            emp.PayrollId = employee.PayrollId;
            emp.FirstName = employee.FirstName.TrimEnd();
            emp.MiddleName = employee.MiddleName;
            emp.LastName = employee.LastName.TrimEnd();
            emp.Birthday = employee.Birthday;
            emp.Address1 = employee.Address1;
            emp.Address2 = employee.Address2;
            emp.Address3 = employee.Address3;
            emp.City = employee.City;
            emp.Country = PropertyBuilder
                .CreatePropertyValue("Country", "Country type", employee.Country, "Country type").AsPropertyValueRef();
            emp.ConfirmationDate = employee.ConfirmationDate;
            emp.CostCenterCode = PropertyBuilder.CreatePropertyValue("CostCenter", "Howco Cost Centers,",
                employee.CostCenterCode, employee.CostCenterName).AsPropertyValueRef();
            var jobTitleName = employee.Title;

            var jobTitle = jobTitles.FirstOrDefault(x => x.Name == jobTitleName);
            if (jobTitle == null)
            {
                jobTitle = new JobTitle() { Name = jobTitleName };
                jobTitle.SaveToDatabase();
            }

            emp.JobTitle = jobTitle.AsJobTitleRef();

            //emp.JobClassificationCode = PropertyBuilder.CreatePropertyValue("JobClassification", "Job Classification", employee.J, "Employee Job Title").AsPropertyValueRef();
            emp.WorkAreaCode = PropertyBuilder.CreatePropertyValue("WorkArea", "Work Area", employee.WorkArea, "Work Area Code")
                .AsPropertyValueRef();

            emp.Location = GetLocationForHrsLocation(employee.Location);

            emp.EEOCode = PropertyBuilder.CreatePropertyValue("EEO", "EEO Code", employee.EEO_Code, "Employee EEO Code")
                .AsPropertyValueRef();

            emp.CountryCode = PropertyBuilder.CreatePropertyValue("Country", "Country", employee.Country, "Employee Country")
                .AsPropertyValueRef();
            emp.CountryOfOriginCode = PropertyBuilder.CreatePropertyValue("CountryOfOrigin", "Country of origin",
                employee.CountryOfOrigin, "Employee Country of Origin").AsPropertyValueRef();
            emp.EmergencyContacts = employee.EmergencyContacts.Select(x => new EmergencyContact()
            {
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                Relationship = x.Relationship
            }).ToList();
            emp.EmployeeVerifications = employee.EmployeeVerifications.Select(x => new EmployeeVerification()
            {
                DocumentType = PropertyBuilder.CreatePropertyValue("EmployeeVerificationDocumentType",
                        "Employee Verification Document Type", x.DocumentType, "Employee Verification Document Type")
                    .AsPropertyValueRef(),
                DocumentNumber = x.DocumentNumber,
                DocumentExpiration = x.DocumentExpiration
            }).ToList();
            emp.EthnicityCode = PropertyBuilder
                .CreatePropertyValue("Ethnicity", "Ethnicity", employee.Ethnicity, "Employee Ethnicity").AsPropertyValueRef();
            emp.GenderCode = PropertyBuilder.CreatePropertyValue("Gender", "Gender", employee.Gender, "Employee Gender")
                .AsPropertyValueRef();
            emp.GovernmentId = employee.GovernmentId;

            emp.RehireStatusCode = (employee.RehireStatus != null)
                ? PropertyBuilder
                    .CreatePropertyValue("RehireStatus", "Rehire Status", employee.RehireStatus, "Employee Rehire Status")
                    .AsPropertyValueRef()
                : null;
            emp.KronosDepartmentCode = PropertyBuilder.CreatePropertyValue("KronosDepartment", "Kronos Department",
                employee.KronosDepartment, "Employee Kronos Department").AsPropertyValueRef();
            //emp.KronosLaborLevelCode = PropertyBuilder.CreatePropertyValue("KronosLaborLevel", "Kronos Labor Level",
            //    employee.KronosLaborLevel, "Employee Kronos Labor Level").AsPropertyValueRef();

            emp.Login = employee.Login;
            emp.OriginalManagerPayrollId = employee.ManagerPayrollId;
            emp.OriginalManagerName = employee.ManagerName;
            emp.MaritalStatusCode = PropertyBuilder
                .CreatePropertyValue("MaritalStatus", "Marital Status", employee.MaritalStatus, "Employee Marital Status")
                .AsPropertyValueRef();
            emp.NationalityCode = PropertyBuilder
                .CreatePropertyValue("Nationality", "Nationality", employee.Nationality, "Employee Nationality")
                .AsPropertyValueRef();
            emp.OriginalHireDate = employee.OriginalHireDate;

            emp.PayrollRegion = emp.Location.AsLocation().PayrollRegions.First();

            emp.BusinessRegionCode = PropertyBuilder.CreatePropertyValue("BusinessRegion", "Business Region",
                employee.BusinessRegion, "Employee Business Region").AsPropertyValueRef();


            if (emp.Login != string.Empty)
            {
                var filter = LdapUser.Helper.FilterBuilder.Where(x => x.NetworkId == emp.Login);
                var ldapUser = LdapUser.Helper.Find(filter).FirstOrDefault();
                if (ldapUser != null)
                {
                    emp.LdapUser = ldapUser.AsLdapUserRef();
                }
            }

            emp.EmailAddresses.Clear();
            if (!String.IsNullOrEmpty(employee.PersonalEmail))
            {
                emp.EmailAddresses.Add(new EmployeeEmailAddress("Personal", employee.PersonalEmail));
            }

            if (!String.IsNullOrEmpty(employee.WorkEmail))
            {
                emp.EmailAddresses.Add(new EmployeeEmailAddress("Work", employee.WorkEmail));
            }

            emp.PhoneNumbers.Clear();
            if (!String.IsNullOrEmpty(employee.Home))
            {
                emp.PhoneNumbers.Add(new EmployeePhoneNumber("Home", employee.Home, employee.Country));
            }

            if (!String.IsNullOrEmpty(employee.Mobile))
            {
                emp.PhoneNumbers.Add(new EmployeePhoneNumber("Mobile", employee.Mobile, employee.Country));
            }

            emp.PriorServiceDate = employee.PriorServiceDate;
            emp.LastRehireDate = employee.LastRehireDate;

            emp.PostalCode = employee.PostalCode;
            emp.PreferredName = (employee.PreferredName != null) ? employee.PreferredName.TrimEnd() : string.Empty;
            emp.State = employee.State;
            emp.Status1Code = PropertyBuilder
                .CreatePropertyValue("Status1", "Status1", employee.Status1, "")
                .AsPropertyValueRef();
            emp.Status2Code = PropertyBuilder
                .CreatePropertyValue("Status2", "Status2", employee.Status2, "")
                .AsPropertyValueRef();
            emp.TerminationCode = (employee.TerminationCode != null)
                ? PropertyBuilder
                    .CreatePropertyValue("TerminationCode", "Termination Code", employee.TerminationCode, "")
                    .AsPropertyValueRef()
                : null;
            emp.TerminationDate = employee.TerminationDate;
            emp.TerminationExplanation = employee.TerminationExplanation;
            emp.TimeTrackingAccrualProfile = employee.TimeTrackingAccrualProfile;
            emp.TimeTrackingAccrualProfileEffectiveDate = employee.TimeTrackingAccrualProfileEffectiveDate;

            emp.IssuedEquipment.Clear();
            //emp.IssuedEquipment.Add(new IssuedEquipment()
            //{
            //    AccessCard = employee.IssuedEquipment.AccessCard,
            //    CellPhone = employee.IssuedEquipment.CellPhone,
            //    CompanyCar = employee.IssuedEquipment.CompanyCar,
            //    CreditCard = employee.IssuedEquipment.CreditCard,
            //    Desktop = employee.IssuedEquipment.Desktop,
            //    GasCard = employee.IssuedEquipment.GasCard,
            //    Laptop = employee.IssuedEquipment.Laptop,
            //    Notes = string.Empty,
            //    PPE = employee.IssuedEquipment.PPE,
            //    ShopKeys = employee.IssuedEquipment.ShopKeys,
            //    Uniform = employee.IssuedEquipment.Uniform
            //});

            performanceProfiler.Stop("Get Employee Details");
            return emp;
        }

        private void GetWorkStatusHistory(Employee emp, EmployeeModelHrs employee)
        {
            emp.WorkStatusHistory.Clear();
            if (employee.WorkStatusHistory.Any())
            {
                foreach (var workStatusHistory in employee.WorkStatusHistory.ToList())
                {
                    emp.WorkStatusHistory.Add(new WorkStatusHistory()
                    {
                        ReasonForChange = PropertyBuilder
                            .CreatePropertyValue("WorkHistoryReasonForChange", "Reason why Work History was changed", workStatusHistory.ReasonForChangeType, "")
                            .AsPropertyValueRef(),
                        Comments = workStatusHistory.Comments,
                        EffectiveDate = workStatusHistory.EffectiveDate,
                        FieldChanged = workStatusHistory.FieldChanged,
                        OldValue = workStatusHistory.OldValue,
                        NewValue = workStatusHistory.NewValue
                    });
                }
            }
        }

        [Test]


        private LocationRef GetLocationForHrsLocation(string employeeLocation)
        {
            if (employeeLocation == "Canada")
            {
                employeeLocation = "Edmonton";
            }

            if (employeeLocation == "Emmott")
            {
                employeeLocation = "Emmott Road";
            }

            if ((employeeLocation == "Lafayette/South Bernard") || (employeeLocation == "Lafayette (Deleted)"))
            {
                employeeLocation = "Lafayette";
            }

            employeeLocation = employeeLocation.TrimEnd();
            var rep = new RepositoryBase<Location>();
            var location = rep.AsQueryable().FirstOrDefault(x => x.Office == employeeLocation) ??
                           rep.AsQueryable().First(x => x.Office == "<unknown>");

            return location.AsLocationRef();
        }

        private void BuildRelationships(PerformanceProfiler performanceProfiler)
        {

            if (!_buildRelationships) return;
            performanceProfiler.Start("Build Relationships");

            var rep = new RepositoryBase<Employee>();
            var employeesMissingManager = rep.AsQueryable()
                .Where(x => x.Manager == null && x.OriginalManagerPayrollId != String.Empty).ToList();

            foreach (var employee in employeesMissingManager)
            {
                var manager = rep.AsQueryable().FirstOrDefault(x => x.PayrollId == employee.OriginalManagerPayrollId);
                if (manager != null)
                {
                    employee.Manager = manager.AsEmployeeRef();
                }

                var directReports = rep.AsQueryable().Where(x => x.OriginalManagerPayrollId == employee.PayrollId).ToList().Select(x => x.AsEmployeeRef()).ToList();
                employee.DirectReports = directReports;

                rep.Upsert(employee);
            }
            performanceProfiler.Stop("Build Relationships");

        }

        [Test]
        public void RunSensitizer()
        {
            var rep = new RepositoryBase<Employee>();

            if ((EnvironmentSettings.CurrentEnvironment == Environment.Development) ||
                (EnvironmentSettings.CurrentEnvironment == Environment.QualityControl))
            {
                var employees = rep.AsQueryable().ToList();
                foreach (var employee in employees)
                {
                    var emp = DataSensitizer.Execute(employee);
                    rep.Upsert(emp);
                }
            }

        }

        [Test]
        public void FindDisciplineHistory()
        {
            var employees = new RepositoryBase<Employee>().AsQueryable()
                .Where(x => x.Discipline.Count > 1).ToList();
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.LastName}, {employee.FirstName}");
            }
        }

        [Test]
        public void FindMultipleBonusSchemes()
        {
            var employees = new RepositoryBase<Employee>().AsQueryable()
                .Where(x => x.Compensation.BonusScheme.Count > 1).ToList();
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.LastName}, {employee.FirstName} - Bonus Schemes: {employee.Compensation.BonusScheme.Count}");
            }
        }

        [Test]
        public void LookForPerformance()
        {
            var employees = new RepositoryBase<Employee>().AsQueryable()
                .Where(x => x.Performance.Count > 1).ToList();
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.LastName}, {employee.FirstName}");
            }
        }

        [Test]
        public void QueryTesting()
        {
            var filter = "{ OriginalManagerName: {'$eq': 'Fraser, Stanton'}}.sort( {'LastName': 1} )";
            var rep = new RepositoryBase<Employee>();
            var employees = rep.FindAll(filter);
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.LastName}, {employee.FirstName}");
            }
        }

        [Test]
        public void QueryTestingWithCollection()
        {
            var filter = "{ OriginalManagerName: {'$eq': 'Fraser, Stanton'} }";
            var sort = "{LastName: 1}";
            var collection = new RepositoryBase<Employee>().Collection;
            var employees = collection.Find(filter).Sort(sort).ToList();
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.LastName}, {employee.FirstName}");
            }
        }

        [Test]
        public void QueryTestingWithCollectionAndProjection()
        {
            var filter = "{ OriginalManagerName: {'$eq': 'Fraser, Stanton'} }";
            var sort = "{LastName: 1}";
            var collection = new RepositoryBase<Employee>().Collection;
            var employees = collection.Find(filter).Sort(sort).ToList().Select(x => new { x.LastName, x.FirstName, x.Location.Office }).ToList();
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.LastName}, {employee.FirstName} {employee.Office}");
            }
        }

        [Test]
        public void AddMissingWorkEmails()
        {
            var helper = new MongoRawQueryHelper<Employee>();

            var filter = Employee.Helper.FilterBuilder.Where(x => x.OldHrsId > 0);
            var projection = Employee.Helper.ProjectionBuilder.Expression(x => new { x.EmailAddresses, x.OldHrsId, x.Id });
            var emps = Employee.Helper.FindWithProjection(filter, projection).ToList();

            //var emps = new RepositoryBase<Employee>().AsQueryable().Where(x => x.OldHrsId > 0).ToList();


            using (DAL.HRS.SqlServer.Model.HrsContext context = new DAL.HRS.SqlServer.Model.HrsContext())
            {

                foreach (var employee in context.Employee.Where(e => e.WorkEmail != null && e.WorkEmail.Contains("@")))
                {
                    var Emp = emps.FirstOrDefault(x => x.OldHrsId == employee.OID);

                    if (Emp == null || Emp.EmailAddresses.Where(x => x.EmailType.Code == "Work").Count() > 0)
                        continue;

                    //Emp.EmailAddresses.Add(new EmployeeEmailAddress("Work", employee.WorkEmail));
                    var mEmp = helper.FindById(Emp.Id);
                    mEmp.EmailAddresses.Add(new EmployeeEmailAddress("Work", employee.WorkEmail));
                    helper.Upsert(mEmp);
                }
            }

        }



    }

}
