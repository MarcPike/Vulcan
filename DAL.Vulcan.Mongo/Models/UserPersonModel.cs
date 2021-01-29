using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.Models
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