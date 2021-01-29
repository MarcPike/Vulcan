using System;
using System.Diagnostics;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Test;
using NUnit.Framework;
using Environment = System.Environment;

namespace DAL.Vulcan.NUnit.Tests.Users
{
    [TestFixture]
    public class UserNameTest
    {
        [Test]
        public void DoIt()
        {
            var repUser = new RepositoryBase<LdapUser>();
            foreach (var user in repUser.AsQueryable().Where(x=>x.UserName == "Sinton, Jim").ToList())
            {
                Console.WriteLine(ObjectDumper.Dump(user));
            }
        }
    }
}
