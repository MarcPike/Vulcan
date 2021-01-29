using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using System.Linq;

namespace DAL.HRS.Mongo.Tests.SynchronizePropertyValues
{
    [TestFixture]
    public class FixEmployeeProperties
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void ExecuteForCostCenter()
        {
            var costCenterCodes = new RepositoryBase<PropertyValue>().AsQueryable().Where(x=>x.Type == "CostCenter");
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var repEmp = new RepositoryBase<Employee>();
            foreach (var costCenterCode in costCenterCodes)
            {
                var costCenterId = costCenterCode.Id.ToString();
                var filter = queryHelper.FilterBuilder
                    .Where(x => x.CostCenterCode.Id == costCenterId && x.CostCenterCode.Description != costCenterCode.Description);
                var employees = queryHelper.Find(filter);
                foreach (var employee in employees)
                {
                    employee.CostCenterCode.Code = costCenterCode.Code;
                    employee.CostCenterCode.Description = costCenterCode.Description;
                    repEmp.Upsert(employee);
                }

            }



        }

    }
}
