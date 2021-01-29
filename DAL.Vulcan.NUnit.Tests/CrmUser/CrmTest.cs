using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.CrmUser
{
    [TestFixture]
    public class CrmUserTest
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void GetEmailList()
        {
            var crmUsers = Mongo.DocClass.CRM.CrmUser.Helper.GetAll();
            foreach (var crmUser in crmUsers)
            {
                var user = crmUser.User.AsUser();
                if (user == null) continue;
                if (!user.Person.EmailAddresses.Any()) continue;

                Console.WriteLine($@"{crmUser.User.UserName} Team: {crmUser.ViewConfig?.Team?.Name ?? "None"} Email: {user.Person.EmailAddresses.FirstOrDefault()?.Address}");
            }
        }
    }
}
