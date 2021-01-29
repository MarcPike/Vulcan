using DAL.Marketing.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.StrategicAccounts
{
    [TestFixture]
    public class StrategicAccountsTesting
    {
        private IHelperMarketing _helperMarketing = new HelperMarketing();

        [Test]
        public void Initialize()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;




        }

        [Test]
        public void AddChildren()
        {
        }
    }
}
