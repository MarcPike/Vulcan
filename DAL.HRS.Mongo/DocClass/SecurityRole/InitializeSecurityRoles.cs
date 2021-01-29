using System.Configuration;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.DocClass.SecurityRole
{
    public static class InitializeSecurityRoles
    {
        private static HelperSecurity _helperSecurity = new HelperSecurity();
        private static HelperUser _helperUser = new HelperUser();

        public static void Execute()
        {
            /*
            var rep = new RepositoryBase<SecurityRole>();
            var securityRoleAdmin = SecurityRoleFactory.CreateHrsSecurityRole("SystemAdmin");

            if (!securityRoleAdmin.Modules.Any())
            {
                securityRoleAdmin.Modules.Add(new SystemModule()
                {
                    Name = "SystemAdmin",
                    Add = true,
                    Modify = true,
                    Delete = true,
                    View = true,
                    CanViewAllEmployees = true,
                });
                rep.Upsert(securityRoleAdmin);
            }
            */
        }
    }
}