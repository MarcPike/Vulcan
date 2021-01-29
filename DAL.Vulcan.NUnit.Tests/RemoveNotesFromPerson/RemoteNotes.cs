using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Helpers;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.RemoveNotesFromPerson
{
    [TestFixture]
    public class RemoteNotes
    {

        [Test]
        public void OpenLdapUsers()
        {
            var helperApplication = new HelperApplication();
            var helperPerson = new HelperPerson();
            var helperUser = new HelperUser(helperPerson);

            helperApplication.VerifyApplication("vulcancrm");

            var marc = helperUser.LookupUserByNetworkId("mpike");
            Assert.IsNotNull(marc);
        }
    }
}
