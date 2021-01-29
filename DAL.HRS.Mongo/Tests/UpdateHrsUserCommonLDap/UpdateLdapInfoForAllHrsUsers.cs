using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Mongo.Tests.UpdateHrsUserCommonLDap
{
    [TestFixture()]
    public class UpdateLdapInfoForAllHrsUsers
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var queryHelperLdap = new MongoRawQueryHelper<LdapUser>();
            var queryHelperHrsUser = new MongoRawQueryHelper<HrsUser>();
            var hrsUsers = queryHelperHrsUser.GetAll();
            foreach (var hrsUser in hrsUsers)
            {
                LdapUser ldapUser;
                if (hrsUser.User.NetworkId == "testUser")
                {
                    var filterTest = queryHelperLdap.FilterBuilder.Where(x => x.NetworkId == "HrsTestUser");
                    ldapUser = queryHelperLdap.Find(filterTest).FirstOrDefault();
                    continue;
                }
                else
                {
                    var filter = queryHelperLdap.FilterBuilder.Where(x => x.NetworkId == hrsUser.User.NetworkId);
                    ldapUser = queryHelperLdap.Find(filter).FirstOrDefault();
                }
                if (ldapUser == null)
                {
                    Console.WriteLine($"No find for {hrsUser.FirstName} {hrsUser.LastName}");
                }
                else
                {
                    hrsUser.ChangeLdapUserTo(ldapUser);
                    queryHelperHrsUser.Upsert(hrsUser);
                }
            }

        }
    }
}
