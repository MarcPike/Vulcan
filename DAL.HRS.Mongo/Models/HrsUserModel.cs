using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using System;
using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class HrsUserModel
    {
        public string UserId { get; set; } = string.Empty;
        public bool SystemAdmin { get; set; } = false;

        public EmployeeRef Employee { get; set; }

        public string Locale { get; set; }
        
        public LocationRef Location { get; set; }

        public EntityRef Entity { get; set; }

        public LdapUserRef User { get; set; }

        public HrsSecurityModel HrsSecurity { get; set; }
        public HseSecurityModel HseSecurity { get; set; }

        public List<AppPermissionRef> RevokedPermissions { get; set; } = new List<AppPermissionRef>();

        public List<AppPermissionRef> AllowedPermissions { get; set; } = new List<AppPermissionRef>();

        public bool ViewAllEntities { get; set; } 

        public HrsUserModel()
        {
        }

        public HrsUserModel(HrsUser user)
        {
            try
            {
                UserId = user.UserId;
                User = user.User;
                SystemAdmin = user.SystemAdmin;
                RevokedPermissions = user.RevokedPermissions;
                AllowedPermissions = user.AllowedPermissions;

                Locale = user.Location?.AsLocation().Locale ?? "en-US";


                var hrsSecurityRole = user.HrsSecurity.GetRole();
                HrsSecurity = hrsSecurityRole != null ? new HrsSecurityModel(user.HrsSecurity) : null;

                var hseSecurityRole = user.HseSecurity.GetRole();
                HseSecurity = hseSecurityRole != null ? new HseSecurityModel(user.HseSecurity) : null;

                Employee = user?.Employee;
                Location = user?.Location;

                Entity = user.Entity;
                ViewAllEntities = user.ViewAllEntities;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
