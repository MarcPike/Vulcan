using System.Collections;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.Models
{
    public class UserModel: IEqualityComparer<UserModel>
    {
        public string Id { get; set; }
        public string NetworkId { get; set; }
        public string UserName { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }
        public string Office { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public bool IsAdmin { get; set; }
        public bool ReadOnly { get; set; }
        public bool IsCalcAdmin { get; set; }

        public UserModel(CrmUser crmUser)
        {
            var user = crmUser.User.AsUser();
            Id = user.Id.ToString();
            NetworkId = user.NetworkId;
            UserName = user.UserName;
            Country = user.Location.AsLocation().Country;
            if (user.Location == null)
                return;
            Region = user.Location.AsLocation().Region;
            Branch = user.Location.AsLocation().Branch;
            Office = user.Location.Office;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserType = crmUser.UserType.ToString();
            IsAdmin = crmUser.IsAdmin;
            ReadOnly = crmUser.ReadOnly;
            IsCalcAdmin = crmUser.IsCalcAdmin;

        }

        public UserModel(LdapUser user)
        {
            Id = user.Id.ToString();
            NetworkId = user.NetworkId;
            UserName = user.UserName;
            Country = user.Location.AsLocation().Country;
            if (user.Location == null)
                return;
            Region = user.Location.AsLocation().Region;
            Branch = user.Location.AsLocation().Branch;
            Office = user.Location.Office;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserType = "(none)";
            IsAdmin = false;
            IsCalcAdmin = false;

            ReadOnly = true;

            var crmUser = CrmUser.Helper.Find(x => x.User.Id == Id).FirstOrDefault();
            if (crmUser != null)
            {
                ReadOnly = crmUser.ReadOnly;
            }

        }

        public bool Equals(UserModel x, UserModel y)
        {
            if ((x == null) || (y == null)) return false;

            return x.Id == y.Id;
        }

        public int GetHashCode(UserModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
