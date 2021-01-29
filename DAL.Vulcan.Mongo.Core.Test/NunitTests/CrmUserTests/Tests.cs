using System;
using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.Mongo.Core.Test.NunitTests.CrmUserTests
{
    [TestFixture]
    class Tests
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void FindUser()
        {
            var user = CrmUser.Helper.Find(x => x.User.LastName == "Pike").FirstOrDefault();
            Assert.IsNotNull(user);
            Console.WriteLine(ObjectDumper.Dump(user));
        }

        
    }
}
