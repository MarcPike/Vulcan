using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Encryption;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.Common.Models;
using DAL.HRS.Mongo.DocClass.Training;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperExternalApi : IHelperExternalApi
    {

        public HrsSecurityModel GetHrsSecurityForUser(string activeDirectoryId)
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var employee = ActiveDirectoryBasedSecurity.GetEmployee(activeDirectoryId, queryHelper);
            var hrsUser = ActiveDirectoryBasedSecurity.GetHrsUser(employee);

            return new HrsSecurityModel(hrsUser.HrsSecurity);
        }

        public HseSecurityModel GetHseSecurityForUser(string activeDirectoryId)
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var employee = ActiveDirectoryBasedSecurity.GetEmployee(activeDirectoryId, queryHelper);
            var hrsUser = ActiveDirectoryBasedSecurity.GetHrsUser(employee);

            return new HseSecurityModel(hrsUser.HseSecurity);
        }

        public List<EmployeeDetailsGridModel> GetEmployeeList()
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();

            var projection = queryHelper.ProjectionBuilder.Expression(x => new BaseGridModel(
                (x.TerminationDate == null || x.TerminationDate > DateTime.Now),
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
            var filter = queryHelper.FilterBuilder.Empty;

            var employees = queryHelper.FindWithProjection(filter, projection);

            return employees.Select(x => new EmployeeDetailsGridModel(x)).ToList();


        }

        public List<QngEmployeeBasicDataModel> GetEmployeeDetailsForQng(DateTime dateOf, string activeDirectoryId)
        {

            var enc = new Encryption();
            var queryHelper = new MongoRawQueryHelper<Employee>();

            var moduleName = "Employee Details";
            FilterDefinition<Employee> filter;

            try
            {
                filter = GetFilterForUser(activeDirectoryId, queryHelper, moduleName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            //var filterRemoveNotYetHired = Employee.Helper.FilterBuilder.
            //    Where(x => ;

            //var filterTerminated =
                //Employee.Helper.FilterBuilder.Where(x => x.TerminationDate == null || x.TerminationDate >= dateOf);

            //var employees = queryHelper.Find(filter).ToList();

            var project = queryHelper.ProjectionBuilder.
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

            return queryHelper.FindWithProjection(filter, project).Where(x=> (x.OriginalHireDate <= dateOf)).ToList();
        }

        public List<QngCompensationModel> GetCompensationForQng(DateTime dateOf, string activeDirectoryId)
        {

            var enc = new Encryption();
            var queryHelper = new MongoRawQueryHelper<Employee>();

            var moduleName = "Compensation";
            FilterDefinition<Employee> filter;

            try
            {
                filter = GetFilterForUser(activeDirectoryId, queryHelper, moduleName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            //FilterDefinition<Employee> activeFilter;

            //if (isActive)
            //{
            //    activeFilter =
            //        Employee.Helper.FilterBuilder.Where(x =>
            //            (x.TerminationDate == null || x.TerminationDate >= dateOf));
            //}
            //else
            //{
            //    activeFilter =
            //        Employee.Helper.FilterBuilder.Where(x =>
            //            (x.TerminationDate != null));

            //}

            var project = queryHelper.ProjectionBuilder.
                Expression(x => new QngCompensationModel(
                    x.Id, x.PayrollId, x.FirstName, x.PreferredName, x.LastName, x.Address1, x.Address2, x.Address3,
                    x.City, x.State, x.PostalCode, x.CountryCode, x.CountryOfOriginCode, x.EthnicityCode, x.Location,
                    x.KronosLaborLevelCode, x.KronosDepartmentCode, x.Status1Code, x.Status2Code, (x.TerminationDate == null || x.TerminationDate >= dateOf), x.Manager,
                    x.TerminationCode, x.PayrollRegion, x.GenderCode, x.GovernmentId,x.EEOCode, x.WorkAreaCode, 
                    x.OriginalHireDate, x.LastRehireDate, x.PriorServiceDate, x.TerminationDate, x.Birthday, x.Compensation, x.JobTitle, dateOf, x.CostCenterCode, x.ConfirmationDate
                ));

            //return queryHelper.FindWithProjection(filter & activeFilter, project).ToList();
            return queryHelper.FindWithProjection(filter, project).ToList();

        }

        //public List<QngEmployeeIncidentModel> GetEmployeeIncidentsForQng(DateTime dateOf, string activeDirectoryId)
        //{

        //    var enc = new Encryption();
        //    var queryHelper = new MongoRawQueryHelper<Employee>();

        //    var moduleName = "Compensation";
        //    FilterDefinition<Employee> filter;

        //    try
        //    {
        //        filter = GetFilterForUser(activeDirectoryId, queryHelper, moduleName);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        throw;
        //    }

        //    var project = queryHelper.ProjectionBuilder.
        //        Expression(x => new QngCompensationModel(
        //            x.Id, x.PayrollId, x.FirstName, x.PreferredName, x.LastName, x.Address1, x.Address2, x.Address3,
        //            x.City, x.State, x.PostalCode, x.CountryCode, x.CountryOfOriginCode, x.EthnicityCode, x.Location,
        //            x.KronosLaborLevelCode, x.KronosDepartmentCode, x.Status1Code, x.Status2Code, x.IsActive, x.Manager,
        //            x.TerminationCode, x.PayrollRegion, x.GenderCode, x.GovernmentId, x.EEOCode, x.WorkAreaCode,
        //            x.OriginalHireDate, x.LastRehireDate, x.PriorServiceDate, x.TerminationDate, x.Birthday, x.Compensation, x.JobTitle, dateOf
        //        ));

        //    return queryHelper.FindWithProjection(filter, project);
        //}

        public List<LocationModel> GetAllLocations()
        {
            var filter = Location.Helper.FilterBuilder.Empty;
            var project = Location.Helper.ProjectionBuilder.Expression(x => new LocationModel(x));
            return Location.Helper.FindWithProjection(filter, project).OrderBy(x => x.Office).ToList();
        }

        public List<QngEmployeeIncidentVarDataModel> GetEmployeeIncidentsByVarDataField(string varDataField, DateTime minDate, DateTime maxDate, string activeDirectoryId)
        {
            var security = GetHseSecurityForUser(activeDirectoryId);

            if (security == null) throw new Exception($"{activeDirectoryId} does not have access to HSE module information");

            return QngEmployeeIncidentVarDataModel.GetValuesFor(varDataField, minDate, maxDate);
        }

        public List<QngEmployeeIncidentModel> GetEmployeeIncidents(DateTime minDate, DateTime maxDate, string activeDirectoryId)
        {
            var security = GetHseSecurityForUser(activeDirectoryId);

            if (security == null) throw new Exception($"{activeDirectoryId} does not have access to HSE module information");

            return QngEmployeeIncidentModel.GetValuesFor(minDate, maxDate);
        }

        public List<QngTrainingInfoModel> GetTrainingInfo(DateTime minDate, DateTime maxDate, string activeDirectoryId)
        {
            var security = GetHrsSecurityForUser(activeDirectoryId);
            if (security == null) throw new Exception($"{activeDirectoryId} does not have access to HRS Training module information");

            var result = new List<QngTrainingInfoModel>();
            var trainingEvents =
                TrainingEvent.Helper.Find(x => x.StartDate >= minDate && x.EndDate <= maxDate).ToList();

            
            
            foreach (var trainingEvent in trainingEvents)
            {
                result.AddRange(QngTrainingInfoModel.GetForTrainingEvent(trainingEvent));
            }

            return result;
        }

        
        private FilterDefinition<Employee> GetFilterForUser(string activeDirectoryId, MongoRawQueryHelper<Employee> queryHelper, string moduleName)
        {
            var employee = ActiveDirectoryBasedSecurity.GetEmployee(activeDirectoryId, queryHelper);
            var hrsUser = ActiveDirectoryBasedSecurity.GetHrsUser(employee);
            var role = SecurityRoleHelper.GetHrsSecurityRole(hrsUser);

            var module = SecurityRoleHelper.GetSecurityModule(role, moduleName);

            if (!module.View) throw new Exception($"User does not have View rights for [{moduleName}]");


            var filter = queryHelper.FilterBuilder.Empty;

            if (role.DirectReportsOnly)
            {
                filter = EmployeeDirectReportsFilterGenerator.GetDirectReportsOnlyFilter(queryHelper, role, employee, filter);
                return filter;
            }

            var locations = hrsUser.HrsSecurity.Locations;
            filter = EmployeeLocationsFilterGenerator.GetLocationsFilter(queryHelper, locations, filter);
            return filter;
        }

    }
}
