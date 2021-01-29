using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;

namespace DAL.HRS.Mongo.Models
{
    public class HrsUserGridModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FullName { get; set; }
        public LocationRef Location { get; set; }

        public EntityRef Entity { get; set; }

        public string HrsRoleName { get; set; } = string.Empty;
        public string HseRoleName { get; set; } = string.Empty;

        public HrsUserGridModel()
        {
            
        }

        public HrsUserGridModel(HrsUser user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            MiddleName = user.MiddleName;
            FullName = user.FullName;
            Location = user.Location;
            Entity = user.Entity;
            var role = user.HrsSecurity?.GetRole();
            if (role != null) HrsRoleName = role.RoleType.Name;
            role = user.HseSecurity?.GetRole();
            if (role != null) HseRoleName = role.RoleType.Name;
        }
    }
}
