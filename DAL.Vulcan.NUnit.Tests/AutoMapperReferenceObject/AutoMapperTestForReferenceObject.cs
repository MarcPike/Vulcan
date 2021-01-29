using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.AutoMapperReferenceObject
{
    [TestFixture]
    public class AutoMapperTestForReferenceObject
    {
        [Test]
        public void UserRefTest()
        {
            var helperApplication = new HelperApplication();
            var helperPerson = new HelperPerson();
            var helperUser = new HelperUser(helperPerson);

            var user = helperUser.LookupUserByNetworkId("mpike");
            var userRef = user.AsUserRef();
            Assert.AreEqual(userRef.Id,user.Id.ToString());
            Assert.AreEqual(userRef.FirstName, user.FirstName);
            Assert.AreEqual(userRef.LastName,user.LastName);
        }
    }
}
