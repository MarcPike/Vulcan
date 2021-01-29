using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Driver;
using DAL.HRS.Mongo.Helpers;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class RequiredActivitiesQuery
    {
        static List<EmployeeRef> DescendantsList = new List<EmployeeRef>();
        static Dictionary<ObjectId, List<EmployeeRef>> EmployeesCache1 = new Dictionary<ObjectId, List<EmployeeRef>>();

        public static List<RequiredActivityModel> GetAllForActiveEmployees(HrsUser hrsUser)
        {
            // Get Active Employee Ids
            var queryHelperEmployee = new MongoRawQueryHelper<Employee>();
            var filterActive = queryHelperEmployee.FilterBuilder.Where(x =>
                (x.TerminationDate == null) || (x.TerminationDate >= DateTime.Now));
            var projectionEmployeeId = queryHelperEmployee.ProjectionBuilder.Expression(x =>
                x.Id.ToString() );

            // Filter if user has direct reports
            var hasDirectReports = false;

            if (hrsUser != null)
            {
                var hrsSecurityRole = hrsUser.HrsSecurity.GetRole();
                if (hrsSecurityRole != null) hasDirectReports = hrsSecurityRole.DirectReportsOnly;


                var hseSecurityRole = hrsUser.HseSecurity.GetRole();
                if (hrsSecurityRole == null && hseSecurityRole != null)
                    hasDirectReports = hseSecurityRole.DirectReportsOnly;
            }

            var filter = queryHelperEmployee.FilterBuilder.Empty;
            var locations = hrsUser.HrsSecurity.Locations;
            var queryHelper = new MongoRawQueryHelper<Employee>();

            //FilterDefinition<Employee> filterDirectReports = queryHelper.FilterBuilder.Empty;
            if (hasDirectReports)
            {
                //filter = queryHelperEmployee.FilterBuilder.Where(x => x.Manager.Id == hrsUser.Employee.Id.ToString());

                var empFilter = Employee.Helper.FilterBuilder.Empty;
                var drProjection = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.DirectReports });

                foreach (var emp in Employee.Helper.FindWithProjection(empFilter, drProjection))
                    EmployeesCache1.Add(emp.Id, emp.DirectReports);

                var empId = new ObjectId(hrsUser.Employee.Id);
                getAllDirectReportsAndDescendants(empId);

                var descendantIDs = DescendantsList.Select(x => new ObjectId(x.Id)).ToList();

                filter = queryHelper.FilterBuilder.Where(x => descendantIDs.Contains(x.Id));

                EmployeesCache1.Clear();
            }
            else
            {
                if (!locations.Any()) return new List<RequiredActivityModel>();
                filter = EmployeeLocationsFilterGenerator.GetLocationsFilter(queryHelperEmployee, locations, filter);

            }
            //FilterDefinition<Employee> filterDirectReports = queryHelperEmployee.FilterBuilder.Empty;
            //if (hasDirectReports)
            //{
            //   filterDirectReports = queryHelperEmployee.FilterBuilder.Where(x => x.Manager.Id == hrsUser.Employee.Id);
            //}

            var activeEmployees = queryHelperEmployee.FindWithProjection(filterActive & filter, projectionEmployeeId);

            // Get RequiredActivities for the Active Employee Id list
            var queryHelperRequiredActivity = new MongoRawQueryHelper<RequiredActivity>();
            var filterRequiredActivity = queryHelperRequiredActivity.FilterBuilder.In(x => x.Employee.Id,
                activeEmployees);
            var filterNoCompleted = queryHelperRequiredActivity.FilterBuilder.Where(x =>
                x.Status != RequiredActivityStatus.Completed && x.CompleteStatus.Code != "ExamDate" && 
               (x.Type == RequiredActivityType.Training || x.Type == RequiredActivityType.TrainingCertification || x.Type == RequiredActivityType.InitialTraining || 
               (x.ActivityType != null && x.ActivityType.Code == "Training" || x.ActivityType.Code == "90-Day Review" || x.ActivityType.Code == "Goals and Objectives" || 
               x.ActivityType.Code == "On-the-Job Training" || x.ActivityType.Code == "Other")));

            var projectRequiredActivity =
                queryHelperRequiredActivity.ProjectionBuilder.Expression(x => new RequiredActivityModel(x));
            
            

            return queryHelperRequiredActivity.FindWithProjection(filterRequiredActivity & filterNoCompleted, projectRequiredActivity);
        }

        public static void getAllDirectReportsAndDescendants(ObjectId empId)
        {

            if (EmployeesCache1.ContainsKey(empId) && EmployeesCache1[empId] != null && EmployeesCache1[empId].Count() > 0)
            {
                foreach (var dr in EmployeesCache1[empId])
                {
                    var directReportId = new ObjectId(dr.Id);
                
                    DescendantsList.Add(dr);

                    getAllDirectReportsAndDescendants(directReportId);
                }
            }
        }
    }
}
