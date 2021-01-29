using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Permissions;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperPermissions : HelperBase, IHelperPermissions
    {
        private  HelperUser _helperUser => new HelperUser(new HelperPerson());
        public PermissionModel AddNewPermission(string application, string userId, string name)
        {
            var rep = new RepositoryBase<Permission>();
            var permission = rep.AsQueryable().SingleOrDefault(x => x.Name == name);
            if (permission == null)
            {
                permission = new Permission() { Name = name };
                rep.Upsert(permission);
            }

            return new PermissionModel(application, userId, permission);
        }

        public PermissionModel GetPermissionModelForId(string application, string userId, string id)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var rep = new RepositoryBase<Permission>();
            var permission = rep.Find(id);
            if (permission == null)
            {
                throw new Exception("Permission not found");
            }

            return new PermissionModel(application, crmUser.User.Id, permission);
        }

        public void RemovePermission(string id)
        {
            var rep = new RepositoryBase<Permission>();
            var permission = rep.Find(id);
            if (permission == null)
            {
                throw new Exception("Permission not found");
            }

            rep.RemoveOne(permission);
        }


        public PermissionModel GetPermissionModelForName(string application, string userId, string name)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);

            var rep = new RepositoryBase<Permission>();
            var permission = rep.AsQueryable().SingleOrDefault(x => x.Name == name);
            if (permission == null)
            {
                throw new Exception("Permission not found");
            }

            return new PermissionModel(application, crmUser.User.Id, permission);
        }

        public PermissionModel SavePermissionModel(PermissionModel model)
        {
            var rep = new RepositoryBase<Permission>();
            var permission = rep.Find(model.Id);
            if (permission == null)
            {
                permission = new Permission()
                {
                    Id = ObjectId.Parse(model.Id)
                };
            }

            permission.Name = model.Name;
            permission.Users = model.Users;
            rep.Upsert(permission);

            return new PermissionModel(model.Application, model.UserId, permission);
        }

        public bool UserHasPermissionForPermissionId(string application, string userId, string permissionId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);

            var rep = new RepositoryBase<Permission>();
            var permission = rep.Find(permissionId);
            if (permission == null)
            {
                throw new Exception("Permission not found");
            }

            return permission.Users.Any(x => x.Id == crmUser.User.Id);
        }

        public bool UserHasPermissionForPermissionName(string application, string userId, string permissionName)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);

            var rep = new RepositoryBase<Permission>();
            var permission = rep.AsQueryable().SingleOrDefault(x => x.Name == permissionName);
            if (permission == null)
            {
                throw new Exception("Permission not found");
            }

            return permission.Users.Any(x => x.UserId == crmUser.User.Id);
        }

        public List<PermissionModel> GetAllPermissions(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);

            var rep = new RepositoryBase<Permission>();
            var permissions = rep.AsQueryable().ToList();

            return permissions.Select(x => new PermissionModel(application,crmUser.User.Id, x)).ToList();
        }

        public List<PermissionModel> GetAllPermissionsForUserId(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);

            var rep = new RepositoryBase<Permission>();
            var permissions = rep.AsQueryable().Where(x=> x.Users.Any(u=>u.Id == crmUser.User.Id)).ToList();

            return permissions.Select(x => new PermissionModel(application, crmUser.User.Id, x)).ToList();
        }

    }
}
