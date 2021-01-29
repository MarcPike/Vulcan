using DAL.HRS.Mongo.DocClass.SecurityRole;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.Models
{
    public class SecurityRoleModel
    {
        public string Id { get; set; } = string.Empty;
        public SecurityRoleTypeRef RoleType { get; set; }
        public List<SystemModuleModel> Modules { get; set; } = new List<SystemModuleModel>();

        public bool DirectReportsOnly { get; set; } = true;

        public List<HrsUserRef> UsersInRole = new List<HrsUserRef>();

        public SecurityRoleModel()
        {
        }

        public SecurityRoleModel(SecurityRole securityRole)
        {
            if (securityRole == null) return;
            
            Id = securityRole.Id.ToString();
            RoleType = securityRole.RoleType;
            Modules = securityRole.Modules.Select(x=> new SystemModuleModel(x)).ToList();
            DirectReportsOnly = securityRole.DirectReportsOnly;

            var hrsUsers = HrsUser.Helper
                .Find(x => x.HrsSecurity.RoleId == securityRole.Id || x.HseSecurity.RoleId == securityRole.Id).ToList();
            foreach (var hrsUser in hrsUsers)
            {
                UsersInRole.Add(hrsUser.AsHrsUserRef());
            }

        }
    }
}
