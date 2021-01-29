using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Security;
using DAL.HRS.Mongo.Models;
using System.Collections.Generic;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperUser
    {
        List<HrsUserGridModel> GetAllHrsUsers();
        LdapUser LookupUserByNetworkId(string networkId);
        UserPersonModel GetUserPersonModel(string userId);
        UserPersonModel SaveUserPersonModel(UserPersonModel model);
        LdapUser GetUser(string userId);
        List<LdapUserRef> GetAllLdapUsers(string lastNameContains);
        (HrsUserToken token, bool expired, LdapUser user) GetUserToken(string userId);
        HrsUserToken GetNewUserToken(string userId);
        HrsUserModel GetUserModel(string userId);

        HrsUserModel SaveUser(HrsUserModel model);

        HrsUser GetHrsUser(string userId);

        List<LdapUserRef> GetNewUserList();
        //HrsUserModel AddHrsUser(string userId, string roleName);
        //HrsUserModel AddHseUser(string userId, string roleName);
        HrsUserModel ChangeHrsUserRole(string userId, string roleName);
        HrsUserModel ChangeHseUserRole(string userId, string roleName);
        HrsUserModel CopyHrsSecurityRole(string fromUserId, string toUserId);
        HrsUserModel CopyHseSecurityRole(string fromUserId, string toUserId);
    }
}