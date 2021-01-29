using System;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.Helpers
{
    public class BaseDashboard
    {
        protected HrsUser _hrsUser;

        public BaseDashboard()
        {
        }

        public BaseDashboard(HrsUser hrsUser)
        {
            _hrsUser = hrsUser;
        }

        protected (FilterDefinition<Employee> Filter, List<string> EmpIdList) GetEmployees(bool onlyActive)
        {
            var role = SecurityRoleHelper.GetHrsSecurityRole(_hrsUser);
            var employee = _hrsUser.Employee.AsEmployee();

            var empFilter = FilterDefinition<Employee>.Empty;

            if (role.DirectReportsOnly)
            {
                empFilter = EmployeeDirectReportsFilterGenerator.GetDirectReportsOnlyFilter(Employee.Helper, role,
                    employee,
                    empFilter);
            }
            else
            {
                var locations = _hrsUser.HrsSecurity.Locations;

                empFilter = EmployeeLocationsFilterGenerator.GetLocationsFilter(Employee.Helper, locations, empFilter);
            }

            if (onlyActive)
                empFilter = empFilter & Employee.Helper.FilterBuilder.Where(x =>
                    x.TerminationDate == null || x.TerminationDate > DateTime.Now);


            var projectionEmployeeId = Employee.Helper.ProjectionBuilder.Expression(x => x.Id.ToString());

            var empIdList = Employee.Helper.FindWithProjection(empFilter, projectionEmployeeId).ToList();
            if (empIdList.All(x => x != employee.Id.ToString())) empIdList.Add(employee.Id.ToString());

            return (empFilter, empIdList);
        }
    }
}