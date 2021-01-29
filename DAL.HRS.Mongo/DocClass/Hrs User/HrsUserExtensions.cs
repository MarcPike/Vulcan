using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.DocClass.Hrs_User
{
    public static class HrsUserExtensions
    {
        public static List<SystemModule> GetMyModules(this HrsUser user)
        {
            var result = new List<SystemModule>();

            var hrsSecurityRole = user.HrsSecurity.GetRole();
            if (hrsSecurityRole != null) result.AddRange(hrsSecurityRole.Modules);

            var hseSecurityRole = user.HseSecurity.GetRole();
            if (hseSecurityRole != null) result.AddRange(hseSecurityRole.Modules);

            return result;
        }

        public static HrsUserModel GetModel(this HrsUser user)
        {
            return new HrsUserModel(user);
        }

        public static HrsUserRef GetHrsUserRef(this HrsUser user)
        {
            return new HrsUserRef(user);
        }

    }
}
