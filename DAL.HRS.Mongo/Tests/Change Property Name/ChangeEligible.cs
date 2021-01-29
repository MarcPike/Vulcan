using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Change_Property_Name
{
    [TestFixture()]
    public class ChangeEligible
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var rep = new RepositoryBase<Employee>();
            foreach (var employee in rep.AsQueryable().ToList())
            {
                rep.Upsert(employee);
            }
        }

    }
}
