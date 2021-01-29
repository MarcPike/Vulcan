using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using NUnit.Framework;

namespace DAL.Common.Test.Tests
{
    [TestFixture]
    public class FixEmHrsUsers
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void FixUsersLdapWrong()
        {
            var queryHelperLdap = new MongoRawQueryHelper<LdapUser>();
            var queryHelperHrsUser = new MongoRawQueryHelper<HrsUser>();

            var franciso = queryHelperLdap.FilterBuilder.Where(x => x.LastName == "Francisco");
            var christiansen = queryHelperLdap.FilterBuilder.Where(x => x.LastName == "Christiansen");

            var ldapUsersWithWrongLocation = new List<LdapUser>();

            ldapUsersWithWrongLocation.Add(queryHelperLdap.Find(franciso).First());
            ldapUsersWithWrongLocation.Add(queryHelperLdap.Find(christiansen).First());

            var correctLocation = Location.GetReferencesForOffice("EM - Singapore");
            var correctEntity = correctLocation.AsLocation().Entity;

            foreach (var ldapUser in ldapUsersWithWrongLocation)
            {
                ldapUser.Location = correctLocation;
                queryHelperLdap.Upsert(ldapUser);

                var ldapUserRef = ldapUser.AsLdapUserRef();

                var hrsUserFilter = queryHelperHrsUser.FilterBuilder.Where(x => x.User.Id == ldapUserRef.Id);

                var hrsUser = queryHelperHrsUser.Find(hrsUserFilter).FirstOrDefault();

                if (hrsUser != null)
                {
                    hrsUser.Location = correctLocation;
                    hrsUser.Entity = correctEntity;
                    hrsUser.User = ldapUserRef;
                    queryHelperHrsUser.Upsert(hrsUser);
                }

            }

        }

    }
}
