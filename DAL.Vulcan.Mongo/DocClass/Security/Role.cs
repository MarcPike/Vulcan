using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.DocClass.Security
{
    public class Role : BaseDocument
    {
        public string Name { get; set; }
        public List<RoleMember> RoleMembers { get; set; } = new List<RoleMember>();

    }
}