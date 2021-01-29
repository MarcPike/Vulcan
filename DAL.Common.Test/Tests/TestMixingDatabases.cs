using System;
using System.Linq;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace DAL.Common.Test.Tests
{
    [TestFixture]
    public class TestMixingDatabases
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void TestLocationsFromBothCommonVulcanDatabases()
        {
            var commonLocations = new MongoRawQueryHelper<Location>().GetAll().Select(x=>x.Office).ToList();
            var vulcanLocations = new MongoRawQueryHelper<DAL.Vulcan.Mongo.DocClass.Locations.Location>().GetAll().Select(x=>x.Office).ToList();

            Assert.IsTrue(commonLocations.Count > vulcanLocations.Count);

            var newLocations = commonLocations.Where(g => vulcanLocations.All(v => v != g)).ToList();

            Console.WriteLine("New locations");
            Console.WriteLine("-------------");
            foreach (var newLocation in newLocations)
            {
                Console.WriteLine(newLocation);
            }
        }

        [Test]
        public void TestLocationsFromBothCommonHrsDatabases()
        {
            EnvironmentSettings.HrsDevelopment();
            var commonLocations = new MongoRawQueryHelper<Location>().GetAll().Select(x => x.Office).ToList();
            var vulcanLocations = new MongoRawQueryHelper<DAL.Vulcan.Mongo.DocClass.Locations.Location>().GetAll().Select(x => x.Office).ToList();

            Assert.IsTrue(commonLocations.Count > vulcanLocations.Count);

            var newLocations = commonLocations.Where(g => vulcanLocations.All(v => v != g)).ToList();

            Console.WriteLine("New locations");
            Console.WriteLine("-------------");
            foreach (var newLocation in newLocations)
            {
                Console.WriteLine(newLocation);
            }
        }
    }
}
