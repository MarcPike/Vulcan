using DAL.Vulcan.Mongo.Base.Context;
using DAL.WindowsAuthentication.MongoDb;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Users
{
    [TestFixture]
    public class RefreshLdapUsers
    {
        [Test]
        public void DoIt()
        {
            EnvironmentSettings.CrmProduction();
            EnvironmentSettings.RunningLocal = false;

            UserAuthentication.RefreshAll();
        }
    }
}
