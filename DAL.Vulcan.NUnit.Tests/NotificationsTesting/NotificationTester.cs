using System.Linq;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.NotificationsTesting
{
    [TestFixture]
    public class NotificationTester
    {
        private readonly string _application = "vulcancrm";
        private readonly HelperApplication _helperApplication;
        private readonly HelperReminder _helperReminder;
        private readonly HelperPerson _helperPerson;
        private readonly HelperNotifications _helperNotifications;
        private readonly HelperUser _helperUser;
        private readonly HelperAction _helperAction;
        private Mongo.DocClass.CRM.CrmUser TestUserOne { get; set; }
        private Mongo.DocClass.CRM.CrmUser TestUserTwo { get; set; }
        public FakePublisher FakePublisher { get; set; } = new FakePublisher();
        public NotificationTester()
        {
            NotificationRouter.RegisterSignalR(FakePublisher);
            _helperPerson = new HelperPerson();
            _helperApplication = new HelperApplication();
            _helperReminder = new HelperReminder();
            _helperUser = new HelperUser(_helperPerson);
            _helperNotifications = new HelperNotifications(_helperUser);
            _helperAction = new HelperAction(_helperUser,_helperNotifications,_helperApplication, _helperReminder);
            var user = _helperUser.LookupUserByNetworkId("mpike");
            TestUserOne = _helperUser.GetCrmUser(_application, user.Id.ToString());

            user = _helperUser.LookupUserByNetworkId("igallego");
            TestUserTwo = _helperUser.GetCrmUser(_application, user.Id.ToString());
        }

        [Test]
        public void TestAddActionOneUserNotTeam()
        {
            Assert.IsNotNull(TestUserOne);
            var actionModel = ActionModel.CreateNewActionModel(_application, TestUserOne.User.Id);

            Assert.IsTrue(actionModel.CrmUsers.Count == 1);
            Assert.IsNotNull(actionModel.CrmUsers.Single(x=>x.UserId == TestUserOne.User.Id.ToString()));

            actionModel.ActionType = "Task";
            actionModel.Label = "Internal Testing (Please ignore)";

            var action = _helperAction.SaveAction(actionModel);

            Assert.IsNotNull(action);

            // Not needed since a notification will get added within _helperAction.SaveAction()
            //NotificationRouter.ActionAdded(action);

            _helperAction.RemoveAction(action);
        }

        [Test]
        public void TestAddActionMultipleUsersNotTeam()
        {
            Assert.IsNotNull(TestUserOne);
            var actionModel = ActionModel.CreateNewActionModel(_application, TestUserOne.User.Id);

            Assert.IsTrue(actionModel.CrmUsers.Count == 1);
            Assert.IsNotNull(actionModel.CrmUsers.Single(x => x.UserId == TestUserOne.User.Id.ToString()));

            actionModel.ActionType = "Task";
            actionModel.Label = "Internal Testing (Please ignore)";
            actionModel.CrmUsers.Add(TestUserTwo.AsCrmUserRef());

            var action = _helperAction.SaveAction(actionModel);

            Assert.IsNotNull(action);

            // Not needed since a notification will get added within _helperAction.SaveAction()
            //NotificationRouter.ActionAdded(action);

            _helperAction.RemoveAction(action);
        }

        [Test]
        public void TestAddActionForTeamTeam()
        {
            Assert.IsNotNull(TestUserOne);
            var actionModel = ActionModel.CreateNewActionModel(_application, TestUserOne.User.Id);

            Assert.IsTrue(actionModel.CrmUsers.Count == 1);
            Assert.IsNotNull(actionModel.CrmUsers.Single(x => x.UserId == TestUserOne.User.Id.ToString()));

            actionModel.ActionType = "Task";
            actionModel.Label = "Internal Testing (Please ignore)";
            actionModel.CrmUsers.Add(TestUserTwo.AsCrmUserRef());
            actionModel.IsTeamAction = true;

            var action = _helperAction.SaveAction(actionModel);

            Assert.IsNotNull(action);

            // Not needed since a notification will get added within _helperAction.SaveAction()
            //NotificationRouter.ActionAdded(action);

            _helperAction.RemoveAction(action);
        }

        [Test]
        public void TestAddActionWithProgress()
        {
            Assert.IsNotNull(TestUserOne);
            var actionModel = ActionModel.CreateNewActionModel(_application, TestUserOne.User.Id);

            Assert.IsTrue(actionModel.CrmUsers.Count == 1);
            Assert.IsNotNull(actionModel.CrmUsers.Single(x => x.UserId == TestUserOne.User.Id.ToString()));

            actionModel.ActionType = "Task";
            actionModel.Label = "Internal Testing (Please ignore)";

            var action = _helperAction.SaveAction(actionModel);

            actionModel.PercentComplete = 10;
            action = _helperAction.SaveAction(actionModel);

            actionModel.PercentComplete = 50;
            action = _helperAction.SaveAction(actionModel);

            actionModel.PercentComplete = 100;
            action = _helperAction.SaveAction(actionModel);

            Assert.IsNotNull(action);

            // Not needed since a notification will get added within _helperAction.SaveAction()
            //NotificationRouter.ActionAdded(action);

            _helperAction.RemoveAction(action);


            var notifications = _helperNotifications.GetMyTeamNotifications("vulcancrm", TestUserOne.User.Id);
        }

    }
}