using System;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Helpers
{
    public static class SecurityRoleHelper
    {
        public static SystemModule GetSecurityModule(SecurityRole role, string moduleName)
        {
            var employeeDetails = role.Modules.FirstOrDefault(x => x.ModuleType.Name == moduleName);

            if (employeeDetails == null) throw new Exception($"HrsUser does not have access to module [{moduleName}]");

            return employeeDetails;
        }

        public static SecurityRole GetHrsSecurityRole(HrsUser hrsUser)
        {
            var roleId = hrsUser.HrsSecurity.RoleId;
            if (roleId == ObjectId.Empty)
            {
                throw new Exception("HrsUser does not have HrsSecurity defined");
            }
            var roleHelper = new MongoRawQueryHelper<SecurityRole>();

            var roleFilter = roleHelper.FilterBuilder.Eq(x => x.Id, roleId);
            var role = roleHelper.Find(roleFilter).FirstOrDefault();

            if (role == null) throw new Exception("HrsUser does not have HrsSecurity defined");
            return role;
        }

        public static SecurityRole GetHseSecurityRole(HrsUser hrsUser)
        {
            var roleId = hrsUser.HseSecurity.RoleId;
            if (roleId == ObjectId.Empty)
            {
                throw new Exception("HrsUser does not have HseSecurity defined");
            }
            var roleHelper = new MongoRawQueryHelper<SecurityRole>();

            var roleFilter = roleHelper.FilterBuilder.Eq(x => x.Id, roleId);
            var role = roleHelper.Find(roleFilter).FirstOrDefault();

            if (role == null) throw new Exception("HrsUser does not have HseSecurity defined");
            return role;
        }

    }
}