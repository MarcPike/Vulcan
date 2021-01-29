using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperTeamHub
    {
        void RefreshNotificationsForUser(CrmUserRef crmUserRef);

        void RefreshActionsForUser(CrmUserRef crmUserRef);
    }
}