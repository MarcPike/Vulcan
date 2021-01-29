using AspNetCore.Identity.MongoDB.Validators;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Security;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;
using LdapUser = DAL.Common.DocClass.LdapUser;

namespace DAL.HRS.Mongo.DocClass.Hrs_User
{
    [BsonIgnoreExtraElements]
    public class HrsUser: BaseDocument
    {
        public string UserId { get; set; } = string.Empty;
        public bool SystemAdmin { get; set; } = false;

        public LdapUserRef User { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    
        public string MiddleName { get; set; }
        public string FullName { get; set; }

        public EmployeeRef Employee { get; set; } 
        public LocationRef Location { get; set; } 
    
        public List<AppPermissionRef> AllowedPermissions { get; set; } = new List<AppPermissionRef>();
        public List<AppPermissionRef> RevokedPermissions { get; set; } = new List<AppPermissionRef>();

        public HrsUserSecurity HrsSecurity { get; set; } = new HrsUserSecurity();
        public HseUserSecurity HseSecurity { get; set; } = new HseUserSecurity();

        public EntityRef Entity { get; set; }

        public bool ViewAllEntities { get; set; } = false;

        public static MongoRawQueryHelper<HrsUser> Helper = new MongoRawQueryHelper<HrsUser>();



        public override List<ValidationError> Validate()
        {
            var result = new List<ValidationError>();
            if (Entity == null)
                result.Add(new ValidationError()
                {
                    ErrorMessage = "Entity cannot be null"
                });
            return result;
        }

        public HrsUser()
        {
        }

        public static HrsUser GetHrsUser(LdapUser user)
        {
            var result = new RepositoryBase<HrsUser>().AsQueryable().SingleOrDefault(x => x.UserId == user.Id.ToString());
            return result;
        }

        public void ChangeLdapUserTo(LdapUser ldapUser)
        {
            User = ldapUser.AsLdapUserRef();
            UserId = ldapUser.Id.ToString();
        }

        public static HrsUser CreateHrsUser(LdapUser user, SecurityRole.SecurityRole hrsSecurityRole, SecurityRole.SecurityRole hseSecurityRole, EmployeeRef employee = null)
        {
            var helperEmployee = new HelperEmployee();

            var locationId = ObjectId.Parse(user.Location.Id);
            var location = new RepositoryBase<Location>().AsQueryable().SingleOrDefault(x => x.Id == locationId)?.AsLocationRef();

            if (location == null) throw new Exception("Location not found");

            var hrsUser = new RepositoryBase<HrsUser>().AsQueryable().SingleOrDefault(x => x.UserId == user.Id.ToString()) ??
                       new HrsUser()
                       {
                           UserId = user.Id.ToString(),
                       };
            hrsUser.User = user.AsLdapUserRef();

            var employeeHelper = DAL.HRS.Mongo.DocClass.Employee.Employee.Helper;

            if (employee == null)
            {
                var employeeFilter = employeeHelper.FilterBuilder.Where(x =>
                    x.LdapUser.ActiveDirectoryId == hrsUser.User.ActiveDirectoryId);
                employee = employeeHelper.Find(employeeFilter).FirstOrDefault()?.AsEmployeeRef();
            }

            if (employee == null) throw new Exception("No employee found for this user");

            hrsUser.HrsSecurity.RoleId = hrsSecurityRole?.Id ?? ObjectId.Empty;
            hrsUser.HseSecurity.RoleId = hseSecurityRole?.Id ?? ObjectId.Empty;
            hrsUser.Location = location;
            hrsUser.FirstName = employee.FirstName;
            hrsUser.LastName = employee.LastName;
            hrsUser.MiddleName = employee.MiddleName;
            hrsUser.FullName = employee.FullName;
            hrsUser.Employee = employee;
            return hrsUser;
        }


        public static HrsUser GetHrsUser(HrsUserToken token)
        {
            return new RepositoryBase<HrsUser>().AsQueryable().SingleOrDefault(x => x.UserId == token.User.Id);
        }

        public HrsUserRef AsHrsUserRef()
        {
            return new HrsUserRef(this);
        }

    }

    
}
