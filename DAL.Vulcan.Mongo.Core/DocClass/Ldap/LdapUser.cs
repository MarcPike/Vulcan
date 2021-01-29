using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using DAL.Vulcan.Mongo.Core.DocClass.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.DocClass.Ldap
{
    public class LdapUser : BaseDocument
    {
        public string NetworkId { get; set; }
        public string UserName { get; set; }
        public LocationRef Location { get; set; }
        public Person Person { get; set; } = null;

        public string Coid
        {
            get
            {
                try
                {
                    var location = Location.AsLocation();
                    if (location.Branch == "USA") return "INC";
                    return location.Branch;
                }
                catch (Exception)
                {
                    return "";
                }

            }
        }

        public string LastName
        {
            get
            {
                if (UserName.IndexOf(',') == -1) return string.Empty;
                return UserName.Substring(0, UserName.IndexOf(','));
            }
        }

        public string FullName => $"{FirstName} {LastName}";

        public string FirstName
        {
            get
            {
                if (UserName.IndexOf(',') == -1) return UserName;
                var indexOf = UserName.IndexOf(',');
                return UserName.Substring(indexOf + 2, UserName.Length-indexOf-2);
            }
        }

        public Person GetDefaultPersonIfMissing()
        {
            if (Person == null)
            {
                Person = new Person(this);
                SaveToDatabase();
            }
            return Person;
        }

        public List<string> GetApplicationRoles(string application)
        {
            var securityManager = new SecurityManager();
            if (!securityManager.AppExists(application))
            {
                throw new Exception($"No application exists with name: {application}");
            }

            var result = new List<ApplicationRole>();
            securityManager.ForApplication(application);
            var roles = securityManager.GetAllApplicationRoles();
            foreach (var role in roles)
            {
                if (role.Role.RoleMembers.Any(x => x.User.Id == Id))
                {
                    result.Add(role);
                }
            }
            return result.Select(x=>x.Role.Name).ToList();
        }

        public UserRef AsUserRef()
        {
            return new UserRef(this);
        }

        public LdapUser Save()
        {
            SaveToDatabase();
            return this;
        }
    }

}
