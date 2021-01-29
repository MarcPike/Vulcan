using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperPermissions
    {
        PermissionModel AddNewPermission(string application, string userId, string name);
        void RemovePermission(string id);
        PermissionModel GetPermissionModelForId(string application, string userId, string id);
        PermissionModel GetPermissionModelForName(string application, string userId, string name);
        PermissionModel SavePermissionModel(PermissionModel model);
        bool UserHasPermissionForPermissionId(string application, string userId, string permissionId);
        bool UserHasPermissionForPermissionName(string application, string userId, string permissionName);
        List<PermissionModel> GetAllPermissions(string application, string userId);
        List<PermissionModel> GetAllPermissionsForUserId(string application, string userId);
    }
}