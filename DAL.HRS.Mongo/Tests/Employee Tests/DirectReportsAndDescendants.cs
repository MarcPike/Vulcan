using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.Tests.Employee_Tests
{
    [TestFixture()]
    public class DirectReportsAndDescendants
    {
        List<EmployeeRef> DescendantsList = new List<EmployeeRef>();

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void getEmployeesWithDirectReports()
        {
            var objectId = new ObjectId("5e4e844c095f5970b89dce01");
            var filter = Employee.Helper.FilterBuilder.Where(x => x.Id == objectId);
            var projection = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.DirectReports });
            var employeesWithDirectReports = Employee.Helper.FindWithProjection(filter, projection).ToList();

            foreach (var employee in employeesWithDirectReports)
            {
                var empId = employee.Id;
                getAllDirectReportsAndDescendants(empId);
            }

        }

        public void getAllDirectReportsAndDescendants(ObjectId empId)
        {
            var manager = new RepositoryBase<DAL.HRS.Mongo.DocClass.Employee.Employee>().AsQueryable()
               .FirstOrDefault(x => x.Id == empId);

            if (manager.DirectReports.Count >= 1)
            {
                foreach (var dr in manager.DirectReports)
                {
                    var objectId = new ObjectId(dr.Id);
                    DescendantsList.Add(dr);

                    

                    getAllDirectReportsAndDescendants(objectId);
                }
            }
            
        }

    }
}
