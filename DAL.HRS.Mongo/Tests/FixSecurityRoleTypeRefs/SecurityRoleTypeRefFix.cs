using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.FixSecurityRoleTypeRefs
{
    [TestFixture]
    public class SecurityRoleTypeRefFix
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void Execute()
        {
            var queryHelper = new MongoRawQueryHelper<SecurityRole>();
            var roles = queryHelper.GetAll();
            foreach (var securityRole in roles)
            {
                if (securityRole.Id.ToString() == securityRole.RoleType.Id)
                    Console.WriteLine($"{securityRole.RoleType.Name} is wrong");
            }

        }
    }
}
