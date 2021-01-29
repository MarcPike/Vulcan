using System;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperUser_Testing
{
    [TestFixture()]
    public class Test
    {
        private Helpers.HelperUser _helperUser;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
            _helperUser = new Helpers.HelperUser();
        }

        [Test]
        public void GetAllLdapUsers()
        {
            var results = _helperUser.GetAllLdapUsers("R");
            foreach (var ldapUserRef in results)
            {
                Console.WriteLine(ldapUserRef.Id + " => "+ldapUserRef.GetFullName());
            }
        }

        [Test]
        public void SaveAllHrsUsersToAddNewFields()
        {
            var hrsUsers = HrsUser.Helper.GetAll();
            foreach (var hrsUser in hrsUsers)
            {
                HrsUser.Helper.Upsert(hrsUser);
            }
        }

        [Test]
        public void GetAllHrsUsers()
        {
            var hrsUsers = _helperUser.GetAllHrsUsers();
        }

        [Test]
        public void GetDeniseUserModel()
        {
            var hrsUser = HrsUser.Helper.Find(x => x.LastName == "Walker").FirstOrDefault();

            var hrsUserRole = hrsUser.HrsSecurity.GetRole();
            foreach (var systemModule in hrsUserRole.Modules)
            {
                Console.WriteLine($"{systemModule.ModuleType.Name} - Add:{systemModule.Add} Modify:{systemModule.Modify} Del: {systemModule.Delete} View: {systemModule.View}");
            }
        }

    }
}
