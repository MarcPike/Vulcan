using System.Collections.Generic;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperExternalLogin
    {
        List<ExternalLoginModel> GetAllExternalLogins(string adminUserId);
        ExternalLoginModel GetNewExternalLogin(string adminUserId);
        ExternalLoginModel Login(string userName, string password);
        ExternalLoginModel SaveExternalLogin(ExternalLoginModel model);
        List<ExternalLoginModel> RemoveExternalLogin(string externalLoginId, string adminUserId);
    }
}