using System;
using System.Linq;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Locations
{
    [TestFixture()]
    public class GetLocations
    {
        private HelperLocation _helperLocation;
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
            EnvironmentSettings.RunningLocal = false;
            _helperLocation = new HelperLocation();
        }

        [Test]
        public void Execute()
        {
            var locations = _helperLocation.GetAllLocations();
            foreach (var location in locations)
            {
                location.SaveToDatabase();
                Console.WriteLine(ObjectDumper.Dump(location.AsLocationRef()));
            }
        }

        [Test]
        public void GetLatestOffices()
        {
            var locations = _helperLocation.GetAllLocations();
            foreach (var location in locations.OrderBy(x=>x.Office))
            {
                //location.SaveToDatabase();
                Console.WriteLine(location.Office);
            }
        }
    }
}
