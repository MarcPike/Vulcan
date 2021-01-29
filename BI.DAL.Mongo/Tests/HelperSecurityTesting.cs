using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BI.DAL.Mongo.Helpers;
using BI.DAL.Mongo.Security;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace BI.DAL.Mongo.Tests
{
    [TestFixture]
    public class HelperSecurityTesting
    {
        private HelperSecurity _helperSecurity = new HelperSecurity();
        private HelperUser _helperUser = new HelperUser();

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.BiDevelopment();
        }

        [Test]
        public void CreateBiUserTokenConfig()
        {
            var biUserTokenConfig = BiUserTokenConfig.Get();
            Assert.IsNotNull(biUserTokenConfig);

        }


        [Test]
        public void GetUser()
        {
            var ldapUser = _helperUser.GetUser("5dd40f67095f593670c5d985");
            Assert.IsNotNull(ldapUser);
            Console.WriteLine($"{ldapUser.LastName}");

            var token = BiUserToken.Create(ldapUser);
            Console.WriteLine($"{token.Expires}");
            Assert.IsNotNull(token);
        }

    }
}
