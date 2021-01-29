using DAL.Vulcan.Mongo.Core.DocClass.Security;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperApplication
    {
        bool ApplicationExists(string application);
        SecurityManager GetSecurityManager(string application);
        void VerifyApplication(string application);
        void VerifyApplicationRole(string roleName);
    }
}