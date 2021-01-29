using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BI.DAL.Mongo.Helpers;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace BI.DAL.Mongo.Tests.InitializeDb
{
    [TestFixture]
    public class CreateBiUsers
    {
        private HelperSecurity _helperSecurity;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.BiDevelopment();
            _helperSecurity = new HelperSecurity();
        }

        [Test]
        public void AddUsers()
        {
            var ldapUser = LdapUser.GetByName("Gallegos", "Isidro");
            Assert.IsNotNull(ldapUser);

            _helperSecurity.AddUser(ldapUser.Id.ToString(), true);

            ldapUser = LdapUser.GetByName("Pike", "Marc");
            Assert.IsNotNull(ldapUser);

            _helperSecurity.AddUser(ldapUser.Id.ToString(), true);

        }

        [Test]
        public void AddTestUsers()
        {
            var ldapUser = LdapUser.GetByName("Levine", "Rebecca");
            Assert.IsNotNull(ldapUser);

            _helperSecurity.AddUser(ldapUser.Id.ToString(), true);

            ldapUser = LdapUser.GetByName("Fraser", "Stanton");
            Assert.IsNotNull(ldapUser);

            _helperSecurity.AddUser(ldapUser.Id.ToString(), true);

        }

        [Test]
        public void AddJaved()
        {
            var ldapUser = LdapUser.GetByName("Merchant", "Javed");
            Assert.IsNotNull(ldapUser);

            _helperSecurity.AddUser(ldapUser.Id.ToString(), true);


        }

        [Test]
        public void AddSpencer()
        {
            var ldapUser = LdapUser.GetByName("Copeland", "Spencer");
            Assert.IsNotNull(ldapUser);

            _helperSecurity.AddUser(ldapUser.Id.ToString(), true);


        }


    }
}
