using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.SqlServer;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using HrsContext = DAL.HRS.SqlServer.Model.HrsContext;


namespace DAL.HRS.Import.Tests
{
    [TestFixture()]
    public class TestCompensation
    {
        private CompensationBuilder _compensationBuilder = new CompensationBuilder();
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void TestCompensationWithOtherComp()
        {
            using (HrsContext context = new HrsContext())
            {
                var rep = new RepositoryBase<Employee>();
                var hrsEmployee = context.Employee.SingleOrDefault(x => x.OID == 1032);
                var employee = rep.AsQueryable().First(x => x.OldHrsId == 1032);

                var dataFetcher = new EmployeeBasicInfoHrs();

                var employeeInfoModel = dataFetcher.GetEmployeeBasicInfo(295);

                _compensationBuilder.GetBaseCompensationHrs(295);
                var compensationHrs = _compensationBuilder.Data;

                ImportHrs.CompensationTransformer.TransFormCompensation(employee, compensationHrs);

                rep.Upsert(employee);

            }

        }

    }
}
