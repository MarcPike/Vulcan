using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Helpers;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public class NotificationPublisher
    {
        public bool SignalRRefreshDefined = false;
        public ISendNotificationRefreshToSignalR SignalRPublisher { get; set; } = null;
        private readonly HelperUser _helperUser = new HelperUser(new HelperPerson());

        public void YourTeamSettingsHaveChanged(CrmUserRef userRef)
        {
            SignalRPublisher.YourTeamSettingsHaveChanged(userRef);
        }

        public void SendNewReminderToUser(CrmUser user, Notification notification)
        {
            
            SignalRPublisher.SendRefreshRemindersForUser(user.AsCrmUserRef(), notification.AsNotificationRef(), notification.SecondaryObjectType.ToString());
        }

        public void RegisterSignalR(ISendNotificationRefreshToSignalR instance)
        {
            if (instance != null)
            {
                SignalRPublisher = instance;
                SignalRRefreshDefined = true;
            }
        }

        public void SendRefreshActionsToUser(CrmUserRef crmUserRef)
        {
            if (SignalRRefreshDefined)
            {
                var crmUser = crmUserRef.AsCrmUser();
                SignalRPublisher.SendRefreshActionsForUser(crmUser.AsCrmUserRef());
            }
        }

        public void SendRefreshNotificationsToUser(CrmUserRef crmUserRef, Notification notification)
        {
            if (SignalRRefreshDefined)
            {
                var crmUser = crmUserRef.AsCrmUser();
                SignalRPublisher.SendRefreshNotificationsForUser(crmUser.AsCrmUserRef(), notification.Label);
            }
        }

        public void SendRefreshNotificationsToUser(CrmUserRef crmUserRef)
        {
            if (SignalRRefreshDefined)
            {
                SignalRPublisher.SendRefreshNotificationsForUser(crmUserRef, string.Empty);
            }
        }

        public void PublishToEntireTeam(Team team, Notification notification)
        {

            foreach (var crmUserRef in team.CrmUsers.ToList())
            {
                var crmUser = crmUserRef.AsCrmUser();
                if (crmUser == null)
                {
                    team.CrmUsers.Remove(crmUserRef);
                    continue;
                }

                crmUser.Notifications.Add(notification.AsNotificationRef());
                crmUser.SaveToDatabase();
                SendRefreshNotificationsToUser(crmUser.AsCrmUserRef(), notification);
            }
        }

        public void PublishNotificationForAction(string application, Action action, Notification notification)
        {

            action.CreateLink(notification);

            PublishNotificationToActionCreator(application, action, notification);


            if (!action.IsTeamAction)
            {
                foreach (var crmUserRef in action.CrmUsers.Where(x => x.UserId != action.CreatedByUserId).ToList())
                {
                    PublishNotificationToCrmUser(crmUserRef.AsCrmUser(),notification);
                    SendRefreshActionsToUser(crmUserRef);
                }
            }

            if ((action.IsTeamAction) && (action.Team != null))
            {
                PublishNotificationToAllTeamMembersExceptOne(application, notification, action.Team.AsTeam(), action.CreatedByUserId);
            }
        }

        public void PublishNotificationToCrmUser(CrmUser crmUser, Notification notification)
        {

            crmUser.Notifications.Add(notification.AsNotificationRef());
            crmUser.SaveToDatabase();

            SendRefreshNotificationsToUser(crmUser.AsCrmUserRef(), notification);
        }

        public void PublishNotificationToActionCreator(string application, Action action, Notification notification)
        {
            var createByCrmUser = _helperUser.GetCrmUser(application, action.CreatedByUserId);
            var userRef = createByCrmUser.AsCrmUserRef();

            createByCrmUser.Notifications.Add(notification.AsNotificationRef());
            createByCrmUser.SaveToDatabase();

            SendRefreshNotificationsToUser(userRef, notification);
            SendRefreshActionsToUser(userRef);
        }

        public void PublishNotificationToAllActionUsersExceptForOne(Action action, Notification notification, string userId)
        {
            foreach (var crmUser in action.CrmUsers.Select(x=>x.AsCrmUser()).ToList())
            {
                var notificationRef = notification.AsNotificationRef();
                if (crmUser.User.Id != userId)
                {
                    crmUser.Notifications.Add(notificationRef);
                    crmUser.SaveToDatabase();
                    action.Notifications.AddReferenceObject(notification.AsNotificationRef());

                    var userRef = crmUser.AsCrmUserRef();

                    SendRefreshNotificationsToUser(userRef,notification);
                    SendRefreshActionsToUser(userRef);
                }
            }
        }

        public void PublishNotificationForGoalToTeam(string application, Goal goal, Notification notification)
        {
            PublishToGoalCreator(application, goal, notification);
            notification.CreateLink(goal);

            if (goal.Team == null) return;

            var team = goal.Team.AsTeam();

            notification.CreateLink(team);

            foreach (var crmUserRef in team.CrmUsers.ToList())
            {
                PublishNotificationToCrmUser(crmUserRef.AsCrmUser(),notification);
            }
        }

        public void PublishNotificationToAllTeamMembersExceptOne(string application, Notification notification, Team team, string excludeUserId)
        {
            foreach (var crmUser in team.CrmUsers.Select(x=>x.AsCrmUser()).ToList())
            {
                if (crmUser.User.Id != excludeUserId)
                {
                    crmUser.Notifications.Add(notification.AsNotificationRef());
                    crmUser.SaveToDatabase();
                    var userRef = crmUser.AsCrmUserRef();
                    //notification.CreateLink(manager);
                    team.Notifications.AddReferenceObject(notification.AsNotificationRef());
                    SendRefreshNotificationsToUser(userRef,notification);
                    SendRefreshActionsToUser(userRef);
                }
            }
        }

        public void PublishNotificationToAllTeamMembersExceptOne(Notification notification, Team team, string excludeUserId)
        {
            foreach (var crmUser in team.CrmUsers.Select(x => x.AsCrmUser()).ToList())
            {
                if (crmUser.User.Id != excludeUserId)
                {
                    crmUser.Notifications.Add(notification.AsNotificationRef());
                    crmUser.SaveToDatabase();
                    //notification.CreateLink(manager);
                    team.Notifications.AddReferenceObject(notification.AsNotificationRef());
                    SendRefreshNotificationsToUser(crmUser.AsCrmUserRef(), notification);
                }
            }
        }

        public void PublishToGoalCreator(string application, Goal goal, Notification notification)
        {
            var crmUser = _helperUser.GetCrmUser(application, notification.CreatedByUserId);
            crmUser.Notifications.Add(notification.AsNotificationRef());
            SendRefreshNotificationsToUser(crmUser.AsCrmUserRef(), notification);
        }

    }
}