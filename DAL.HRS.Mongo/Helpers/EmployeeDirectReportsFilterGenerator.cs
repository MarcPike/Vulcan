using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.Helpers
{
    public static class EmployeeDirectReportsFilterGenerator
    {
        public static FilterDefinition<Employee> GetDirectReportsOnlyFilter(MongoRawQueryHelper<Employee> queryHelper, SecurityRole role,
            Employee employee, FilterDefinition<Employee> filter)
        {
            if (role.DirectReportsOnly)
            {
                var directReports = employee.GetAllDirectReports();

                if (!directReports.Any()) return Employee.Helper.FilterBuilder.Where(x => 1 == 0);
                
                foreach (var employeeRef in directReports)
                {
                    var employeeId = ObjectId.Parse(employeeRef.Id);
                    if (filter == FilterDefinition<Employee>.Empty)
                    {
                        filter = queryHelper.FilterBuilder.Eq(x => x.Id, employeeId);
                    }
                    else
                    {
                        filter = filter | queryHelper.FilterBuilder.Eq(x => x.Id, employeeId);
                    }
                }


            }

            return filter;
        }


    }
}