using System.Collections.Generic;
using BI.DAL.Mongo.BiUserObjects;
using BI.DAL.Mongo.Models;
using BI.DAL.Mongo.Security;
using DAL.Common.DocClass;

namespace BI.DAL.Mongo.Helpers
{
    public interface IHelperUser
    {
        List<BiUserModel> GetAllBiUsers();
        LdapUser LookupUserByNetworkId(string networkId);
        BiUserModel GetBiUserModel(string userId);
        BiUserModel SaveBiUser(BiUserModel model);
        LdapUser GetLdapUser(string userId);
        List<LdapUserRef> GetAllLdapUsers(string lastNameContains);
        (BiUserToken token, bool expired, LdapUser user) GetUserToken(string userId);
        BiUserToken GetNewUserToken(string userId);
        BiUser GetBiUser(string userId);
        List<LdapUserRef> GetNewUserList();
    }
}