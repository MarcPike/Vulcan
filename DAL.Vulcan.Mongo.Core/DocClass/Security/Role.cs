using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.Core.DocClass;

namespace DAL.Vulcan.Mongo.Core.DocClass.Security
{
    public class Role : BaseDocument
    {
        public string Name { get; set; }
        public List<RoleMember> RoleMembers { get; set; } = new List<RoleMember>();

    }
}