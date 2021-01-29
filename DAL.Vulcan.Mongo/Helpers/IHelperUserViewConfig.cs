using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperUserViewConfig
    {
        ViewConfig SetViewConfig(string application, string userId, string salesTeamId = null);
        ViewConfig GetViewConfig(string application, string userId);
        string GetDefaultRoleName(string application, string userId);
    }
}