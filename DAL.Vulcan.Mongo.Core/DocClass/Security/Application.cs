using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.Core.DocClass.Security
{
    public  class Application: BaseDocument
    {
        public string Name { get; set; }
        public List<AppTask> Tasks { get; set; } = new List<AppTask>();
        public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();

        public List<LdapUser> Users { get; set; } = new List<LdapUser>();
    }
}
