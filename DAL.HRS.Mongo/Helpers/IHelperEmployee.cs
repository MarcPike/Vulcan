using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperEmployee
    {
        EmployeeModel GetNewEmployeeModel();
        EmployeeModel GetEmployeeModel(string id);
        Employee GetEmployee(string id);
        List<EmployeeModel> GetEmployeeDirectReports(string id);
        EmployeeModel SaveEmployee(EmployeeModel model);
        List<EmployeeDetailsGridModel> GetEmployeeDetailsGrid(string userId);
        List<EmployeeModel> GetAllMyEmployees(HrsUser hrsUser, SecurityRole role, SystemModule module);
        List<Employee> GetAllMyEmployeesForHrsModule(string userId, string moduleTypeName);
        List<Employee> GetAllMyEmployeesForHseModule(string userId, string moduleTypeName);
        List<EmployeeAuditTrailModel> GetAuditTrailForEmployee(string id);
        List<EmployeeRef> GetAllMyEmployeeReferencesForDirectReports(string employeeId);
        List<EmployeeRef> GetAllEmployeeReferencesForLocation(string locationId);
        List<EmployeeRef> GetAllEmployeeReferences();

        List<EmployeeRef> GetAllEmployeeReferencesBasedOnSecurityRoleModule(string employeeId, string moduleTypeName, bool hrsRole);

        List<EmployeeRef> GetAllEmployeeReferencesOfPossibleManagers(string employeeId);

        Employee FindEmployeeForHrsUser(HrsUser user);
        List<JobTitleRef> GetAllJobTitles();
        JobTitleModel GetJobTitleModel(string jobTitleId);
        JobTitleModel SaveJobTitle(JobTitleModel model);

        List<GlobalHeadcountModel> GetGlobalHeadCount();

        void RemoveEmployee(string employeeId);

       

    }
}