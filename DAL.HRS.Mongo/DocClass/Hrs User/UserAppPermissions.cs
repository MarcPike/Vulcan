using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hrs_User
{
    public class UserAppPermissions : BaseDocument
    {
        public HrsUserRef User { get; set; }
        public SecurityRoleTypeRef SecurityRoleType { get; set; }
        public SystemModuleTypeRef ModuleType { get; set; }
        public List<AppPermissionRef> GrantedAppPermissions { get; set; } = new List<AppPermissionRef>();
        public List<AppPermissionRef> RevokedAppPermissions { get; set; } = new List<AppPermissionRef>();
    }
}
