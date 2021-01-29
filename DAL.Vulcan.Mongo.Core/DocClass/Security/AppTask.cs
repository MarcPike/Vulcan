using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.Core.DocClass.Security
{
    public class AppTask: BaseDocument
    {
        public string Name { get; set; }

        public List<Role> Roles { get; set; } = new List<Role>();

        public List<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

        public List<LdapUser> GetAllUsersWithPermission()
        {
            var result = 
                (from role in Roles from roleMember in role.RoleMembers
                 where roleMember.IsRevoked == false
                 select roleMember.User).ToList();

            result.AddRange(UserPermissions.Where(x=>x.IsRevoked == false).Select(x=>x.User));
            return result;
        }

        public List<LdapUser> GetAllUsersWithoutPermission()
        {
            var usersWithPermission = GetAllUsersWithPermission();
            var allUsers = new RepositoryBase<LdapUser>().AsQueryable().ToList();

            return allUsers.Where(x => usersWithPermission.All(y => y.Id != x.Id)).ToList();

        }
    }
}