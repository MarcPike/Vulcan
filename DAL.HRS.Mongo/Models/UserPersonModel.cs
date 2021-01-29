using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Ldap;

namespace DAL.HRS.Mongo.Models
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