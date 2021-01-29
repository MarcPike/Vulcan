using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.ActionTests
{
    [TestFixture]
    public class AddRemoveActionTests
    {
        private HelperPerson _helperPerson;
        private HelperUser _helperUser;
        private HelperAction _helperAction;
        private HelperNotifications _helperNotifications;
        private HelperApplication _helperApplication;
        private HelperReminder _helperReminder;
        private LdapUser _ldapUser;
        private Mongo.DocClass.CRM.CrmUser _crmUser;
        private LdapUser _otherLdapUser;
        private Mongo.DocClass.CRM.CrmUser _otherCrmUser;
        private const string AppName = "vulcancrm";

        [SetUp]
        public void SetUp()
        {
            _helperApplication = new HelperApplication();
            _helperPerson = new HelperPerson();
            _helperUser = new HelperUser(_helperPerson);
            _helperNotifications = new HelperNotifications(_helperUser);
            _helperReminder = new HelperReminder();
            _helperAction = new HelperAction(_helperUser,_helperNotifications,_helperApplication,_helperReminder);

            LoadTestUsers();
        }

        private void LoadTestUsers()
        {
            _ldapUser = _helperUser.LookupUserByNetworkId("mpike");
            _crmUser = _helperUser.GetCrmUser(AppName, _ldapUser.Id.ToString());
            _otherLdapUser = _helperUser.LookupUserByNetworkId("igallego");
            _otherCrmUser = _helperUser.GetCrmUser(AppName, _otherLdapUser.Id.ToString());
        }

        [Test]
        public void Execute()
        {
            var newActionModel = _helperAction.CreateNewAction(AppName, _ldapUser);
            newActionModel.ActionType = "Meeting";
            newActionModel.CrmUsers.Add(_otherCrmUser.AsCrmUserRef());
            newActionModel.Label = "Test Add/Remove action logic #1";
            newActionModel.DueDate = DateTime.Today.AddDays(1);

            var savedAction = _helperAction.SaveAction(newActionModel);

            LoadTestUsers();

            Assert.IsTrue(_crmUser.Actions.Any(x => x.Id == savedAction.Id.ToString()));
            Assert.IsTrue(_otherCrmUser.Actions.Any(x => x.Id == savedAction.Id.ToString()));

            _helperAction.RemoveAction(AppName,_ldapUser.Id.ToString(),savedAction.Id.ToString());

            LoadTestUsers();

            Assert.IsTrue(_crmUser.Actions.All(x => x.Id != savedAction.Id.ToString()));
            Assert.IsTrue(_otherCrmUser.Actions.All(x => x.Id != savedAction.Id.ToString()));

        }
    }
}
