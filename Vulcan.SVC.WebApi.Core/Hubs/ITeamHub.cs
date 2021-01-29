using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Messages;
using DAL.Vulcan.Mongo.Core.DocClass.Notifications;

namespace Vulcan.SVC.WebApi.Core.Hubs
{
    public interface ITeamHub
    {
        //List<UserRef> JoinTeam(string teamId, string userId);
        //void LeaveTeam(string teamId, string userId);

        //void RefreshActivitiesForTeam(TeamRef teamRef);

        //void RefreshTasksForTeam(TeamRef teamRef);

        //void RefreshEventsForTeam(TeamRef teamRef);
        //void SendRefreshRemindersForUser(CrmUserRef crmUserRef, NotificationRef notificationRef, string type);

        //void SendTeamMessage(TeamRef teamRef, UserRef user, string message);
        Task SendRefreshPrivateMessages(string userId, MessageObject messageObject);
        Task SendRefreshTeamMessages(string teamId, MessageObject messageObject);
        Task SendRefreshGroupMessages(string groupId, MessageObject messageObject);

        Task SendRefreshNotificationsForUser(CrmUserRef crmUserRef, string label);
        Task SendRefreshActionsForUser(CrmUserRef crmUserRef);
        Task SendRefreshEmailForUser(string userId);
        Task SendRefreshEmailForContact(string contactId);

        Task SendWonQuoteMessageFromUser(string application, string userId, string quoteId);

        Task SendRefreshRemindersForUser(CrmUserRef crmUserRef, NotificationRef notificationRef, string type);

        Task YourTeamSettingsHaveChanged(CrmUserRef userRef);

        Task QuoteExportStatusChanged(string quoteId);

        Task PublishUserTeamMessage(string teamMessageId);

        TeamHub GetHub();

    }
}