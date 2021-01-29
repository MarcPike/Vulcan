using System.Diagnostics;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.Users
{
    [TestFixture()]
    public class UserReferenceTests
    {
        /*
        [Test]
        public void TestMethod1()
        {
            var user = new RepositoryBase<LdapUser>().AsQueryable().Single(x => x.NetworkId == "mpike");

            var manager = new RepositoryBase<Manager>().AsQueryable().SingleOrDefault(x=>x.User.Id == user.Id.ToString());

            Trace.WriteLine(ObjectDumper.Dump(manager));
        }
        */
    }
}
