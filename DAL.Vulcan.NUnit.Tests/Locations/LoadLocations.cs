using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Locations;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Locations
{
    [TestFixture]
    public class LoadLocations
    {
        [Test]
        public void DoIt()
        {
            EnvironmentSettings.CrmDevelopment();
            Location.GenerateDefaults();
        }
    }
}
