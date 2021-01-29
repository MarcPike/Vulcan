using System;
using System.Collections.Generic;
using System.Text;
using BI.DAL.Mongo.AppPermissions;
using BI.DAL.Mongo.BiQueries;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace BI.DAL.Mongo.BiUserObjects
{
    public class BiUser : BaseDocument
    {
        public static MongoRawQueryHelper<BiUser> Helper = new MongoRawQueryHelper<BiUser>();
        public LdapUserRef User { get; set; }
        public Person Person { get; set; }
        public string UserId { get; set; }

        public List<AppPermissionRef> AppPermissions { get; set; } = new List<AppPermissionRef>();

        public LocationRef Location {get;set;}
        public bool SystemAdmin { get; set; } = false;

        public List<BiQueryBase> Queries { get; set; } = new List<BiQueryBase>();

        public BiUser()
        {
            
        }

        public BiUser(LdapUser ldapUser, bool isAdmin = false)
        {
            User = ldapUser.AsLdapUserRef();
            
            SystemAdmin = isAdmin;
            Person = ldapUser.Person;
            UserId = User.Id;
            Location = ldapUser.Location;
        }

        public BiUserRef AsBiUserRef()
        {
            return new BiUserRef(this);
        }
    }
}
