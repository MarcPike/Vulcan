using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.DocClass.SecurityRole
{
    //public static class SecurityRoleFactory
    //{
    //    private static RepositoryBase<SecurityRole> _repository; 
    //    public static SecurityRole GetSecurityRole(string roleTypeId)
    //    {
    //        var roleType = new RepositoryBase<SecurityRoleType>().Find(roleTypeId);
    //        _repository = new RepositoryBase<SecurityRole>();
    //        var securityRole = _repository.AsQueryable().FirstOrDefault(x => x.RoleType.Id == roleTypeId);
    //        if (securityRole == null)
    //        {
    //            securityRole = new SecurityRole()
    //            {
    //                RoleType = roleType.AsSecurityRoleTypeRef()
    //            };
    //            securityRole = _repository.Upsert(securityRole);
    //        }

    //        return securityRole;
    //    }

    //}
}
