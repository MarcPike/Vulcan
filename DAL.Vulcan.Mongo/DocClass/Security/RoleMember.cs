using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.DocClass.Security
{
    public class RoleMember: BaseDocument
    {
        public LdapUser User { get; set; }
        public bool IsRevoked { get; set; } = false;
        public bool IsAdmin { get; set; } = false;


    }
}