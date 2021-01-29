using System;
using System.Linq;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Security_Tests
{
    [TestFixture]
    public class RunSecurityTests
    {
        private IHelperSecurity _helperSecurity;
        private IHelperUser _helperUser;
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
            _helperSecurity = new HelperSecurity();
            _helperUser = new Helpers.HelperUser();
        }

        [Test]
        public void Impersonate()
        {
            var networkIdEncryted = _helperSecurity.EncodeToBase64("mpike");
            var passwordEncrypted = "WmwxRnVuNjY=";

            Console.WriteLine(ObjectDumper.Dump(_helperSecurity.Authenticate(networkIdEncryted, passwordEncrypted)));
        }

        [Test]
        public void GetMyUserModel()
        {
            var networkIdEncryted = _helperSecurity.EncodeToBase64("mpike");
            var passwordEncrypted = "WmwxRnVuNjY=";

            var token = _helperSecurity.Authenticate(networkIdEncryted, passwordEncrypted);

            var myUserModel = _helperUser.GetUserModel(token.TokenId);
            Console.WriteLine(ObjectDumper.Dump(myUserModel));
        }

        [Test]
        public void GetHrsRoles()
        {
            var roles = _helperSecurity.GetAllRoles().Where(x=>x.RoleType.IsHrsRole).ToList();
            foreach (var role in roles)
            {
                Console.WriteLine(ObjectDumper.Dump(role));
            }
        }

        [Test]
        public void GetHseRoles()
        {
            var roles = _helperSecurity.GetAllRoles().Where(x=>x.RoleType.IsHseRole).ToList();
            foreach (var role in roles)
            {
                Console.WriteLine(ObjectDumper.Dump(role));
            }
        }

        [Test]
        public void GetHrsRoleNames()
        {
            var roleNames = _helperSecurity.GetAllRoleTypes().Where(x=>x.IsHrsRole).ToList();
            foreach (var roleTypeRef in roleNames)
            {
                Console.WriteLine(roleTypeRef.Name);
            }
        }

        [Test]
        public void GetHseRoleNames()
        {
            var roleNames = _helperSecurity.GetAllRoleTypes().Where(x=>x.IsHseRole).ToList();
            foreach (var roleTypeRef in roleNames)
            {
                Console.WriteLine(roleTypeRef.Name);
            }
        }

        [Test]
        public void GetHrsUsers()
        {
            var userList = _helperUser.GetAllHrsUsers();
            foreach (var hrsUserModel in userList)
            {
                //Console.WriteLine($"Hrs User: {hrsUserModel.L}  Employee: {hrsUserModel.Employee?.FullName}");
            }
        }


    }

}
