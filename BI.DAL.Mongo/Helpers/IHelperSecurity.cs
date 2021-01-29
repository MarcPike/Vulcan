using BI.DAL.Mongo.Models;
using BI.DAL.Mongo.Security;
using System.Collections.Generic;

namespace BI.DAL.Mongo.Helpers
{
    public interface IHelperSecurity
    {
        string EncodeToBase64(string normalText);
        string DecodeFrom64(string encodedData);
        BiUserToken Authenticate(string networkIdEncrypted, string passwordEncrypted);
        bool CheckAuthenticate(string userName, string password);
        BiUserToken Impersonate(string adminUserIdEncrypted, string networkIdEncrypted);
        
        BiUserModel AddUser(string userId, bool isAdmin);
        BiUserModel GetNewBiUserModel();

        void RemoveBiUser(string userId);
        BiUserModel GetBiUserModel(string userId);
        BiUserModel SaveBiUserModel(BiUserModel model);

        Dictionary<string, object> GetParametersDictionary();

        AppPermissionModel CreateAppPermission(string label, string description);
        void RemoveAppPermission(string appPermissionId);
        List<AppPermissionModel> GetAllAppPermissions();

        bool GetHasAppPermission(string userId, string permissionLabel);
    }
}