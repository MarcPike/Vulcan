using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Security;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperUser
    {
        List<UserModel> GetAllAvailableNewUsersForApplication(string application, string lastName, List<UserModel> existingUsers);
        List<CrmUserRef> GetAllDirectors(string application);
        List<CrmUserRef> GetAllManagers(string application);
        List<CrmUserRef> GetAllSalesPersons(string application);
        List<UserModel> GetExistingUsersForApplication(string application);
        List<CrmUserRef> GetExistingUserReferencesForApplication(string application);
        LdapUser GetUser(string userId);
        CrmUser GetCrmUser(string application, string userId);
        CrmUserInfo GetCrmUserInfo(string application, string userId);
        UserPersonModel GetUserPersonModel(string userId);
        UserPersonModel SaveUserPersonModel(UserPersonModel model);
        (CrmUserToken token, bool expired, CrmUser crmUser) GetUserToken(string application, string userId);
        CrmUserToken GetNewUserToken(string application, string userId);
        LdapUser LookupUserByNetworkId(string networkId);
        void RemoveUser(string application, string userId);
        CrmUserModel SaveCrmUserModel(CrmUserModel model);
        CrmUserModel GetCrmUserModel(string application, string userId);
        List<UserRef> GetAllEmployees(string application);
        CrmUserModel CreateAndOrSetUserAsUserType(string application, string userId, CrmUserType userType, bool isAdmin, bool readOnly, bool isCalcAdmin);
        TeamUserCompanyViewSelectionsModel GetTeamUserCompanyViewSelectionsModel(string application, string userId);
        TeamUserCompanyViewSelectionsModel SaveTeamUserCompanyViewSelectionsModel(TeamUserCompanyViewSelectionsModel model);
        string GetUserCoid(CrmUser crmUser);
        void UserConnectedCommand(string application, string userId);
        //void ControllerMethodCalled(CrmUserRef user, string controller, string method);
        //void ControllerMethodCalled(string application, string userId, string controller, string method);

        CrmUserModel ChangeUserLocation(string application, string userId, string userIdToModify, string moveToLocationId);
    }
}