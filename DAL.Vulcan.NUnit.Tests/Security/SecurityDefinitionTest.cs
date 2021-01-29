using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Security;
using DAL.WindowsAuthentication.MongoDb;
using NUnit.Framework;
using ObjectDumper = DAL.Vulcan.Test.ObjectDumper;

namespace DAL.Vulcan.NUnit.Tests.Security
{
    [TestFixture]
    public class SecurityDefinitionTest
    {
        [Test]
        public void SecurityConfigureCrm()
        {
            SecurityManager securityManager = new SecurityManager();

            securityManager.ForApplication("vulcancrm");

            var mpike = UserAuthentication.GetUser("mpike", "Kick1966");
            Assert.IsNotNull(mpike);

            var userRep = new RepositoryBase<LdapUser>();

            var isidro = userRep.AsQueryable().FirstOrDefault(x => x.NetworkId == "igallego");
            Assert.IsNotNull(isidro);

            if (securityManager.Application.Users.All(x=>x.Id != mpike.Id)) securityManager.Application.Users.Add(mpike);

            if (securityManager.Application.Users.All(x => x.Id != isidro.Id)) securityManager.Application.Users.Add(isidro);

            securityManager.SaveAll();
            
            securityManager.AddApplicationRole("SalesPerson");
            securityManager.AddUsersToApplicationRole("SalesPerson", new List<LdapUser>() {isidro, mpike});

            securityManager.AddApplicationRole("Admin");
            securityManager.AddUsersToApplicationRole("Admin", new List<LdapUser>() { isidro, mpike });

            securityManager.AddApplicationRole("Manager");
            securityManager.AddUsersToApplicationRole("Manager", new List<LdapUser>() { isidro, mpike });
        }

        [Test]
        public void RefreshUsers()
        {
            var userRep = new RepositoryBase<LdapUser>();

            var isidro = userRep.AsQueryable().FirstOrDefault(x => x.NetworkId == "igallego");
            var mpike = userRep.AsQueryable().FirstOrDefault(x => x.NetworkId == "mpike");

            var allUsers = userRep.AsQueryable().Where(x=>x.Id != isidro.Id && x.Id != mpike.Id).ToList();
            foreach (var user in allUsers)
            {
                userRep.RemoveOne(user);
            }

            allUsers = userRep.AsQueryable().ToList();

            var ldapReader = new LdapReader();
            ldapReader.RefreshUserListFromLdap();

            var badLocations = ldapReader.BadLocations.OrderBy(x => x.Location).Distinct();
            foreach (var location in badLocations)
            {
                Trace.WriteLine(Test.ObjectDumper.Dump(location));
            }

        }

    }
}
