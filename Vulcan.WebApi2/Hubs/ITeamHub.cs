using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Messages;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Mongo.DocClass.Security;
using Microsoft.AspNetCore.SignalR;

namespace Vulcan.WebApi2.Hubs
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