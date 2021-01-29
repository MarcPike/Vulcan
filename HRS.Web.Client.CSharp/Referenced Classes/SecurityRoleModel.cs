using System.Collections.Generic;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class SecurityRoleModel
    {
        public string Id { get; set; } = string.Empty;
        public SecurityRoleTypeRef RoleType { get; set; }
        public List<SystemModuleModel> Modules { get; set; } = new List<SystemModuleModel>();

        public bool DirectReportsOnly { get; set; } = true;
    }
}
