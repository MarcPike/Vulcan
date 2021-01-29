using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;

namespace DAL.Common.Test.Tests
{
    [TestFixture]
    public class TestCommonDatabaseInterface
    {
        [Test]
        public void TestNormalRepository()
        {
            var rep = new RepositoryBase<Location>();
        }

        [Test]
        public void TestNormalRepositoryButVulcan()
        {
            var rep = new RepositoryBase<DAL.Vulcan.Mongo.DocClass.Locations.Location>();
        }


        [Test]
        public void TestMongoQueryHelper()
        {
            var queryHelper = new MongoRawQueryHelper<Location>();
        }

        [Test]
        public void TestMongoQueryHelperButWithVulcan()
        {
            var queryHelper = new MongoRawQueryHelper< DAL.Vulcan.Mongo.DocClass.Locations.Location >();
        }

    }
}
