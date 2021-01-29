using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Security;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using DAL.WindowsAuthentication.MongoDb;
using NUnit.Framework;
using ObjectDumper = DAL.Vulcan.Test.ObjectDumper;

namespace DAL.Vulcan.NUnit.Tests.Security
{
	[TestFixture]
    public class VulcanInitialize
	{
	    public readonly string Application = "vulcancrm";
        //private IHelperUser _helperUser = new HelperUser(new HelperPerson());

        [Test]
        public void InitialConfigForCrm()
        {

            //SecurityManager securityManager = new SecurityManager();

            //securityManager.ForApplication(Application);

            var userRep = new RepositoryBase<LdapUser>();

            var mpike = userRep.AsQueryable().FirstOrDefault(x => x.NetworkId == "mpike");

            var newCrmUser = new Mongo.DocClass.CRM.CrmUser(mpike, CrmUserType.Manager) {IsAdmin = true};
            newCrmUser.SaveToDatabase();

            var isidro = userRep.AsQueryable().FirstOrDefault(x => x.NetworkId == "igallego");
            newCrmUser = new Mongo.DocClass.CRM.CrmUser(isidro, CrmUserType.Manager) { IsAdmin = true };
            newCrmUser.SaveToDatabase();
        }

        [Test]
        public void AddDirectorRole()
        {
            SecurityManager securityManager = new SecurityManager();

            securityManager.ForApplication("vulcancrm");
            securityManager.AddApplicationRole("Director");
            securityManager.SaveAll();

        }

        [Test]
        public void RefreshUsers()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
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
