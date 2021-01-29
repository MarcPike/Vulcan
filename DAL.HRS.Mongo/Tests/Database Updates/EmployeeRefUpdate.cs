using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Database_Updates
{
    [TestFixture()]
    public class EmployeeRefUpdate
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void UpdateEmployeeRefForManager()
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();

            //var filter = queryHelper.FilterBuilder.Ne(x => x.Manager, null);
            //var project = queryHelper.ProjectionBuilder.Expression(x => new {x.Id, x.Manager, x.DirectReports});

            var employees = queryHelper.Collection.Find(new BsonDocument()).ToList();


            foreach (var employee in employees)
            {
                employee.Manager = employee.Manager.AsEmployee().AsEmployeeRef();

                foreach (var employeeDirectReport in employee.DirectReports.ToList())
                {
                    var directReport = queryHelper.FilterBuilder.Eq(x => x.Id, ObjectId.Parse(employeeDirectReport.Id));
                    var projection = queryHelper.ProjectionBuilder.Expression(x => x.PayrollRegion);

                    var payrollRegion = queryHelper.Collection.Find(directReport).Project(projection).SingleOrDefault();

                    employeeDirectReport.PayrollRegion = payrollRegion;
                }
                employee.SaveToDatabase();
            }


        }
    }
}
