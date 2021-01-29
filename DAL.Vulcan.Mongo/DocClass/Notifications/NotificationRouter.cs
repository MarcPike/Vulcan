using System;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.CRM;
using Action = DAL.Vulcan.Mongo.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public static class NotificationRouter
    {
        public static readonly NotificationPublisher Publisher = new NotificationPublisher();

        public static void RegisterSignalR(ISendNotificationRefreshToSignalR instance)
        {
            if (instance != null)
            {
                Publisher.RegisterSignalR(instance);
            }
        }

        public static void YourTeamSettingsHaveChanged(CrmUserRef userRef)
        {
            Publisher.YourTeamSettingsHaveChanged(userRef);
        }

        public static void ProspectAddedToTeam(Team team, Prospect prospect)
        {
            var notificationBuilder = new NotificationBuilderForProspectAddedToTeam(team, prospect);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishToEntireTeam(team, notification);
        }

        public static void ProspectRemovedFromTeam(Team team, Prospect prospect)
        {
            var notificationBuilder = new NotificationBuilderForProspectRemovedFromTeam(team, prospect);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishToEntireTeam(team, notification);
        }

        public static void TeamNameChanged(Team team, string previousName)
        {
            var notificationBuilder = new NotificationBuilderForTeamNameChanged(team, previousName);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishToEntireTeam(team, notification);
        }

        public static void MilestoneAddedToGoal(Goal goal, Milestone milestone)
        {
            var notificationBuilder = new NotificationBuilderForMilestoneAddedToGoal(goal,milestone);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishToEntireTeam(goal.Team?.AsTeam(), notification);
        }

        public static void CompanyAddedToTeam(Team team, Company company)
        {
            var notificationBuilder = new NotificationBuilderForCompanyAddedToTeam(team,company);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishToEntireTeam(team, notification);
        }

        public static void CompanyRemovedFromTeam(Team team, Company company)
        {
            var notificationBuilder = new NotificationBuilderForCompanyRemovedFromTeam(team, company);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishToEntireTeam(team, notification);
        }


        public static void CompanyGroupAddedToTeam(Team team, CompanyGroup group)
        {
            var notificationBuilder = new NotificationBuilderForCompanyGroupAddedToTeam(team,group);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishToEntireTeam(team, notification);
        }

        public static void CompanyGroupRemovedFromTeam(Team team, CompanyGroup group)
        {

            var notificationBuilder = new NotificationBuilderForCompanyGroupRemovedFromTeam(team, group);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishToEntireTeam(team, notification);
        }


        public static void UserAddedToTeam(CrmUserRef crmUserRef, Team team )
        {
            var notificationBuilder = new NotificationBuilderForUserAddedToTeam(crmUserRef,team);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishToEntireTeam(team, notification);
        }

        public static void UserRemovedFromTeam(CrmUserRef crmUserRef, Team team)
        {
            var notificationBuilder = new NotificationBuilderForUserRemovedFromTeam(crmUserRef,team);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishToEntireTeam(team, notification);
        }

        public static void TeamCreated(Team team)
        {
            var notificationBuilder = new NotificationBuilderForTeamCreated(team);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishToEntireTeam(team, notification);
        }

        public static void GoalWasAdded(Goal goal)
        {
            var notificationBuilder = new NotificationBuilderForGoalWasAdded(goal);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishNotificationForGoalToTeam("vulcancrm", goal, notification);
        }


        public static void GoalLost(Goal goal)
        {
            var notificationBuilder = new NotificationBuilderForGoalLost(goal);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishNotificationForGoalToTeam("vulcancrm", goal, notification);

        }

        public static void ActionAddedToGoal(Action action)
        {
            if (action.Goal == null) throw new Exception("Action does not have a Goal");

            var notificationBuilder = new NotificationBuilderForActionAddedToGoal(action);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishNotificationForGoalToTeam("vulcancrm", action.Goal.AsGoal(), notification);
        }

        public static void ActionRemovedFromGoal(Action action, Goal goal)
        {
            var notificationBuilder = new NotificationBuilderForActionRemovedFromGoal(action, goal);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishNotificationForGoalToTeam("vulcancrm", goal, notification);
        }
        public static void ActionAssignedToMilestone(Goal goal, Action action, Milestone milestone)
        {
            var notificationBuilder = new NotificationBuilderForActionAssignedToMilestone(goal, action, milestone);
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishNotificationForGoalToTeam("vulcancrm", goal, notification);
        }

        public static void ActionAdded(Action action)
        {
            var notificationBuilder = new NotificationBuilderForActionWasAdded(action,GetNotificationObjectTypeForAction(action));
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishNotificationForAction("vulcancrm", action, notification);
        }

        public static void ActionStarted(Action action)
        {

            var notificationBuilder = new NotificationBuilderForActionWasStarted(action, GetNotificationObjectTypeForAction(action));
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishNotificationForAction("vulcancrm", action, notification);
        }

        public static void ActionModified(Action action)
        {
            var notificationBuilder = new NotificationBuilderForActionWasModified(action,GetNotificationObjectTypeForAction(action));
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishNotificationForAction("vulcancrm", action, notification);
        }

        public static void ActionCompleted(Action action)
        {
            var notificationBuilder = new NotificationBuilderForActionWasCompleted(action,GetNotificationObjectTypeForAction(action));
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishNotificationForAction("vulcancrm", action, notification);
        }

        public static void ActionFailed(Action action)
        {
            var notificationBuilder = new NotificationBuilderForActionFailed(action, GetNotificationObjectTypeForAction(action));
            var notification = notificationBuilder.GetNotification();

            Publisher.PublishNotificationForAction("vulcancrm", action, notification);

        }

        public static void PersonAddedToAction(Action action, CrmUser crmUser)
        {
            var notificationBuilder = new NotificationBuilderForPersonAddedToAction(action,crmUser,GetNotificationObjectTypeForAction(action));

            var notification = notificationBuilder.GetNotification();
            Publisher.PublishNotificationForAction("vulcancrm", action, notification );

            var notificationForOtherUsers = notificationBuilder.GetNotificationForOtherUsers();
            Publisher.PublishNotificationToAllActionUsersExceptForOne(action, notificationForOtherUsers, crmUser.User.Id);
        }


        private static NotificationObjectType GetNotificationObjectTypeForAction(Action action)
        {
            NotificationObjectType result;
            switch (action.ActionType)
            {
                case ActionType.Email:
                {
                    result = NotificationObjectType.Email;
                    break;
                }
                case ActionType.Event:
                {
                    result = NotificationObjectType.Event;
                    break;
                }
                case ActionType.Fax:
                {
                    result = NotificationObjectType.Fax;
                    break;
                }
                case ActionType.Meeting:
                {
                    result = NotificationObjectType.Meeting;
                    break;
                }
                case ActionType.Phone:
                {
                    result = NotificationObjectType.Phonecall;
                    break;
                }
                default:
                {
                    result = NotificationObjectType.Task;
                    break;
                }
            }
            return result;
        }
    }
}
