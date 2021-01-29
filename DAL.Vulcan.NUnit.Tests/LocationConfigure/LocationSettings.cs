using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Config;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.LocationConfigure
{
    [TestFixture()]
    public class LocationSettings
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
        }

        [Test]
        public void SetupGrayOptions()
        {
            var location = new RepositoryBase<Location>().AsQueryable().SingleOrDefault(x => x.Office == "Gray");
            Assert.IsNotNull(location);

            var locationRef = location.AsLocationRef();

            var locationConfigurationForHeatTreat = 
                new RepositoryBase<LocationConfiguration>().AsQueryable()
                    .SingleOrDefault(x=> x.Location.Id == locationRef.Id) ??
                new LocationConfiguration()
                {
                    Location = location.AsLocationRef(),
                    DefaultMargins = new List<LocationResourceDefaultMargin>()
                    {
                        new LocationResourceDefaultMargin() {ResourceType = ResourceType.HeatTreat.ToString(), DefaultMargin = 25},
                        new LocationResourceDefaultMargin() {ResourceType = ResourceType.Bore.ToString(), DefaultMargin = 25},
                        new LocationResourceDefaultMargin() {ResourceType = ResourceType.Machining.ToString(), DefaultMargin = 25},
                        new LocationResourceDefaultMargin() {ResourceType = ResourceType.Ship.ToString(), DefaultMargin = 15}
                    }
                };

            locationConfigurationForHeatTreat.SaveToDatabase();

        }

        [Test]
        public void GetGrayOptions()
        {
            var location = new RepositoryBase<Location>().AsQueryable().SingleOrDefault(x => x.Office == "Gray");
            Assert.IsNotNull(location);

            var locationRef = location.AsLocationRef();

            var locationConfiguration = LocationConfiguration.GetConfigurationFor(locationRef);

            Console.WriteLine(ObjectDumper.Dump(locationConfiguration));
        }
    }
}
