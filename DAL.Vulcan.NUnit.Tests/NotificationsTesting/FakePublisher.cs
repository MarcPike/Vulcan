using System;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.PublishSignalR;

namespace DAL.Vulcan.NUnit.Tests.NotificationsTesting
{
    public class FakePublisher: ISendNotificationRefreshToSignalR
    {
        private readonly HelperPerson _helperPerson;
        private readonly HelperUser _helperUser;
        private readonly HelperContact _helperContact;

        public FakePublisher()
        {
            _helperPerson = new HelperPerson();
            _helperUser = new HelperUser(_helperPerson);
            _helperContact = new HelperContact(_helperPerson,_helperUser);
        }

        public async Task SendRefreshRemindersForUser(CrmUserRef crmUserRef, NotificationRef notificationRef, string type)
        {
            Console.WriteLine($"Send Refresh Reminders for User: {crmUserRef.FullName} Notification: {notificationRef.Label} for Type: {type}");
            await PublishSignalREvents.SendNewReminderToUser(crmUserRef, notificationRef, type);
        }

        public async Task SendRefreshNotificationsForUser(CrmUserRef crmUserRef, string label)
        {
            Console.WriteLine($"Send Refresh Notifications for User: {crmUserRef.FullName} Label: {label}");
            await PublishSignalREvents.SendRefreshNotificationsForUser(crmUserRef);
        }

        public async Task SendRefreshActionsForUser(CrmUserRef crmUserRef)
        {
            Console.WriteLine($"Send Refresh Actions for User: {crmUserRef.FullName}");
            await PublishSignalREvents.RefreshActionsForUser(crmUserRef);
        }

        public async Task SendRefreshEmailForUser(string userId)
        {
            var crmUser = _helperUser.GetCrmUser("vulcancrm", userId);

            Console.WriteLine($"Send Refresh Email for UserId: {userId}");

            await PublishSignalREvents.SendRefreshEmailToUser(crmUser.AsCrmUserRef());
        }

        public async Task SendRefreshEmailForContact(string contactId)
        {
            var contact = _helperContact.GetContact(contactId);
            Console.WriteLine($"Send Refresh Email for ContactId: {contactId}");
            await PublishSignalREvents.PublishRefreshEmailForContact(contact.AsContactRef());
        }

        public async Task YourTeamSettingsHaveChanged(CrmUserRef userRef)
        {
            Console.WriteLine($"Your Team settings have changed: {userRef.FullName}");
            await PublishSignalREvents.PublishYourTeamSettingsHaveChanged(userRef);

        }
    }
}