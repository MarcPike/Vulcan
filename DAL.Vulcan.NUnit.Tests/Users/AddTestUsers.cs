using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Users
{
    [TestFixture]
    public class AddTestUsers
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void Execute()
        {
            //var user = new LdapUser()
            //{
            //    Coid = "INC",
            //    NetworkId = "CrozPOC2"


            //};
        }

    }
}
