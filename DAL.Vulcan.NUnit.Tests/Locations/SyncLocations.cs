using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Locations;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Locations
{
    [TestFixture]
    public class SyncLocations
    {
        [Test]
        public void SyncDevFromQC()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var locations = new RepositoryBase<Location>().AsQueryable().ToList();

            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var repLocation = new RepositoryBase<Location>();

            foreach (var location in locations)
            {
                var locationFound = repLocation.AsQueryable().SingleOrDefault(x => x.Office == location.Office);
                Assert.IsNotNull(locationFound);

                locationFound.MapLocation.Coordinates = location.MapLocation.Coordinates;
                repLocation.Upsert(locationFound);
            }

        }

    }
}
