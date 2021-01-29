using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Models;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;
using DAL.HRS.Mongo.DocClass.Hrs_User;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperRequiredActivity : IHelperRequiredActivity
    {
        private static readonly MongoRawQueryHelper<RequiredActivity> QueryHelperRequiredActivity = new MongoRawQueryHelper<RequiredActivity>();


        //public List<RequiredActivityModel> GetAllRequiredActivitiesDueLessThanSixMonthsFromNow(string status, string type, HrsUser hrsUser)
        public List<RequiredActivityModel> GetAllRequiredActivitiesDueLessThanSixMonthsFromNow(HrsUser hrsUser)
        {
            var result = new List<RequiredActivityModel>();

            var beforeDate = DateTime.Now.Date.AddMonths(6);

            var empQueryHelper = new MongoRawQueryHelper<Employee>();
            var projectionEmployeeId = empQueryHelper.ProjectionBuilder.Expression(x => x.Id.ToString());

            var role = SecurityRoleHelper.GetHrsSecurityRole(hrsUser);
            var employee = hrsUser.Employee.AsEmployee();

            FilterDefinition<Employee> filter = empQueryHelper.FilterBuilder.Empty;

            if (role.DirectReportsOnly)
            {
                filter = EmployeeDirectReportsFilterGenerator.GetDirectReportsOnlyFilter(empQueryHelper, role, employee, filter);
            }
            else
            {
                var locations = hrsUser.HrsSecurity.Locations;
                if (!locations.Any())
                {
                    return result;
                }
                
                filter = EmployeeLocationsFilterGenerator.GetLocationsFilter(empQueryHelper, locations, filter);
            }

            var filteredEmployees = empQueryHelper.FindWithProjection(filter, projectionEmployeeId);


            var queryHelper = new MongoRawQueryHelper<RequiredActivity>();
            var filterRequiredActivity = queryHelper.FilterBuilder.In(x => x.Employee.Id, filteredEmployees);

            FilterDefinition<RequiredActivity> baseFilter = queryHelper.FilterBuilder.Where(x =>
                (x.CompletionDeadline <= beforeDate) || (x.RevisedCompletionDeadline <= beforeDate));

            //FilterDefinition<RequiredActivity> statusFilter = queryHelper.FilterBuilder.Empty;
            //FilterDefinition<RequiredActivity> typeFilter = queryHelper.FilterBuilder.Empty;
            

            //if (status != "all")
            //{
            //    RequiredActivityStatus requiredActivityStatus =
            //        (RequiredActivityStatus)Enum.Parse(typeof(RequiredActivityStatus), status);
            //    statusFilter = queryHelper.FilterBuilder.Where(x => x.Status == requiredActivityStatus);

            //}

            //if (type != "all")
            //{
            //    RequiredActivityType requiredActivityType =
            //        (RequiredActivityType)Enum.Parse(typeof(RequiredActivityType), type);
            //    typeFilter = queryHelper.FilterBuilder.Where(x => x.Type == requiredActivityType);
            //}

           

            var project = queryHelper.ProjectionBuilder.Expression(
                x => new RequiredActivityModel(x));

            //result.AddRange(queryHelper.FindWithProjection(baseFilter & statusFilter & typeFilter & filterRequiredActivity, project));
            result.AddRange(queryHelper.FindWithProjection(baseFilter & filterRequiredActivity, project));

            return result.OrderBy(x => x.CompletionDeadline).ToList();

        }

        public RequiredActivityModel SaveRequiredActivity(RequiredActivityModel model)
        {
            var requiredActivity = RequiredActivity.Helper.FindById(model.Id);
            if (requiredActivity == null) throw new Exception("Required Activity not found");

            requiredActivity.Comments = model.Comments;
            requiredActivity.CompleteStatus = model.CompleteStatus;
            requiredActivity.CompletionDeadline = model.CompletionDeadline;
            requiredActivity.ActivityType = model.ActivityType;
            requiredActivity.DateCompleted = model.DateCompleted;
            requiredActivity.Description = model.Description;
            requiredActivity.RevisedCompletionDeadline = model.RevisedCompletionDeadline;
            requiredActivity.TrainingCourse = model.TrainingCourse;
            requiredActivity.Status = model.Status;
            requiredActivity.Type = model.Type;

            requiredActivity = RequiredActivity.Helper.Upsert(requiredActivity);

            return new RequiredActivityModel(requiredActivity);

        }

        public RequiredActivityModel ChangeRequiredActivityStatus(string requiredActivityId, string status)
        {
            var requiredActivity = RequiredActivity.Helper.FindById(requiredActivityId);
            if (requiredActivity == null) throw new Exception("Required Activity not found");

            var requiredActivityStatus = (RequiredActivityStatus)Enum.Parse(typeof(RequiredActivityStatus), status);
            requiredActivity.Status = requiredActivityStatus;
            RequiredActivity.Helper.Upsert(requiredActivity);

            return new RequiredActivityModel(requiredActivity);
        }


        public void CreateRequiredActivitiesForJobTitle(EmployeeRef employee, JobTitleRef oldJobTitle, JobTitleRef newJobTitle)
        {
            if ((oldJobTitle == null) && (newJobTitle == null)) return;

            JobTitle oldJob = oldJobTitle?.AsJobTitle();
            JobTitle newJob = newJobTitle?.AsJobTitle();

            List<TrainingInitialCourse> trainingCoursesRequired = new List<TrainingInitialCourse>();

            if (newJob != null)
            {
                if (!newJob.TrainingInitialCourses.Any()) return;
                trainingCoursesRequired.AddRange(newJob.TrainingInitialCourses);
            }

            if (oldJob != null)
            {
                foreach (var oldJobTrainingInitialCourse in oldJob.TrainingInitialCourses)
                {
                    var existingCourses = (trainingCoursesRequired.Where(x =>
                         x.TrainingCourse.Id == oldJobTrainingInitialCourse.TrainingCourse.Id)).ToList();

                    foreach (var trainingInitialCourse in existingCourses)
                    {
                        trainingCoursesRequired.Remove(trainingInitialCourse);
                    }
                }
            }
            var helperRequiredActivity = new MongoRawQueryHelper<RequiredActivity>();
            foreach (var trainingInitialCourse in trainingCoursesRequired)
            {
                var trainingCourse = trainingInitialCourse.TrainingCourse.AsTrainingCourse();
                var type = (trainingCourse.ExpirationMonths > 0)
                    ? RequiredActivityType.TrainingCertification
                    : RequiredActivityType.Training;
                var requiredActivity = new RequiredActivity()
                {
                    Status = RequiredActivityStatus.Pending,
                    Type = RequiredActivityType.InitialTraining,
                    CompletionDeadline = DateTime.Now.Date.AddDays(trainingInitialCourse.DaysToComplete),
                    Description = $"{trainingCourse.Name}",
                    Employee = employee,
                    TrainingCourse = trainingCourse.AsTrainingCourseRef()
                };
                helperRequiredActivity.Upsert(requiredActivity);


            }

        }

        //internal void CreateRequiredActivitiesForTrainingEventAttendees(TrainingEvent trainingEvent, List<TrainingAttendee> originalTrainingAttendees, List<TrainingAttendee> currentTrainingAttendees)
        //{
        //    // Ignore past
        //    if (trainingEvent.EndDate <= DateTime.Now.Date) return;

        //    // If we cannot determine when due, ignore
        //    var dueDate = trainingEvent.StartDate ?? trainingEvent.EndDate;
        //    if (dueDate == null) return;

        //    // If we cannot determine TrainingCourse ignore
        //    var trainingCourse = trainingEvent.TrainingCourse.AsTrainingCourse();
        //    if (trainingCourse == null) return;

        //    var type = (trainingCourse.Certification != null)
        //        ? RequiredActivityType.TrainingCertification
        //        : RequiredActivityType.Training;
           

        //    var newAttendees = currentTrainingAttendees
        //        .Where(x => originalTrainingAttendees.All(o => o.Employee.Id != x.Employee.Id)).ToList();
        //    var removedAttendees = originalTrainingAttendees
        //        .Where(x => currentTrainingAttendees.All(o => o.Employee.Id != x.Employee.Id)).ToList();
        //    var updatedAttendees = currentTrainingAttendees
        //        .Where(x => originalTrainingAttendees.All(o => o.Employee.Id == x.Employee.Id
        //        && o.GetHashCode() != x.GetHashCode())).ToList();




        //    // Remove RequiredActivities if they were removed from this TrainingEvent
        //    foreach (var trainingAttendee in removedAttendees)
        //    {
        //        // if not there who cares
        //        if (trainingAttendee.RequiredActivity == null) continue;

        //        var requiredActivity = trainingAttendee.RequiredActivity.AsRequiredActivity();

        //        QueryHelperRequiredActivity.DeleteOne(requiredActivity.Id);
        //    }

        //    // Add new attendees
        //    foreach (var trainingAttendee in newAttendees)
        //    {
        //        AddRequiredActivity(trainingAttendee);
        //    }

        //    // check for updates
        //    foreach (var trainingAttendee in updatedAttendees)
        //    {
        //        RequiredActivity requiredActivity = null;

        //        if (trainingAttendee.RequiredActivity == null)
        //        {
        //            AddRequiredActivity(trainingAttendee);
        //            continue;
        //        }

        //        requiredActivity = trainingAttendee.RequiredActivity.AsRequiredActivity();

        //        if (requiredActivity.Status == RequiredActivityStatus.Pending)
        //        {
        //            var endDate = requiredActivity.RevisedCompletionDeadline ?? requiredActivity.CompletionDeadline;
        //            if (trainingEvent.StartDate != endDate)
        //            {
        //                UpdateRequiredActivity(requiredActivity, trainingAttendee);
        //            }
        //        }
        //    }

        //    foreach (var currentTrainingAttendee in currentTrainingAttendees)
        //    {
        //        if (currentTrainingAttendee.RequiredActivity == null)
        //        {
        //            AddRequiredActivity(currentTrainingAttendee);
        //            continue;
        //        }

        //        var requiredActivity = currentTrainingAttendee.RequiredActivity.AsRequiredActivity();
        //        if (requiredActivity == null)
        //        {
        //            AddRequiredActivity(currentTrainingAttendee);
        //        }
        //        else
        //        {
        //            var deadline = requiredActivity.RevisedCompletionDeadline ?? requiredActivity.CompletionDeadline;
        //            if (deadline != trainingEvent.StartDate)
        //            {
        //                UpdateRequiredActivity(requiredActivity, currentTrainingAttendee);
        //            }
        //        }
        //    }
        //    var helperTrainingEvent = new MongoRawQueryHelper<TrainingEvent>();
        //    helperTrainingEvent.Upsert(trainingEvent);

        //    foreach (var trainingAttendee in currentTrainingAttendees.Where(x => x.DateCompleted != null))
        //    {
        //        var requiredActivity = trainingAttendee.RequiredActivity?.AsRequiredActivity();
        //        if (requiredActivity != null)
        //        {
        //            UpdateRequiredActivity(requiredActivity, trainingAttendee);
        //        }
        //        else
        //        {
        //            AddRequiredActivity(trainingAttendee);
        //        }
        //    }


        //    void AddRequiredActivity(TrainingAttendee trainingAttendee)
        //    {
        //        var requiredActivity = new RequiredActivity()
        //        {
        //            Type = type,
        //            Status = (trainingAttendee.DateCompleted != null) ? RequiredActivityStatus.Completed : RequiredActivityStatus.Pending,
        //            CompletionDeadline = trainingEvent.StartDate,
        //            TrainingCourse = trainingCourse.AsTrainingCourseRef(),
        //            DateCompleted = trainingAttendee.DateCompleted,
        //            Employee = trainingAttendee.Employee,
        //            Description = $"{trainingCourse.Name} Starting: {trainingEvent.StartDate}"
        //        };
        //        if ((type == RequiredActivityType.TrainingCertification) &&
        //            (requiredActivity.Status == RequiredActivityStatus.Completed))
        //        {
        //            AddNextRequiredActivityForCertification(trainingAttendee, requiredActivity);
        //        }


        //        QueryHelperRequiredActivity.Upsert(requiredActivity);
        //        trainingAttendee.RequiredActivity = requiredActivity.AsRequiredActivityRef();
        //    }

        //    void UpdateRequiredActivity(RequiredActivity requiredActivity, TrainingAttendee trainingAttendee)
        //    {
        //        // did they enter or modify the DateCompleted
        //        if (requiredActivity.DateCompleted != trainingAttendee.DateCompleted)
        //        {
        //            // they just completed it
        //            if (requiredActivity.DateCompleted == null)
        //            {
        //                AddNextRequiredActivityForCertification(trainingAttendee, requiredActivity);
        //            }
        //            // they modified the DateCompleted
        //            else
        //            {
        //                UpdateNextRequiredActivityForCertification(trainingAttendee, requiredActivity);
        //            }
        //        }

        //        if (requiredActivity.CompletionDeadline == null)
        //        {
        //            requiredActivity.RevisedCompletionDeadline = trainingEvent.StartDate;
        //        }
        //        else
        //        {
        //            requiredActivity.CompletionDeadline = trainingEvent.StartDate;
        //        }
        //        requiredActivity.DateCompleted = trainingAttendee.DateCompleted;
        //        if ((requiredActivity.DateCompleted != null) &&
        //            (requiredActivity.Status == RequiredActivityStatus.Pending))
        //        {
        //            requiredActivity.Status = RequiredActivityStatus.Completed;
        //        }
        //        QueryHelperRequiredActivity.Upsert(requiredActivity);
        //        trainingAttendee.RequiredActivity = requiredActivity.AsRequiredActivityRef();
        //    }

        //    void AddNextRequiredActivityForCertification(TrainingAttendee trainingAttendee, RequiredActivity requiredActivity)
        //    {
        //        if (trainingCourse.Certification == null) return;
        //        var nextDate = trainingAttendee.DateCompleted ?? DateTime.Now.Date;
        //        nextDate = nextDate.AddMonths(trainingCourse.ExpirationMonths);
        //        var nextRequiredActivity = new RequiredActivity()
        //        {
        //            Employee = trainingAttendee.Employee,
        //            TrainingCourse = trainingCourse.AsTrainingCourseRef(),
        //            CompletionDeadline = nextDate,
        //            Description = $"{trainingCourse.Name} Starting: {trainingEvent.StartDate}",
        //            Type = RequiredActivityType.TrainingCertification,
        //            Status = RequiredActivityStatus.Pending
        //        };
        //        QueryHelperRequiredActivity.Upsert(nextRequiredActivity);
        //        requiredActivity.NextRequiredActivityForCertification = nextRequiredActivity.AsRequiredActivityRef();
        //    }

        //    void UpdateNextRequiredActivityForCertification(TrainingAttendee trainingAttendee, RequiredActivity requiredActivity)
        //    {
        //        if (trainingCourse.Certification == null) return;
        //        var nextDate = trainingAttendee.DateCompleted ?? DateTime.Now.Date;
        //        nextDate = nextDate.AddMonths(trainingCourse.ExpirationMonths);
        //        var nextRequiredActivity = requiredActivity.NextRequiredActivityForCertification.AsRequiredActivity();
        //        nextRequiredActivity.RevisedCompletionDeadline = nextDate;
        //        QueryHelperRequiredActivity.Upsert(nextRequiredActivity);
        //        requiredActivity.NextRequiredActivityForCertification = nextRequiredActivity.AsRequiredActivityRef();
        //    }

        //}


    }
}
