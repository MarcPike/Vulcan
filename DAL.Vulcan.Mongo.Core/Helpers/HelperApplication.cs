using System;
using DAL.Vulcan.Mongo.Core.DocClass.Security;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public class HelperApplication : HelperBase, IHelperApplication
    {
        public SecurityManager GetSecurityManager(string application)
        {
            var securityManager = new SecurityManager();
            if (!securityManager.AppExists(application)) throw new Exception($"No application exists: {application}");
            return securityManager.ForApplication(application);
        }

        public bool ApplicationExists(string application)
        {
            var securityManager = new SecurityManager();
            return securityManager.AppExists(application);
        }

        public void VerifyApplication(string application)
        {
            if (!ApplicationExists(application)) throw new Exception($"No application exists: {application}");
        }

        public void VerifyApplicationRole(string roleName)
        {
            if ((roleName != "Admin") && (roleName != "Manager") && (roleName != "SalesPerson"))
            {
                throw new Exception($"{roleName} is not a valid Role");
            }
        }
    }
}
