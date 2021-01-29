using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Diagnostics;
using System.Linq;

namespace DAL.HRS.Mongo.Tests.HelperSecurity_Tests
{
    [TestFixture()]
    public class HelperSecurityTests
    {
        private HelperSecurity _helperSecurity;
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsQualityControl();
            _helperSecurity = new HelperSecurity();
        }

        [Test]
        public void GetHrsNewUserList()
        {
            var results = _helperSecurity.GetHrsNewUserList();
        }

        [Test]
        public void GetRoleNames()
        {
            Console.WriteLine("Dev");
            Console.WriteLine("==============================");
            var helper = new MongoRawQueryHelper<SecurityRole>();
            var securityRoles = helper.GetAll().Select(x => x.RoleType.Name).ToList();
            foreach (var securityRole in securityRoles)
            {
                Console.WriteLine(securityRole);
            }
            EnvironmentSettings.HrsProduction();
            helper = new MongoRawQueryHelper<SecurityRole>();
            Console.WriteLine("");
            Console.WriteLine("Prod");
            Console.WriteLine("==============================");
            securityRoles = helper.GetAll().Select(x => x.RoleType.Name).ToList();
            foreach (var securityRole in securityRoles)
            {
                Console.WriteLine(securityRole);
            }
        }

        [Test]
        public void GetModulesForRole()
        {
            var securyRoleModel = _helperSecurity.GetSecurityRoleModelForName("HSESystemAdmin");
            foreach (var module in securyRoleModel.Modules)       
            {
                Console.WriteLine(module.ModuleType.Name);        
            }

            
        }

        [Test]
        public void GetModuleNames()
        {
            var modules = SystemModuleType.Helper.GetAll().OrderBy(x => x.Name).ToList();
            foreach (var systemModuleType in modules)
            {
                Console.WriteLine($"{systemModuleType.Name}: IsHrs: {systemModuleType.IsHrsModule} IsHse: {systemModuleType.IsHseModule}");
            }

        }

        [Test]
        public void GetRolesThatHaveDashboards()
        {

            var roles = SecurityRole.Helper.Find(x => x.Modules.Any(m => m.ModuleType.Name == "HSE Dashboard"))
                .ToList();

            foreach (var role in roles)
            {
                Console.WriteLine(role.RoleType.Name);
            }
            

        }



        [Test]
        public void CheckForLdapUserNotValidForHrsUsers()
        {
            var queryHelperLdap = new MongoRawQueryHelper<LdapUser>();
            var queryHelperHrs = new MongoRawQueryHelper<HrsUser>();

            LdapUser ldapUser = null;
            foreach (var hrsUser in queryHelperHrs.GetAll())
            {
                ldapUser = queryHelperLdap.FindById(hrsUser.UserId);
                if (ldapUser == null)
                {
                    Console.WriteLine($"{hrsUser.FirstName} {hrsUser.LastName} has incorrect LdapUser");
                } else
                {
                    Console.WriteLine($"{hrsUser.FirstName} {hrsUser.LastName} is OK");
                }
            }
        }


        [Test]
        public void GetLdapUser()
        {
            var queryHelperLdap = new MongoRawQueryHelper<LdapUser>();
            var queryHelperHrs = new MongoRawQueryHelper<HrsUser>();

            LdapUser ldapUser = LdapUser.Helper.Find(x => x.LastName == "Pike").FirstOrDefault();

        }

        //[Test]
        //public void FixHrsUserRoleIssue()
        //{
        //    var queryHelper = new MongoRawQueryHelper<HrsUser>();
        //    foreach (var hrsUser in queryHelper.GetAll())
        //    {
        //        if (hrsUser.HrsSecurity.GetRole() != null)
        //        {
        //            hrsUser.HrsSecurity.RoleId = hrsUser.HrsSecurity.Role.Id;
        //        }

        //        if (hrsUser.HseSecurity.Role != null)
        //        {
        //            hrsUser.HseSecurity.RoleId = hrsUser.HseSecurity.Role.Id;
        //        }

        //        queryHelper.Upsert(hrsUser);
        //    }
        //}

        [Test]
        public void PerformanceTesting()
        {
            var helperSecurity = new HelperSecurity();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var results = helperSecurity.GetHrsNewUserList();
            sw.Stop();
            Console.WriteLine("Elapsed: "+sw.Elapsed);
            foreach (var hrsNewUserModel in results)
            {
                Console.WriteLine($"{hrsNewUserModel.Employee.FullName} Ldap: {hrsNewUserModel.LdapUser?.GetFullName()}");
            }

        }

        [Test]
        public void AddKennyNessAsHrsUserProduction()
        {
            EnvironmentSettings.HrsProduction();


            var helperSecurity = new HelperSecurity();
            helperSecurity.AddUser( "5db85013095f5934a81697f2", "5dd40f67095f593670c5d963", "5d7a891f095f5825801bc0c6",
                "5de58b0e0de3c61c843696c9", Entity.GetRefByName("Edgen Murray"));
        }

        [Test]
        public void AddKennyNessAsHrsUserDevelopment()
        {
            EnvironmentSettings.HrsDevelopment();


            var helperSecurity = new HelperSecurity();
            helperSecurity.AddUser("5db85013095f5934a81697f2", "5dd40f67095f593670c5d963", "5de58b0e0de3c61c843696c9",
                "5de58b0e0de3c61c843696c9", Entity.GetRefByName("Howco"));
        }

        [Test]
        public void GetAllRoles()
        {
            var roles = _helperSecurity.GetAllRoles();
            foreach (var securityRoleModel in roles)
            {
                Console.WriteLine($"{securityRoleModel.Id} - {securityRoleModel.RoleType.Name}");
            }
        }

        [Test]
        public void GetAllRoleTypes()
        {
            var roleTypes = _helperSecurity.GetAllRoleTypes();
            foreach (var roleTypeRef in roleTypes)
            {
                Console.WriteLine($"{roleTypeRef.Id} - {roleTypeRef.Name}");
            }
        }

    }
}
