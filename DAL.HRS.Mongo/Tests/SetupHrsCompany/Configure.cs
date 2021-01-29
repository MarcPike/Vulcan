using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Linq;

namespace DAL.HRS.Mongo.Tests.SetupHrsCompany
{
    [TestFixture()]
    public class ConfigureHrsCompanies
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var allLocations = new RepositoryBase<Location>().AsQueryable().ToList();
            var allPropertyValues = new RepositoryBase<PropertyValue>().AsQueryable().ToList();
            var rep = new RepositoryBase<Entity>();

            UpdateEmployeeEntity();
            UpdateHrsUserEntity();

        }

        [Test]
        public void UpdateEmployeeEntity()
        {
            var database = EnvironmentSettings.Database;
            var entityRef = Entity.GetRefByName("Howco");
            database = EnvironmentSettings.Database;
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var noFilter = queryHelper.FilterBuilder.Empty;
            var update = queryHelper.UpdateBuilder.Set(x => x.Entity, entityRef);
            queryHelper.UpdateMany(noFilter, update);
        }

        [Test]
        public void UpdateHrsUserEntity()
        {
            var entityRef = Entity.GetRefByName("Howco");

            var queryHelper = new MongoRawQueryHelper<HrsUser>();
            var noFilter = queryHelper.FilterBuilder.Empty;
            var update = queryHelper.UpdateBuilder.Set(x => x.Entity, entityRef);
            queryHelper.UpdateMany(noFilter, update);
        }

    }
}
