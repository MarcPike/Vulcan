using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using BI.DAL.Mongo.BiUserObjects;
using BI.DAL.Mongo.Helpers;
using BI.DAL.Mongo.Models;
using DAL.Common.DocClass;
using DAL.Common.Ldap;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Environment = System.Environment;

namespace BI.WebApi.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class SecurityController : BaseController
    {

        private readonly IHelperSecurity _helperSecurity;
        //private VulcanLogger _logger;

        public SecurityController(IHelperSecurity helperSecurity) : base()
        {
            _helperSecurity = helperSecurity;
        }

        #region System Maintenance

        [AllowAnonymous]
        [HttpGet]
        [Route("security/RefreshLdapUsers")]
        public JsonResult RefreshLdapUsers()
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                UserAuthentication.RefreshAll();
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("token.UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }
        #endregion

        #region Authentication

        [AllowAnonymous]
        [HttpGet]
        [Route("security/Impersonate/{adminUserIdEncrypted}/{networkIdEncrypted}")]
        public JsonResult Impersonate(string adminUserIdEncrypted, string networkIdEncrypted)
        {
            dynamic result = new ExpandoObject();
            var adminUserId = _helperSecurity.DecodeFrom64(adminUserIdEncrypted);
            var networkId = _helperSecurity.DecodeFrom64(networkIdEncrypted);
            var resultStatusCode = HttpStatusCode.Unauthorized;
            try
            {
                var userToken = _helperSecurity.Impersonate(adminUserIdEncrypted, networkIdEncrypted);

                result.UserToken = userToken;
                resultStatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("adminUserId", adminUserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref resultStatusCode);
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetImpersonateList/{adminUserIdEncrypted}")]
        public JsonResult GetImpersonateList(string adminUserIdEncrypted)
        {
            dynamic result = new ExpandoObject();
            var adminUserId = _helperSecurity.DecodeFrom64(adminUserIdEncrypted);
            var admin = _helperUser.GetBiUser(adminUserId);
            var tokenData = GetTokenDataFromHeaders();
            try
            {

                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                if (!admin.SystemAdmin) throw new Exception("You are not authorized");

                result.UserList = new RepositoryBase<BiUser>().AsQueryable()
                    .Select(x => new { x.User.UserName, x.User.NetworkId }).ToList();

            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("adminUserId", adminUserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("security/Authenticate")]
        public JsonResult Authenticate([FromBody] AuthenticationModel model)
        {
            dynamic result = new ExpandoObject();
            string networkId = string.Empty;
            string password = string.Empty;
            var statusCode = HttpStatusCode.OK;
            try
            {
                networkId = _helperSecurity.DecodeFrom64(model.NetworkId);
                password = _helperSecurity.DecodeFrom64(model.Password);

                if (networkId.Contains("@"))
                {
                    var ldapUser = new RepositoryBase<LdapUser>().AsQueryable().FirstOrDefault(x => x.Person.EmailAddresses.Any(e => e.Address == networkId));
                    if (ldapUser != null)
                    {
                        networkId = ldapUser.NetworkId;
                    }
                }

                if (!_helperSecurity.CheckAuthenticate(networkId, password))
                {
                    //statusCode = HttpStatusCode.Unauthorized;
                    throw new Exception("User could not be authenticated");

                }

                //var user = _helperUser.LookupUserByNetworkId(networkId);
                var token = _helperSecurity.Authenticate(model.NetworkId, model.Password);
                result.UserToken = new BiUserTokenModel(token);
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("networkIdEncrypted", model.NetworkId);
                parameters.Add("passwordEncrypted", model.Password);
                result.ErrorMessage = ex.Message;
                statusCode = HttpStatusCode.Unauthorized;
            }

            return JsonResultWithStatusCode(result, statusCode);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetAllLdapUsersStartingWith/{startingWith}")]
        public JsonResult GetAllLdapUsers(string startingWith)
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.Employees = _helperUser.GetAllLdapUsers(startingWith);
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        #endregion

        #region Testing

        [AllowAnonymous]
        [HttpGet]
        [Route("security/SetDeveloperToken/{networkIdEncrypted}/{passwordEncrypted}")]
        public JsonResult SetDeveloperToken(string networkIdEncrypted, string passwordEncrypted)
        {
            var networkId = _helperSecurity.DecodeFrom64(networkIdEncrypted);
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            try
            {
                if (EnvironmentSettings.CurrentEnvironment != global::DAL.Vulcan.Mongo.Base.Context.Environment.Development)
                {
                    throw new Exception("This feature only works in development");
                }

                var token = _helperSecurity.Authenticate(networkIdEncrypted, passwordEncrypted);
                result.UserToken = token;

                DeveloperToken.Token = token;

            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("networkId", networkId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref statusCode);
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetDeveloperToken")]
        public JsonResult GetDeveloperToken()
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            try
            {
                if (EnvironmentSettings.CurrentEnvironment != global::DAL.Vulcan.Mongo.Base.Context.Environment.Development)
                {
                    throw new Exception("This feature only works in development");
                }

                result.UserToken = DeveloperToken.Token;
            }
            catch (Exception ex)
            {
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref statusCode);
            }

            return JsonResultWithStatusCode(result, statusCode);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("security/ClearDeveloperToken")]
        public JsonResult ClearDeveloperToken()
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            try
            {
                DeveloperToken.Token = null;

                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref statusCode);
            }

            return JsonResultWithStatusCode(result, statusCode);
        }
        #endregion

        #region App Permissions

        [AllowAnonymous]
        [HttpGet]
        [Route("security/CreateAppPermission/{permissionName}/{permissionLabel}")]
        public JsonResult CreateAppPermission(string label, string description)
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var biUser = _helperUser.GetBiUser(tokenData.UserId);
                if (!biUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.AppPermissionModel = _helperSecurity.CreateAppPermission(label, description);
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                parameters.Add("label", label);
                parameters.Add("description", description);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/RemoveAppPermission/{permissionLabel}")]
        public JsonResult RemoveAppPermission(string appPermissionId)
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var biUser = _helperUser.GetBiUser(tokenData.UserId);
                if (!biUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                _helperSecurity.RemoveAppPermission(appPermissionId);

            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                parameters.Add("appPermissionId", appPermissionId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetAllAppPermissions")]
        public JsonResult GetAllAppPermissions()
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var biUser = _helperUser.GetBiUser(tokenData.UserId);
                if (!biUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.AppPermissions = _helperSecurity.GetAllAppPermissions();
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        #endregion

        #region Security Checks

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetHasAppPermission/{moduleTypeId}/{permissionLabel}")]
        public JsonResult GetHasAppPermission(string permissionLabel)
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.HasPermission = _helperSecurity.GetHasAppPermission(tokenData.UserId, permissionLabel);
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                parameters.Add("permissionLabel", permissionLabel);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        #endregion

        #region Add Users

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetNewBiUserList")]
        public JsonResult GetNewBiUserList()
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var biUser = _helperUser.GetBiUser(tokenData.UserId);
                if (!biUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.BiUserModel = _helperSecurity.GetNewBiUserModel();
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }



        [AllowAnonymous]
        [HttpGet]
        [Route("security/AddUser/{userId}/{isAdmin}")]
        public JsonResult AddUser(string userId, bool isAdmin)
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var biUser = _helperUser.GetBiUser(tokenData.UserId);
                if (!biUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.BiUserModel = _helperSecurity.AddUser(userId, isAdmin);
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("AdminUserId", tokenData.UserId);
                parameters.Add("UserId", userId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/RemoveUser/{userId}")]
        public JsonResult RemoveUser(string userId)
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                _helperSecurity.RemoveBiUser(userId);
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetBiUserModel/{userId}")]
        public JsonResult GetBiUserModel(string userId)

        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var model = _helperSecurity.GetBiUserModel(tokenData.UserId);
                result.BiUserModel = model;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("security/SaveBiUserModel")]
        public JsonResult SaveBiUserModel([FromBody] BiUserModel model)

        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                CheckForModelErrors();
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                result.BiUserModel = _helperSecurity.SaveBiUserModel(model);
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, resultStatusCode);
        }


        #endregion


    }
}
