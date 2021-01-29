using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Security;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Models;
using System.Collections.Generic;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperSecurity
    {
        string EncodeToBase64(string normalText);
        string DecodeFrom64(string encodedData);
        HrsUserToken Authenticate(string networkIdEncrypted, string passwordEncrypted);
        HrsUserToken Impersonate(string adminUserIdEncrypted, string networkIdEncrypted);

        AppPermissionModel CreateAppPermission(string label, string description);
        void RemoveAppPermission(string appPermissionId);
        List<AppPermissionModel> GetAllAppPermissions();

        List<SecurityRoleModel> GetAllRoles();
        List<SecurityRoleTypeRef> GetAllRoleTypes();
        List<SystemModuleTypeRef> GetAllModuleTypes();
        List<SystemModuleModel> GetAllModules();


        SecurityRoleTypeRef DefineNewRoleType(string roleName, bool isHrs, bool isHse, bool autoSave=false);
        SystemModuleTypeRef DefineNewModuleType(string name, bool isHrs, bool isHse);

        SecurityRoleModel SaveRole(SecurityRoleModel model);
        //SystemModuleModel SaveModule(SystemModuleModel model);
        List<HrsUserModel> UsersInRole(string roleId);
        void RemoveRole(string roleId);
        void RemoveModuleType(string moduleTypeId);
        bool GetHasHrsPermission(string userId, string moduleTypeId, string permissionLabel);
        bool GetHasHsePermission(string userId, string moduleTypeId, string permissionLabel);
        SecurityRoleModel GetSecurityRoleModelForName(string roleName);
        //SecurityRoleModel GetSecurityRoleModel(string securityRoleTypeId);

        SecurityRoleModel GetSecurityRoleModelForRoleType(string roleTypeId);
        SecurityRoleModel GetSecurityRoleModel(string roleId);

        SystemModuleModel GetSystemModuleModel(string moduleTypeId);
        SystemModuleModel GetSystemModuleModel(SystemModuleType moduleType);

        FileOperations GetFileOperationsForModule(string userId, string moduleTypeId);

        List<NewHrsUserModel> GetHrsNewUserList();
        HrsUserModel AddUser(string employeeId, string userId, string hrsRoleId, string hseRoleId, EntityRef entity);

        void RemoveHrsUser(string userId);
        //List<LocationRef> GetHrsUserRoleModuleLocations(string userId,
        //    string securityRoleTypeId, string systemModuleTypeId);

        //List<LocationRef> AddHrsUserRoleModuleLocation(string userId,
        //    string securityRoleTypeId, string systemModuleTypeId, string locationId);

        //List<LocationRef> RemoveHrsUserRoleModuleLocation(string userId,
        //    string securityRoleTypeId, string systemModuleTypeId, string locationId);

        SecurityRoleTypeRef GetRoleTypeForName(string roleName);
        SystemModuleTypeRef GetHrsModuleTypeForName(string moduleName);
        SystemModuleTypeRef GetHseModuleTypeForName(string moduleName);

        UserAppPermissionsModel GetUserHrsAppPermissionForModule(string userId, string moduleTypeId);
        UserAppPermissionsModel GetUserHseAppPermissionForModule(string userId, string moduleTypeId);

        UserAppPermissionsModel SaveUserAppPermissionsForModule(UserAppPermissionsModel model);

        HrsUserModel GetHrsUserModel(string hrsUserId);
        HrsUserModel SaveHrsUserModel(HrsUserModel model);

        Dictionary<string, object> GetParametersDictionary();


        SecurityRoleModel CreateNewRole(SecurityRoleTypeRef roleTypeRef);
    }
}