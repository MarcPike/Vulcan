using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.PersonTests
{
    /*
    [TestFixture()]
    public class PersonContactsAndUserTestingcs
    {
        private IHelperUser _helperUser;
        private IHelperLocation _helperLocation;
        private IHelperTeam _helperTeam;
        private IHelperCompany _helperCompany;
        private IHelperGoal _helperGoal;
        private IHelperAction _helperAction;
        private IHelperUserViewConfig _helperUserViewConfig;
        private IHelperApplication _helperApplication;
        private IHelperNotifications _helperNotifications;
        private IHelperPerson _helperPerson;
        private IHelperContact _helperContact;
        private const string AppName = "vulcancrm";

        [SetUp]
        public void Setup()
        {
            _helperLocation = new HelperLocation();
            _helperApplication = new HelperApplication();
            _helperPerson = new HelperPerson();
            _helperContact = new HelperContact(_helperPerson, _helperUser);
            _helperUser = new HelperUser(_helperApplication,_helperPerson);
            _helperTeam = new HelperTeam(_helperUser);
            _helperCompany = new HelperCompany();
            _helperNotifications = new HelperNotifications(_helperUser);
            _helperAction = new HelperAction(_helperUser, _helperNotifications, _helperApplication);
            _helperGoal = new HelperGoal(_helperUser, _helperNotifications, _helperAction);
            _helperUserViewConfig = new HelperUserViewConfig(_helperUser, _helperTeam);
           
        }

        [Test]
        public void UpdateUser()
        {
            var manager = GetManager();

            var user = _helperUser.GetUser(manager.User.Id);
            //var personModelForUser = _helperPerson.GetPersonModelForUser(user);



        }

        [TearDown]
        public void TearDown()
        {
            
        }

        private Manager GetManager()
        {
            var user = _helperUser.LookupUserByNetworkId("mpike");
            Assert.IsTrue(_helperUser.IsInRole(AppName, user, "Manager"));
            var manager = _helperUser.GetManagerForUserId(AppName, user.Id.ToString());
            return manager;
        }

    }
    */
}
