using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using System.Collections.Generic;

namespace DAL.HRS.Mongo.Models
{
    public class UserAppPermissionsModel
    {
        public string Id { get; set; }
        public HrsUserRef User { get; set; }
        public SecurityRoleTypeRef SecurityRoleType { get; set; }
        public SystemModuleTypeRef ModuleType { get; set; }
        public List<AppPermissionRef> GrantedAppPermissions { get; set; } = new List<AppPermissionRef>();
        public List<AppPermissionRef> RevokedAppPermissions { get; set; } = new List<AppPermissionRef>();

        public UserAppPermissionsModel()
        {
        }

        public UserAppPermissionsModel(UserAppPermissions perm)
        {
            Id = perm.Id.ToString();
            User = perm.User;
            SecurityRoleType = perm.SecurityRoleType;
            ModuleType = perm.ModuleType;
            GrantedAppPermissions = perm.GrantedAppPermissions;
            RevokedAppPermissions = perm.RevokedAppPermissions;
        }

    }
}
