using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class UserPersonModel: PersonModelBase {

        public string Application { get; set; }
        public string UserId { get; set; }

        public UserPersonModel(LdapUser user) : base(user.GetDefaultPersonIfMissing())
        {
            UserId = user.Id.ToString();
        }
    }
}