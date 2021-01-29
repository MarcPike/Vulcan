using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Mongo.Models;
using Action = DAL.Vulcan.Mongo.DocClass.CRM.Action;
using CrmUser = DAL.Vulcan.Mongo.DocClass.CRM.CrmUser;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperNotifications
    {
        void UserRemovedFromTeam(CrmUser crmUser, TeamRef team);
        void UserAddedToTeam(CrmUser crmUser, TeamRef team);
        void TeamCreated(TeamRef team);
        void CompanyAddedToTeam(TeamRef team, CompanyRef company);
        void CompanyGroupAddedToTeam(TeamRef team, CompanyGroupRef group);
        void CompanyGroupRemovedFromTeam(TeamRef team, CompanyGroupRef group);
        void CompanyRemovedFromTeam(TeamRef team, CompanyRef company);
        void ActionWasAdded(Action action);
        void ActionWasCompleted(Action action);
        void ActionWasStarted(Action action);
        void ActionWasModified(Action action);
        void GoalWasAdded(Goal goal);
        void MilestoneAddedToGoal(Goal goal, Milestone milestone);
        void ActionWasAddedToGoal(Action action, Goal goal);
        void RemovedActionFromGoal(Action action, Goal goal);
        void ActionAssignedToMilestone(Goal goal, Action action, Milestone milestone);
        void RemoveNotificationsLinkedToGoal(Goal goal);

        List<NotificationModel> GetMyNotifications(string application, string userId);
        List<NotificationModel> GetMyTeamNotifications(string application, string userId);
        Notification GetNotification(string notificationId);
        void RemoveNotification(Notification notification);
        void SendRefreshNotificationsToUser(CrmUserRef crmUser, Notification notification);
        void ActionWasRemoved(ActionRef asActionRef);
    }
}