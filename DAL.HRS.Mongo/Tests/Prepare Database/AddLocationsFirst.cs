using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Prepare_Database
{
    [TestFixture]
    public class AddLocationsFirst
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        //[Test]
        //public void Execute()
        //{
        //    Location();
        //}
    }
}
