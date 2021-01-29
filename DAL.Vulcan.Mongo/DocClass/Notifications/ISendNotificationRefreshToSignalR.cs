using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public interface ISendNotificationRefreshToSignalR
    {
        Task SendRefreshRemindersForUser(CrmUserRef crmUserRef, NotificationRef notificationRef, string type);
        Task SendRefreshNotificationsForUser(CrmUserRef crmUserRef, string label);
        Task SendRefreshActionsForUser(CrmUserRef crmUserRef);
        Task SendRefreshEmailForUser(string userId);
        Task SendRefreshEmailForContact(string contactId);
        Task YourTeamSettingsHaveChanged(CrmUserRef userRef);
    }
}