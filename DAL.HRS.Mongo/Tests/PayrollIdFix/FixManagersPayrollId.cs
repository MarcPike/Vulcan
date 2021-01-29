using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.Tests.PayrollIdFix
{
    [TestFixture]
    class FixManagersPayrollId
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void Execute()
        {

            var filter = Employee.Helper.FilterBuilder.Where(x => x.Manager != null);
            var projection = Employee.Helper.ProjectionBuilder.Expression(x => x.PayrollId);
            var allEmpPayrollIds = Employee.Helper.FindWithProjection(filter, projection).ToList();


            var filterBadList = Employee.Helper.FilterBuilder.Where(x => x.Manager != null && !allEmpPayrollIds.Contains(x.Manager.PayrollId));
            var badListprojection = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.PayrollId, Manager = new { Id = x.Manager.Id, x.Manager.PayrollId } });
            var badList = Employee.Helper.FindWithProjection(filterBadList, badListprojection).ToList();

            var badIdList = badList.Select(x => MongoDB.Bson.ObjectId.Parse(x.Manager.Id)).ToList();


            var managerFilter = Employee.Helper.FilterBuilder.Where(x => badIdList.Contains(x.Id));
            var managerProjection = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.PayrollId });
            var managersList = Employee.Helper.FindWithProjection(managerFilter, managerProjection).ToList();



            foreach (var badEmp in badList)
            {
                var emp = managersList.FirstOrDefault(x => x.Id.ToString() == badEmp.Manager.Id);

                if (emp != null)
                {
                    var mEmp = Employee.Helper.FindById(badEmp.Id);
                    mEmp.Manager.PayrollId = emp.PayrollId;
                    Employee.Helper.Upsert(mEmp);


                }



            }

        }
    }
}
