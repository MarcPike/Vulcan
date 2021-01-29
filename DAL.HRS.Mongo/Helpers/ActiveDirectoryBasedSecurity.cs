using System;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.Helpers
{
    public static class ActiveDirectoryBasedSecurity
    {
        public static HrsUser GetHrsUser(Employee employee)
        {
            var queryHelperHrsUser = new MongoRawQueryHelper<HrsUser>();
            var userFilter = queryHelperHrsUser.FilterBuilder.Where(x => x.Employee.Id == employee.Id.ToString());
            var hrsUser = queryHelperHrsUser.Find(userFilter).FirstOrDefault();
            if (hrsUser == null) throw new Exception($"{employee.FirstName} {employee.LastName} is not an HrsUser");
            return hrsUser;
        }

        public static Employee GetEmployee(string activeDirectoryId, MongoRawQueryHelper<Employee> queryHelper)
        {
            var filterFindEmployee =
                queryHelper.FilterBuilder.Where(x => x.LdapUser.ActiveDirectoryId == activeDirectoryId);
            var employee = queryHelper.Find(filterFindEmployee).FirstOrDefault();
            if (employee == null) throw new Exception($"Employee not found with ActiveDirectoryId: {activeDirectoryId}");
            return employee;
        }

    }
}