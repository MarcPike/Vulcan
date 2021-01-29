using System;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperTraining : HelperBase, IHelperTraining
    {
        private readonly HelperProperties _helperProperties = new HelperProperties();
        private HelperRequiredActivity _helperRequiredActivities = new HelperRequiredActivity();
        private List<DescendantItem> DescendantCache;

        private List<EmployeeRef> DescendantsList = new List<EmployeeRef>();
        private Dictionary<ObjectId, List<EmployeeRef>> EmployeesCache;


        public TrainingCourseDeActivateListModel GetTrainingCourseDeActivateListModel(HrsUserRef hrsUser)
        {
            return new TrainingCourseDeActivateListModel
            {
                UpdateUser = hrsUser
            };
        }


        public List<TrainingCourseDeActivateLog> DeActivateTrainingCourses(TrainingCourseDeActivateListModel model)
        {
            var result = new List<TrainingCourseDeActivateLog>();
            foreach (var courseId in model.TrainingCourseIdList)
                result.Add(DeActivateTrainingCourse(courseId, model.UpdateUser));

            return result;
        }

        public TrainingCourseDeActivateLog DeActivateTrainingCourse(string trainingCourseId, HrsUserRef hrsUserRef)
        {
            var trainingCourse = TrainingCourse.Helper.FindById(trainingCourseId);
            if (trainingCourse == null) throw new Exception("Training Course not found");

            return new TrainingCourseDeActivateLog(trainingCourse, hrsUserRef);
        }


        public List<TrainingCourseDeActivateLog> GetDeactivatedTrainingCoursesHistory()
        {
            return TrainingCourseDeActivateLog.Helper.GetAll();
        }

        public List<TrainingCourseModel> GetTrainingCourses()
        {
            return new RepositoryBase<TrainingCourse>().AsQueryable().Where(x => x.IsActive).OrderBy(x => x.Name)
                .ToList().Select(x => new TrainingCourseModel(x)).ToList();
        }

        public List<TrainingCourseModel> GetTrainingCoursesForLocation(string locationId)
        {
            return new RepositoryBase<TrainingCourse>().AsQueryable()
                .Where(x => x.IsActive && x.Locations.Any(l => l.Id == locationId)).OrderBy(x => x.Name).ToList()
                .Select(x => new TrainingCourseModel(x)).ToList();
        }

        public List<TrainingEventGridModel> GetAllTrainingEvents(HrsUser hrsUser)
        {
            if (hrsUser == null) throw new Exception("HrsUser not found");


            var queryHelper = new MongoRawQueryHelper<TrainingEvent>();
            var locations = hrsUser.HrsSecurity.Locations.Select(x => x.Id).ToList();
            var filter = queryHelper.FilterBuilder.Empty;

            if (locations.Count > 0) filter = queryHelper.FilterBuilder.In(x => x.Location.Id, locations);


            var project = queryHelper.ProjectionBuilder.Expression(
                x => new
                {
                    x.Id, x.Location, AttendeeCount = x.Attendees.Count, x.Cost, x.StartDate, x.EndDate,
                    x.TrainingCourse, x.InternalInstructor, x.ExternalInstructor, x.CertificateExpiration
                });

            return queryHelper.FindWithProjection(filter, project).Select(x =>
                new
                    TrainingEventGridModel
                    {
                        Id = x.Id.ToString(),
                        AttendeeCount = x.AttendeeCount,
                        CertificateExpiration = x.CertificateExpiration,
                        Cost = x.Cost,
                        TrainingCourse = x.TrainingCourse,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        InternalInstructor = x.InternalInstructor,
                        ExternalInstructor = x.ExternalInstructor,
                        Location = x.Location
                    }).OrderByDescending(x => x.StartDate).ToList();


            //return queryHelper.FindWithProjection(filter, project).OrderByDescending(x=>x.StartDate).ThenBy(x=>x.TrainingCourse.Name).ToList();
        }

        public List<TrainingEventModel> GetTrainingEventsForLocation(string locationId)
        {
            return new RepositoryBase<TrainingEvent>().AsQueryable().ToList().Where(x => x.Location.Id == locationId)
                .Select(x => new TrainingEventModel(x))
                .OrderByDescending(x => x.StartDate).ThenBy(x => x.TrainingCourse.Name).ToList();
        }


        public List<TrainingEventModel> GetTrainingEventsForTrainingCourse(string trainingCourseId)
        {
            var trainingEvents = new RepositoryBase<TrainingEvent>().AsQueryable()
                .Where(x => x.TrainingCourse.Id == trainingCourseId).ToList().Select(x => new TrainingEventModel(x));
            return trainingEvents.OrderByDescending(x => x.StartDate).ThenBy(x => x.TrainingCourse.Name).ToList();
        }

        public List<TrainingEventModel> GetTrainingEventsForEmployee(string employeeId)
        {
            var helperEmployee = new HelperEmployee();
            var emp = helperEmployee.GetEmployee(employeeId);
            if (emp == null) throw new Exception("Employee not found");

            var deactivatedTraining = GetDeactivatedTrainingCoursesHistory();

            var result = new List<TrainingEventModel>();
            var eventRemoved = false;
            foreach (var empTrainingEvent in emp.TrainingEvents.ToList())
            {
                var trainingEvent = empTrainingEvent.AsTrainingEvent();
                if (trainingEvent != null)
                {
                    var deactivedCourse =
                        deactivatedTraining.SingleOrDefault(x =>
                            x.TrainingCourse.Id == trainingEvent.TrainingCourse.Id);

                    if (deactivedCourse != null)
                    {
                        var attended = trainingEvent.Attendees.SingleOrDefault(x => x.Employee.Id == employeeId);

                        if (attended != null && attended.DateCompleted == null)
                            continue;
                    }


                    result.Add(new TrainingEventModel(trainingEvent));
                }
                else
                {
                    emp.TrainingEvents.Remove(empTrainingEvent);
                    eventRemoved = true;
                }
            }

            if (eventRemoved) new MongoRawQueryHelper<Employee>().Upsert(emp);

            return result.OrderByDescending(x => x.StartDate).ThenBy(x => x.TrainingCourse.Name).ToList();
        }

        public List<EmployeeTrainingEventGridModel> GetAllTrainingEventsForEmployeesGrid(HrsUser hrsUser)
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();

            EmployeesCache = new Dictionary<ObjectId, List<EmployeeRef>>();
            var filterActive = queryHelper.FilterBuilder.Empty;
            //if (isActive)
            //{
            //    filterActive = queryHelper.FilterBuilder.Where(x =>
            //        x.TerminationDate == null || x.TerminationDate > DateTime.Now);
            //}
            //else
            //{
            //    filterActive = queryHelper.FilterBuilder.Where(x =>
            //        x.TerminationDate != null);
            //}


            var hasDirectReports = false;

            if (hrsUser != null)
            {
                var hrsSecurityRole = hrsUser.HrsSecurity.GetRole();
                if (hrsSecurityRole != null) hasDirectReports = hrsSecurityRole.DirectReportsOnly;


                var hseSecurityRole = hrsUser.HseSecurity.GetRole();
                if (hrsSecurityRole == null && hseSecurityRole != null)
                    hasDirectReports = hseSecurityRole.DirectReportsOnly;
            }

            var filter = queryHelper.FilterBuilder.Empty;
            var locations = hrsUser.HrsSecurity.Locations;


            //FilterDefinition<Employee> filterDirectReports = queryHelper.FilterBuilder.Empty;
            if (hasDirectReports)
            {
                //filter = queryHelper.FilterBuilder.Where(x => x.Manager.Id == hrsUser.Employee.Id.ToString());


                var empFilter = Employee.Helper.FilterBuilder.Empty;
                var drProjection = Employee.Helper.ProjectionBuilder.Expression(x => new {x.Id, x.DirectReports});

                foreach (var emp in Employee.Helper.FindWithProjection(empFilter, drProjection))
                    EmployeesCache.Add(emp.Id, emp.DirectReports);

                var empId = new ObjectId(hrsUser.Employee.Id);
                getAllDirectReportsAndDescendants(empId);

                var descendantIDs = DescendantsList.Select(x => new ObjectId(x.Id)).ToList();

                filter = queryHelper.FilterBuilder.Where(x => descendantIDs.Contains(x.Id));
            }
            else
            {
                if (!locations.Any()) return new List<EmployeeTrainingEventGridModel>();
                filter = EmployeeLocationsFilterGenerator.GetLocationsFilter(queryHelper, locations, filter);
            }


            var projection = queryHelper.ProjectionBuilder.Expression(row => new EmployeeTrainingEventGridModel
                {
                    Birthday = _encryption.Decrypt<DateTime>(row.Birthday),
                    BusinessRegionCode = row.BusinessRegionCode,
                    CostCenterCode = row.CostCenterCode,
                    EmployeeId = row.Id.ToString(),
                    FirstName = row.FirstName,
                    GenderCode = row.GenderCode,
                    GovernmentId = row.GovernmentId == null
                        ? string.Empty
                        : _encryption.Decrypt<string>(row.GovernmentId),
                    OriginalHireDate = row.OriginalHireDate,
                    TerminationDate = row.TerminationDate,
                    JobTitle = row.JobTitle,
                    KronosDepartmentCode = row.KronosDepartmentCode,
                    LastName = row.LastName,
                    Location = row.Location,
                    Manager = row.Manager == null ? null : row.Manager.Refresh(),
                    MiddleName = row.MiddleName,
                    PayrollId = row.PayrollId,
                    PayrollRegion = row.PayrollRegion,
                    PreferredName = row.PreferredName,
                    Status1Code = row.Status1Code,
                    IsActive = row.TerminationDate == null || row.TerminationDate > DateTime.Now,
                    TrainingEventsCount = row.TrainingEvents.Count
                }
            );

            return queryHelper.FindWithProjection(filter, projection).ToList();


            //if (hasTraining)
            //{
            //    return queryHelper.FindWithProjection(filterActive, projection).Where(x => x.TrainingEventsCount > 0).ToList();
            //}
            //else
            //{
            //    return queryHelper.FindWithProjection(filterActive, projection).Where(x => x.TrainingEventsCount == 0).ToList();
            //}
        }


        public TrainingCourseModel GetNewTrainingCourseModel()
        {
            return new TrainingCourseModel(new TrainingCourse());
        }

        public TrainingCourseModel SaveTrainingCourse(TrainingCourseModel model)
        {
            var rep = new RepositoryBase<TrainingCourse>();
            var id = ObjectId.Parse(model.Id);
            var trainingCourse = rep.AsQueryable().FirstOrDefault(x => x.Id == id) ?? new TrainingCourse
            {
                Id = id
            };

            trainingCourse.VenueType = model.VenueType;
            trainingCourse.Description = model.Description;
            trainingCourse.ExpirationMonths = model.ExpirationMonths;
            trainingCourse.GroupClassification = model.GroupClassification;
            trainingCourse.CourseType = model.CourseType;
            trainingCourse.Name = model.Name;
            trainingCourse.Locations = model.Locations;

            rep.Upsert(trainingCourse);
            return new TrainingCourseModel(trainingCourse);
        }

        public TrainingEventModel GetNewTrainingEventModel()
        {
            return new TrainingEventModel(new TrainingEvent());
        }

        public TrainingEventModel GetTrainingEvent(string trainingEventId)
        {
            var rep = new RepositoryBase<TrainingEvent>();
            var id = ObjectId.Parse(trainingEventId);
            var trainingEvent = rep.AsQueryable().FirstOrDefault(x => x.Id == id);
            if (trainingEvent == null) throw new Exception("Training Event not found");

            return new TrainingEventModel(trainingEvent);
        }

        public void RemoveTrainingEvent(string trainingEventId)
        {
            var helperRequiredActivity = new MongoRawQueryHelper<RequiredActivity>();
            var helperEmployee = new MongoRawQueryHelper<Employee>();
            var helperTrainingEvent = new MongoRawQueryHelper<TrainingEvent>();
            var trainingEvent = helperTrainingEvent.FindById(trainingEventId);
            if (trainingEvent == null) throw new Exception("Training event not found");

            foreach (var trainingAttendee in trainingEvent.Attendees)
            {
                var employee = trainingAttendee.Employee.AsEmployee();
                var removeTrainingEvent = employee.TrainingEvents.FirstOrDefault(x => x.Id == trainingEventId);
                if (removeTrainingEvent != null)
                {
                    employee.TrainingEvents.Remove(removeTrainingEvent);
                    helperEmployee.Upsert(employee);
                }

                if (trainingAttendee.RequiredActivity != null)
                {
                    var requiredActivity = trainingAttendee.RequiredActivity.AsRequiredActivity();
                    if (requiredActivity != null) helperRequiredActivity.DeleteOne(requiredActivity.Id);
                }

                helperTrainingEvent.DeleteOne(trainingEventId);
            }
        }

        public TrainingEventModel SaveTrainingEvent(TrainingEventModel model)
        {
            var rep = new RepositoryBase<TrainingEvent>();
            var id = ObjectId.Parse(model.Id);

            if (model.TrainingCourse == null) throw new Exception("Missing Training Course");
            var trainingCourse = new MongoRawQueryHelper<TrainingCourse>().FindById(model.TrainingCourse.Id);
            if (trainingCourse == null) throw new Exception("Training course not found");

            var trainingEvent = rep.AsQueryable().FirstOrDefault(x => x.Id == id) ?? new TrainingEvent
            {
                Id = id
            };

            var originalTrainingAttendees = trainingEvent.Attendees;
            var currentTrainingAttendees = model.Attendees;

            trainingEvent.Attendees = model.Attendees;
            foreach (var attendee in trainingEvent.Attendees) attendee.Employee?.Refresh();

            trainingEvent.TrainingHours = model.TrainingHours;
            trainingEvent.ExternalInstructor = model.ExternalInstructor;
            trainingEvent.InternalInstructor = model.InternalInstructor;
            trainingEvent.StartDate = model.StartDate?.Date;
            trainingEvent.EndDate = model.EndDate?.Date;
            trainingEvent.TrainingCourse = model.TrainingCourse;
            trainingEvent.CertificateExpiration = model.CertificateExpiration?.Date;
            trainingEvent.Cost = model.Cost;
            trainingEvent.Location = model.Location;

            //_helperRequiredActivity.SetRequiredDashboardItemsForThisTrainingEvent(trainingEvent);

            rep.Upsert(trainingEvent);

            //Required Activity should not be created from an Event - Per Denise 11/18/2020
            //_helperRequiredActivities.CreateRequiredActivitiesForTrainingEventAttendees(trainingEvent,
            //    originalTrainingAttendees, currentTrainingAttendees);

            // Save to Employee.TrainingEvents for each Attendee
            var trainingEventRef = trainingEvent.AsTrainingEventRef();
            foreach (var trainingAttendee in model.Attendees)
            {
                var employee = trainingAttendee.Employee.AsEmployee();
                if (employee.TrainingEvents.All(x => x.Id != trainingEventRef.Id))
                {
                    employee.TrainingEvents.Add(trainingEventRef);
                    employee.SaveToDatabase();
                }
            }

            // Remove from Employee.TrainingEvents if Attendee is not longer an attendee
            foreach (var originalTrainingAttendee in originalTrainingAttendees)
                // if attendee is removed
                if (currentTrainingAttendees.All(x => x.Employee.Id != originalTrainingAttendee.Employee.Id))
                {
                    var employee = originalTrainingAttendee.Employee.AsEmployee();
                    var trainingEventToRemove =
                        employee.TrainingEvents.FirstOrDefault(x => x.Id == trainingEvent.Id.ToString());
                    if (trainingEventToRemove != null)
                    {
                        employee.TrainingEvents.Remove(trainingEventToRemove);
                        employee.SaveToDatabase();
                    }
                }

            return new TrainingEventModel(trainingEvent);
        }

        public TrainingAttendeeModel GetNewTrainingAttendeeModel(string employeeId)
        {
            return new TrainingAttendeeModel(employeeId);
        }

        public List<PropertyValueModel> GetGroupClassifications(string entityId)
        {
            return _helperProperties.GetPropertyValuesForProperty("TrainingGroupClassification", entityId);
        }

        public List<PropertyValueModel> GetGroupCourseTypes(string entityId)
        {
            return _helperProperties.GetPropertyValuesForProperty("TrainingCourseType", entityId);
        }

        public List<PropertyValueModel> GetVenueTypes(string entityId)
        {
            return _helperProperties.GetPropertyValuesForProperty("TrainingVenueType", entityId);
        }

        public List<RequiredActivityModel> GetRequiredActivitiesForEmployee(string employeeId)
        {
            var helperEmployee = new HelperEmployee();
            var emp = helperEmployee.GetEmployee(employeeId);
            if (emp == null) throw new Exception("Employee not found");

            var result = new List<RequiredActivityModel>();
            var employeeRef = emp.AsEmployeeRef();

            var queryHelper = new MongoRawQueryHelper<RequiredActivity>();
            var filter =
                queryHelper.FilterBuilder.Where(x => x.Employee.Id == employeeRef.Id && x.TrainingCourse != null);
            var sort = queryHelper.SortBuilder.Descending(x => x.CompletionDeadline);
            var requiredActivities = queryHelper.FindWithSort(filter, sort);

            var deactivatedTraining = GetDeactivatedTrainingCoursesHistory();

            foreach (var activity in requiredActivities)
            {
                var deactiveTraining =
                    deactivatedTraining.SingleOrDefault(x => x.TrainingCourse.Id == activity.TrainingCourse.Id);

                if (deactiveTraining != null && activity.DateCompleted == null)
                    continue;

                result.Add(new RequiredActivityModel(activity));
            }

            return result.OrderByDescending(x => x.CompletionDeadline).ToList();
        }

        public List<PropertyValueModel> GetRequiredActivityTypes(string entityId)
        {
            return _helperProperties.GetPropertyValuesForProperty("RequiredActivityType", entityId);
        }

        public List<PropertyValueModel> GetRequiredActivityCompleteStatusTypes(string entityId)
        {
            return _helperProperties.GetPropertyValuesForProperty("RequiredActivityCompleteStatus", entityId);
        }

        public RequiredActivityModel GetNewRequiredActivityModelForEmployee(string employeeId)
        {
            var employee = new RepositoryBase<Employee>().Find(employeeId);
            if (employee == null) throw new Exception("Employee not found");
            var requiredActivity = new RequiredActivity {Employee = employee.AsEmployeeRef()};
            var result = new RequiredActivityModel(requiredActivity) {IsDirty = true};
            return result;
        }

        public RequiredActivityModel GetRequiredActivityModel(string requiredActivityId)
        {
            var requiredActivity = new RepositoryBase<RequiredActivity>().Find(requiredActivityId);
            return new RequiredActivityModel(requiredActivity);
        }

        public RequiredActivityModel SaveRequiredActivity(RequiredActivityModel model)
        {
            var queryHelper = new MongoRawQueryHelper<RequiredActivity>();
            var queryHelperSupportingDocs = new MongoRawQueryHelper<SupportingDocument>();
            var requiredActivity = queryHelper.FindById(model.Id);
            if (requiredActivity == null)
                requiredActivity = new RequiredActivity
                {
                    Id = ObjectId.Parse(model.Id)
                };

            requiredActivity.Employee = model.Employee;
            requiredActivity.ActivityType = model.ActivityType;
            requiredActivity.Comments = model.Comments;
            requiredActivity.CompleteStatus = model.CompleteStatus;
            requiredActivity.CompletionDeadline = model.CompletionDeadline?.Date;
            requiredActivity.DateCompleted = model.DateCompleted?.Date;
            requiredActivity.Description = model.Description;

            var filterSupportingDocs =
                queryHelperSupportingDocs.FilterBuilder.Where(x => x.BaseDocumentId == ObjectId.Parse(model.Id));
            requiredActivity.HasSupportingDocs = queryHelperSupportingDocs.Find(filterSupportingDocs).Any();
            requiredActivity.RevisedCompletionDeadline = model.RevisedCompletionDeadline?.Date;
            requiredActivity.TrainingCourse = model.TrainingCourse;
            queryHelper.Upsert(requiredActivity);
            return new RequiredActivityModel(requiredActivity);
        }

        public void RemoveRequiredActivity(string requiredActivityId)
        {
            var rep = new RepositoryBase<RequiredActivity>();
            var requiredActivity = rep.Find(requiredActivityId);
            if (requiredActivity != null) rep.RemoveOne(requiredActivity);
        }

        public List<TrainingEventSupportingDocumentNestedModel> GetTrainingEventSupportingDocumentsNested()
        {
            var queryHelperTraining = new MongoRawQueryHelper<TrainingEvent>();
            var queryHelperDocs = new MongoRawQueryHelper<SupportingDocument>();

            var supportDocFilter =
                queryHelperDocs.FilterBuilder.Where(x => x.Module.Name == "Training");
            var supportDocProject = queryHelperDocs.ProjectionBuilder.Expression(
                x => new SupportingDocumentModel
                {
                    SupportingDocumentId = x.Id.ToString(),
                    BaseDocumentId = x.BaseDocumentId.ToString(),
                    Module = x.Module,
                    Comments = x.Comments,
                    DocumentDate = x.DocumentDate,
                    DocumentType = x.DocumentType,
                    FileName = x.FileName,
                    FileSize = x.FileSize,
                    FileCreateDate = x.FileCreateDate,
                    FileId = x.FileInfo.Id.ToString()
                });
            var supportingDocs = queryHelperDocs.FindWithProjection(supportDocFilter, supportDocProject);

            var trainingFilter =
                queryHelperTraining.FilterBuilder.In(x => x.Id,
                    supportingDocs.Select(d => ObjectId.Parse(d.BaseDocumentId)));
            var trainingProject = queryHelperTraining.ProjectionBuilder.Expression(
                trainingEvent => new TrainingEventSmallModel
                {
                    Id = trainingEvent.Id.ToString(),
                    OldHrsId = trainingEvent.OldHrsId,
                    StartDate = trainingEvent.StartDate,
                    EndDate = trainingEvent.EndDate,
                    CertificateExpiration = trainingEvent.CertificateExpiration,
                    Cost = trainingEvent.Cost,
                    TrainingCourse = trainingEvent.TrainingCourse,
                    AttendeesCount = trainingEvent.Attendees.Count,
                    Location = trainingEvent.Location,
                    InternalInstructor = trainingEvent.InternalInstructor,
                    ExternalInstructor = trainingEvent.ExternalInstructor
                });
            var trainingEvents = queryHelperTraining.FindWithProjection(trainingFilter, trainingProject);


            return trainingEvents.Select(x =>
                new TrainingEventSupportingDocumentNestedModel(x,
                    supportingDocs.Where(s => s.BaseDocumentId == x.Id).ToList())).ToList();
        }

        public List<TrainingEventSupportingDocumentFlatModel> GetTrainingEventSupportingDocumentsFlat()
        {
            var results = GetTrainingEventSupportingDocumentsNested();
            return TrainingEventSupportingDocumentFlatModel.ConvertNestedToFlat(results);
        }

        public List<TrainingCourseRef> GetTrainingCourseReferences()
        {
            var queryHelper = new MongoRawQueryHelper<TrainingCourse>();
            var filter = queryHelper.FilterBuilder.Empty;
            var project = queryHelper.ProjectionBuilder.Expression(x => new TrainingCourseRef
            {
                Id = x.Id.ToString(),
                CourseType = x.CourseType,
                Description = x.Description,
                Name = x.Name,
                ExpirationMonths = x.ExpirationMonths,
                GroupClassification = x.GroupClassification,
                VenueType = x.VenueType
            });
            return queryHelper.FindWithProjection(filter, project).OrderBy(x => x.Name).ToList();
        }

        public List<TrainingCourseRef> GetTrainingCourseReferencesForLocation(string locationId)
        {
            var queryHelper = new MongoRawQueryHelper<TrainingCourse>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Locations.Any(l => l.Id == locationId));
            var project = queryHelper.ProjectionBuilder.Expression(x => new TrainingCourseRef
            {
                Id = x.Id.ToString(),
                CourseType = x.CourseType,
                Description = x.Description,
                Name = x.Name,
                ExpirationMonths = x.ExpirationMonths,
                GroupClassification = x.GroupClassification,
                VenueType = x.VenueType
            });
            return queryHelper.FindWithProjection(filter, project).OrderBy(x => x.Name).ToList();
        }

        public TrainingCourseModel GetTrainingCourse(string courseId)
        {
            var queryHelper = new MongoRawQueryHelper<TrainingCourse>();
            var trainingCourse = queryHelper.FindById(courseId);
            if (trainingCourse == null) throw new Exception("TrainingCourse not found");
            return new TrainingCourseModel(trainingCourse);
        }

        public JobTitleCourseCopyModel GetModelForJobTitleCourseCopy(string sourceId, string targetId)
        {
            var source = JobTitle.Helper.FindById(sourceId);
            var target = JobTitle.Helper.FindById(targetId);

            if (source == null) throw new Exception("Source not found");
            if (target == null) throw new Exception("Target not found");

            return new JobTitleCourseCopyModel(source, target);
        }


        public void CopyJobTitleCourses(JobTitleCourseCopyModel model)
        {
            var target = model.Target.AsJobTitle();
            foreach (var copyCourse in model.TrainingCoursesToCopy.Where(x => x.Checked))
                if (target.TrainingInitialCourses.All(x => x.Id != copyCourse.TrainingCourse.Id))
                    target.TrainingInitialCourses.Add(copyCourse.TrainingCourse);

            JobTitle.Helper.Upsert(target);
        }

        public JobTitleModel CopyAllJobTitleCourses(string sourceId, string targetId)
        {
            var source = JobTitle.Helper.FindById(sourceId);
            var target = JobTitle.Helper.FindById(targetId);

            if (source == null) throw new Exception("Source not found");
            if (target == null) throw new Exception("Target not found");

            foreach (var course in source.TrainingInitialCourses)
                if (target.TrainingInitialCourses.All(x => x.Id != course.Id))
                    target.TrainingInitialCourses.Add(course);

            JobTitle.Helper.Upsert(target);
            return new JobTitleModel(target);
        }

        public List<TrainingEventAttendee> GetAllExpiringEvents(List<string> empIdList)
        {
            var result = new List<TrainingEventAttendee>();
            var eventList = new List<TrainingEventModel>();


            var queryHelper = new MongoRawQueryHelper<TrainingEvent>();

            //var filterEvents = queryHelper.FilterBuilder.Where(x => x.Attendees.Any());
            var fromDate = DateTime.Now.Date;
            var toDate = DateTime.Now.AddMonths(3);


            // Stage1
            // All [Hr] and [Hr and HSE] Group Classifications that have a CertificateExpiration Expiring Today through 6 months forward

            var trainingEventsFilterStage1 = TrainingEvent.Helper.FilterBuilder
                                                 .Where(x =>
                                                     x.TrainingCourse.GroupClassification.Code == "HR and HSE" ||
                                                     x.TrainingCourse.GroupClassification.Code == "HR") &
                                             TrainingEvent.Helper.FilterBuilder.Gte(x => x.CertificateExpiration,
                                                 DateTime.Now.Date) &
                                             TrainingEvent.Helper.FilterBuilder.Lte(x => x.CertificateExpiration,
                                                 DateTime.Now.Date.AddDays(90));

            var trainingProject = queryHelper.ProjectionBuilder.Expression(
                x => new TrainingEventModel(x));

            eventList = queryHelper.FindWithProjection(trainingEventsFilterStage1, trainingProject).ToList();

            foreach (var empEvent in eventList)
            {
                var attendees = empEvent.Attendees.Where(x => empIdList.Contains(x.Employee.Id)).Distinct().ToList();
                if (attendees.Count == 0) continue;

                var dueDate = empEvent.CertificateExpiration ?? empEvent.EndDate;

                foreach (var emp in attendees)
                    //var empLocationFilter = Employee.Helper.FilterBuilder.Where(x => x.Id == ObjectId.Parse(emp.Employee.Id));
                    //var empLocationProject = Employee.Helper.ProjectionBuilder.Expression(x => x.Location);

                    //var empLocation = Employee.Helper.FindWithProjection(empLocationFilter, empLocationProject)
                    //    .FirstOrDefault();

                    result.Add(new TrainingEventAttendee
                    {
                        Id = emp.Id,
                        AttendeeName = $"{emp.Employee.LastName}, {emp.Employee.FirstName}",
                        EventName = empEvent.TrainingCourse.Name,
                        EventDescription = empEvent.TrainingCourse.Description,
                        LocationOffice = empEvent?.Location?.Office ?? emp.Employee.Location.Office,
                        DueDate = dueDate ?? DateTime.MinValue,
                        PayrollId = emp.Employee.PayrollId,
                        GroupClassification = empEvent.TrainingCourse.GroupClassification
                    });
            }

            // Stage 2
            // TrainingEvents not completed with expected completion dates 6 months forward
            var trainingEventsFilterStage2 =
                TrainingEvent.Helper.FilterBuilder.Lte(x => x.EndDate, DateTime.Now.Date.AddMonths(6))
                &
                TrainingEvent.Helper.FilterBuilder.Gte(x => x.EndDate, DateTime.Now.Date.AddMonths(-24))
                &
                TrainingEvent.Helper.FilterBuilder.Where(x => x.Attendees.Any(a => empIdList.Contains(a.Employee.Id)));

            eventList = TrainingEvent.Helper.FindWithProjection(trainingEventsFilterStage2, trainingProject)
                .ToList();

            //eventList = eventList.Where(x => x.Attendees.Any(a => empIdList.Contains(a.Employee.Id))).ToList();

            foreach (var empEvent in eventList)
            {
                var attendees = empEvent.Attendees;
                if (attendees.Count == 0) continue;

                foreach (var attendee in attendees.Where(x => empIdList.Contains(x.Employee.Id)))
                {
                    if (attendee.Dismissed) continue;

                    if (attendee.DateCompleted != null) continue;

                    result.Add(new TrainingEventAttendee
                    {
                        Id = attendee.Id,
                        AttendeeName = $"{attendee.Employee.LastName}, {attendee.Employee.FirstName}",
                        EventName = empEvent.TrainingCourse.Name,
                        EventDescription = empEvent.TrainingCourse.Description,
                        LocationOffice = empEvent?.Location?.Office ?? attendee.Employee.Location.Office,
                        DueDate = empEvent.EndDate.Value.Date,
                        PayrollId = attendee.Employee.PayrollId,
                        GroupClassification = empEvent.TrainingCourse.GroupClassification
                    });
                }

                //    if (emp.DateCompleted == null)
                //        {

                //            if (empEvent.CertificateExpiration != null)
                //            {
                //                newExpirationDate = empEvent.CertificateExpiration ?? DateTime.MinValue;
                //            }


                //            result.Add(new TrainingEventAttendee
                //            {
                //                Id = emp.Id,
                //                AttendeeName = $"{emp.Employee.LastName}, {emp.Employee.FirstName}",
                //                EventName = empEvent.TrainingCourse.Name,
                //                EventDescription = empEvent.TrainingCourse.Description,
                //                LocationOffice = emp.Employee.Location.Office,
                //                DueDate = newExpirationDate,
                //                PayrollId = emp.Employee.PayrollId,
                //                GroupClassification = empEvent.TrainingCourse.GroupClassification,
                //            }); 
                //        }

                //    }
                //}
                //else if (empEvent.CertificateExpiration == null)
                //{
                //    foreach (var emp in attendees)
                //    {
                //        if ((emp.DateCompleted == null) && (empEvent.EndDate != null))
                //        {
                //            newExpirationDate = empEvent.EndDate?.Date ?? DateTime.MinValue;

                //            result.Add(new TrainingEventAttendee
                //            {
                //                Id = emp.Id,
                //                AttendeeName = $"{emp.Employee.LastName}, {emp.Employee.FirstName}",
                //                EventName = empEvent.TrainingCourse.Name,
                //                EventDescription = empEvent.TrainingCourse.Description,
                //                LocationOffice = emp.Employee.Location.Office,
                //                DueDate = newExpirationDate,
                //                PayrollId = emp.Employee.PayrollId,
                //                GroupClassification = empEvent.TrainingCourse.GroupClassification,
                //            });
                //        }
                //    }
                //}
            }

            return result.OrderBy(x => x.DueDate).ToList();
        }

        public void getAllDirectReportsAndDescendants(ObjectId empId)
        {
            //Using recursion caused a Stack Overflow error so, I have this code.....


            DescendantCache = new List<DescendantItem> {new DescendantItem {Id = empId, Status = true}};


            if (EmployeesCache.ContainsKey(empId) && EmployeesCache[empId] != null && EmployeesCache[empId].Count() > 0)
            {
                // manager - level 1
                foreach (var dr in EmployeesCache[empId])
                    DescendantCache.Add(new DescendantItem {Id = new ObjectId(dr.Id), EmployeeRef = dr});

                var notAdded = DescendantCache.Where(x => !x.Status && x.EmployeeRef != null).ToList();


                // check direct report for dr - levels 2 - x
                while (notAdded.Any())
                {
                    foreach (var n in notAdded)
                    {
                        n.Status = true; // mark n as complete

                        if (EmployeesCache.ContainsKey(n.Id) && EmployeesCache[n.Id] != null &&
                            EmployeesCache[n.Id].Count() > 0)
                            foreach (var dr in EmployeesCache[n.Id])
                            {
                                var drId = new ObjectId(dr.Id);
                                var existing = DescendantCache.FirstOrDefault(x => x.Id == drId);

                                if (existing == null)
                                    DescendantCache.Add(new DescendantItem
                                        {Id = new ObjectId(dr.Id), EmployeeRef = dr});
                            }
                    }

                    notAdded = DescendantCache.Where(x => !x.Status).ToList();
                }

                DescendantsList = DescendantCache
                    .Where(x => x.EmployeeRef != null)
                    .Select(x => x.EmployeeRef)
                    .ToList();
            }
        }

        public class DescendantItem
        {
            public ObjectId Id { get; set; }
            public bool Status { get; set; }
            public EmployeeRef EmployeeRef { get; set; }
        }
    }
}