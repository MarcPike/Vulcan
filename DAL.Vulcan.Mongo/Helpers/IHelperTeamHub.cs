using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperTeamHub
    {
        void RefreshNotificationsForUser(CrmUserRef crmUserRef);

        void RefreshActionsForUser(CrmUserRef crmUserRef);
    }
}