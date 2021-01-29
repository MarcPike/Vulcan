using DAL.Vulcan.Mongo.DocClass.Security;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperApplication
    {
        bool ApplicationExists(string application);
        SecurityManager GetSecurityManager(string application);
        void VerifyApplication(string application);
        void VerifyApplicationRole(string roleName);
    }
}