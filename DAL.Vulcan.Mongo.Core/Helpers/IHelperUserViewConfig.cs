using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperUserViewConfig
    {
        ViewConfig SetViewConfig(string application, string userId, string salesTeamId = null);
        ViewConfig GetViewConfig(string application, string userId);
        string GetDefaultRoleName(string application, string userId);
    }
}