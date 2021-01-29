using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.ProductDevRoleIssue
{
    [TestFixture]
    public class HrsIIImProblem
    {
        [Test]
        public void Compare()
        {

            var roleName = "HR IIIM - Coordinator";
            
            EnvironmentSettings.HrsDevelopment();
            var helperDev = new MongoRawQueryHelper<SecurityRole>();
            var filterDev = helperDev.FilterBuilder.Where(x => x.RoleType.Name == roleName);
            var roleDev = helperDev.Find(filterDev).FirstOrDefault();

            EnvironmentSettings.HrsProduction();
            var helperProd = new MongoRawQueryHelper<SecurityRole>();
            var filterProd = helperDev.FilterBuilder.Where(x => x.RoleType.Name == roleName);
            var roleProd = helperProd.Find(filterProd).FirstOrDefault();

            Console.WriteLine("Role in Dev");
            Console.WriteLine(ObjectDumper.Dump(roleDev));
            Console.WriteLine("");
            Console.WriteLine("Role in Prod");
            Console.WriteLine(ObjectDumper.Dump(roleProd));

        }
    }
}
