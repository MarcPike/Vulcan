using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using DAL.Common.Ldap;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class SecurityController : BaseController
    {

        private readonly IHelperSecurity _helperSecurity;
        //private VulcanLogger _logger;

        public SecurityController(IHelperSecurity helperSecurity): base()
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
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                UserAuthentication.RefreshAll();
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("token.UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName:MethodBase.GetCurrentMethod().Name,sendEmail:true, exception:ex, parameters:parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
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
            result.Success = false;
            var adminUserId = _helperSecurity.DecodeFrom64(adminUserIdEncrypted);
            var networkId = _helperSecurity.DecodeFrom64(networkIdEncrypted);
            var resultStatusCode = HttpStatusCode.Unauthorized;
            try
            {
                var userToken = _helperSecurity.Impersonate(adminUserIdEncrypted, networkIdEncrypted);

                result.UserToken = userToken;
                result.Success = true;
                resultStatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("adminUserId", adminUserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetImpersonateList/{adminUserIdEncrypted}")]
        public JsonResult GetImpersonateList(string adminUserIdEncrypted)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var adminUserId = _helperSecurity.DecodeFrom64(adminUserIdEncrypted);
            var admin = _helperUser.GetHrsUser(adminUserId);
            var tokenData = GetTokenDataFromHeaders();
            try
            {

                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                if (!admin.SystemAdmin) throw new Exception("You are not authorized");

                result.UserList = new RepositoryBase<HrsUser>().AsQueryable()
                    .Select(x => new {x.User.UserName, x.User.NetworkId}).ToList();

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("adminUserId", adminUserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/Authenticate/{networkIdEncrypted}/{passwordEncrypted}")]
        public JsonResult Authenticate(string networkIdEncrypted, string passwordEncrypted)
        {
            var networkId = _helperSecurity.DecodeFrom64(networkIdEncrypted);
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = HttpStatusCode.OK;
            try
            {
                var token = _helperSecurity.Authenticate(networkIdEncrypted, passwordEncrypted);

                var hrsUser = _helperUser.GetHrsUser(token.User.Id);
                if (hrsUser == null) throw new Exception("User not an Hrs User.");

                result.UserToken = token;
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("networkId", networkId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetAllLdapUsersStartingWith/{startingWith}")]
        public JsonResult GetAllLdapUsers(string startingWith)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.Employees = _helperUser.GetAllLdapUsers(startingWith);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
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
            result.Success = false;
            var statusCode = HttpStatusCode.OK;
            try
            {
                if (EnvironmentSettings.CurrentEnvironment != Environment.Development)
                {
                    throw new Exception("This feature only works in development");
                }

                var token = _helperSecurity.Authenticate(networkIdEncrypted, passwordEncrypted);
                result.UserToken = token;

                DeveloperToken.Token = token;

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("networkId", networkId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetDeveloperToken")]
        public JsonResult GetDeveloperToken()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = HttpStatusCode.OK;
            try
            {
                if (EnvironmentSettings.CurrentEnvironment != Environment.Development)
                {
                    throw new Exception("This feature only works in development");
                }

                result.UserToken = DeveloperToken.Token;
                result.Success = true;
            }
            catch (Exception ex)
            {
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("security/ClearDeveloperToken")]
        public JsonResult ClearDeveloperToken()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = HttpStatusCode.OK;
            try
            {
                DeveloperToken.Token = null;

                result.Success = true;
            }
            catch (Exception ex)
            {
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                result.Success = false;
                result.ErrorMessage = ex.Message;
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
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.AppPermissionModel = _helperSecurity.CreateAppPermission(label, description);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("label", label);
                //parameters.Add("description", description);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/RemoveAppPermission/{permissionLabel}")]
        public JsonResult RemoveAppPermission(string appPermissionId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                _helperSecurity.RemoveAppPermission(appPermissionId);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("appPermissionId", appPermissionId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetAllAppPermissions")]
        public JsonResult GetAllAppPermissions()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.AppPermissions = _helperSecurity.GetAllAppPermissions();

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetUserHrsAppPermissionForModule/{userId}/{moduleTypeId}")]
        public JsonResult GetUserHrsAppPermissionForModule(string userId, string moduleTypeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.UserAppPermissionsModel = _helperSecurity.GetUserHrsAppPermissionForModule(userId, moduleTypeId);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", userId);
                //parameters.Add("ModuleTypeId", moduleTypeId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetUserHseAppPermissionForModule/{userId}/{moduleTypeId}")]
        public JsonResult GetUserHseAppPermissionForModule(string userId, string moduleTypeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.UserAppPermissionsModel = _helperSecurity.GetUserHseAppPermissionForModule(userId, moduleTypeId);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", userId);
                //parameters.Add("ModuleTypeId", moduleTypeId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("security/SaveUserAppPermissionsForModule")]
        public JsonResult SaveUserAppPermissionsForModule([FromBody] UserAppPermissionsModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.UserAppPermissionsModel = _helperSecurity.SaveUserAppPermissionsForModule(model);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        #endregion

        #region Roles

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("security/GetCreateRoleModel")]
        //public JsonResult GetCreateRoleModel()
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var tokenData = GetTokenDataFromHeaders();
        //    var resultStatusCode = tokenData.StatusCodeResult;
        //    try
        //    {
        //        ThrowExceptionForBadToken(tokenData.StatusCodeResult);
        //        var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
        //        if (!hrsUser.SystemAdmin)
        //        {
        //            resultStatusCode = HttpStatusCode.Forbidden;
        //            throw new Exception("User calling this does not have SysAdmin.Modify privileges");
        //        }

        //        result.CreateRoleModel = _helperSecurity.GetCreateRoleModel();
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var parameters = GetParametersDictionary();
        //        parameters.Add("UserId", tokenData.UserId);
        //        _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
        //        result.Success = false;
        //        result.ErrorMessage = ex.Message;
        //    }
        //    return JsonResultWithStatusCode(result, resultStatusCode);

        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("security/CreateNewRole")]
        //public JsonResult CreateNewRole([FromBody] CreateRoleModel model)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var tokenData = GetTokenDataFromHeaders();
        //    var resultStatusCode = tokenData.StatusCodeResult;
        //    try
        //    {
        //        ThrowExceptionForBadToken(tokenData.StatusCodeResult);
        //        var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
        //        if (!hrsUser.SystemAdmin)
        //        {
        //            resultStatusCode = HttpStatusCode.Forbidden;
        //            throw new Exception("User calling this does not have SysAdmin.Modify privileges");
        //        }

        //        result.SecurityRoleModel = _helperSecurity.CreateNewRole(model);
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var parameters = GetParametersDictionary();
        //        parameters.Add("UserId", tokenData.UserId);
        //        _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
        //        result.Success = false;
        //        result.ErrorMessage = ex.Message;
        //    }
        //    return JsonResultWithStatusCode(result, resultStatusCode);

        //}

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetRoleTypeForName/{roleName}")]
        public JsonResult GetRoleTypeForName(string roleName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.RoleType = _helperSecurity.GetRoleTypeForName(roleName);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("hrsRoleTypeId", roleName);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/CreateNewRole/{roleName}/{isHrs}/{isHse}")]
        public JsonResult CreateNewRole(string roleName, bool isHrs, bool isHse)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                var existing = SecurityRoleType.Helper.Find(x => x.Name == roleName).FirstOrDefault();
                if (existing != null)
                {
                    existing.IsHrsRole = isHrs;
                    existing.IsHseRole = isHse;
                    SecurityRoleType.Helper.Upsert(existing);
                    var role = SecurityRole.Helper.
                        Find(x => x.RoleType.Id == existing.Id.ToString()).First();
                    
                    role.RoleType = existing.AsSecurityRoleTypeRef();
                    SecurityRole.Helper.Upsert(role);
                    
                    result.SecurityRoleModel = new SecurityRoleModel(role);
                    result.SecurityRoleTypeRef = role.RoleType;
                    result.Success = true;
                }
                else
                {
                    var roleTypeRef = _helperSecurity.DefineNewRoleType(roleName, isHrs, isHse);
                    result.SecurityRoleModel = _helperSecurity.CreateNewRole(roleTypeRef);
                    result.SecurityRoleTypeRef = roleTypeRef;
                    result.Success = true;
                }

            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("hrsRoleTypeId", roleName);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetSecurityRoleModelForRoleType/{securityRoleTypeId}")]
        public JsonResult GetSecurityRoleModelForRoleType(string securityRoleTypeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.SecurityRoleModel = _helperSecurity.GetSecurityRoleModelForRoleType(securityRoleTypeId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetSecurityRoleModelForName/{securityRoleName}")]
        public JsonResult GetSecurityRoleModelForName(string securityRoleName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.SecurityRoleModel = _helperSecurity.GetSecurityRoleModelForName(securityRoleName);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }


        [AllowAnonymous]
        [HttpPost]
        [Route("security/SaveRole")]
        public JsonResult SaveRole([FromBody] SecurityRoleModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                var roleModel = _helperSecurity.SaveRole(model);
                result.SecurityRoleModel = roleModel;
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetAllRoles")]
        public JsonResult GetAllRoles()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.SecurityRoles = _helperSecurity.GetAllRoles();
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetAllRoleDefinitions")]
        public JsonResult GetAllRoleDefinitions()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                var queryHelper = new MongoRawQueryHelper<SecurityRole>();
                var filter = queryHelper.FilterBuilder.Empty;
                var project =
                    queryHelper.ProjectionBuilder.Expression(x => new {x.Id, x.RoleType, x.DirectReportsOnly});

                result.SecurityRoleDefinitions = queryHelper.FindWithProjection(filter, project).ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetAllRoleTypes")]
        public JsonResult GetAllRoleTypes()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.SecurityRoleTypes = _helperSecurity.GetAllRoleTypes();
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetSecurityRoleModel/{roleId}")]
        public JsonResult GetSecurityRoleModel(string roleId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.SecurityRoleModel = _helperSecurity.GetSecurityRoleModel(roleId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/RemoveRole/{roleTypeId}")]
        public JsonResult RemoveRole(string roleTypeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                _helperSecurity.RemoveRole(roleTypeId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        #endregion

        #region Modules


        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetHrsModuleTypeForName/{moduleName}")]
        public JsonResult GetHrsModuleTypeForName(string moduleName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.ModuleType = _helperSecurity.GetHrsModuleTypeForName(moduleName);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("moduleName", moduleName);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetHseModuleTypeForName/{moduleName}")]
        public JsonResult GetHseModuleTypeForName(string moduleName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.ModuleType = _helperSecurity.GetHseModuleTypeForName(moduleName);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("moduleName", moduleName);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/DefineNewModuleType/{moduleName}/{isHrs}/{isHse}")]
        public JsonResult DefineNewModuleType(string moduleName, bool isHrs, bool isHse)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                var systemModuleTypeRef = _helperSecurity.DefineNewModuleType(moduleName, isHrs, isHse);
                var systemModuleModel = new SystemModuleModel(systemModuleTypeRef);
                result.SystemModuleRef = systemModuleTypeRef;
                result.SystemModuleModel = systemModuleModel;

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("moduleName", moduleName);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetSystemModuleModel/{moduleTypeId}")]
        public JsonResult GetSystemModuleModel(string moduleTypeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.SystemModuleModel = _helperSecurity.GetSystemModuleModel(moduleTypeId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetAllModules")]
        public JsonResult GetAllModules()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.ModuleModelsList = _helperSecurity.GetAllModules();
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetAllModuleTypes")]
        public JsonResult GetAllModuleTypes()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.ModuleTypes = _helperSecurity.GetAllModuleTypes();

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/RemoveModuleType/{moduleTypeId}")]
        public JsonResult RemoveModuleType(string moduleTypeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                _helperSecurity.RemoveModuleType(moduleTypeId);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }



        #endregion

        #region Security Checks

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetHasHrsPermission/{moduleTypeId}/{permissionLabel}")]
        public JsonResult GetHasHrsPermission(string moduleTypeId, string permissionLabel)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.HasPermission = _helperSecurity.GetHasHrsPermission(tokenData.UserId, moduleTypeId, permissionLabel);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("moduleTypeId", moduleTypeId);
                //parameters.Add("permissionLabel", permissionLabel);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);

                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetHasHsePermission/{moduleTypeId}/{permissionLabel}")]
        public JsonResult GetHasHsePermission(string moduleTypeId, string permissionLabel)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.HasPermission = _helperSecurity.GetHasHsePermission(tokenData.UserId, moduleTypeId, permissionLabel);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("moduleTypeId", moduleTypeId);
                //parameters.Add("permissionLabel", permissionLabel);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);

                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/UsersInRole/{roleTypeId}")]
        public JsonResult UsersInRole(string roleTypeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.UserList = _helperSecurity.UsersInRole(roleTypeId);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("roleTypeId", roleTypeId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);

                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        #endregion

        #region Add Users

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetNewHrsUserList")]
        public JsonResult GetNewHrsUserList()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.HrsUserModel = _helperSecurity.GetHrsNewUserList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }



        [AllowAnonymous]
        [HttpGet]
        [Route("security/AddUser/{employeeId}/{userId}/{hrsRoleId}/{hseRoleId}")]
        public JsonResult AddUser(string employeeId, string userId, string hrsRoleId, string hseRoleId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    resultStatusCode = HttpStatusCode.Forbidden;
                    throw new Exception("User calling this does not have SysAdmin.Modify privileges");
                }

                result.HrsUserModel = _helperSecurity.AddUser(employeeId, userId, hrsRoleId, hseRoleId, hrsUser.Entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("employeeId", employeeId);
                //parameters.Add("userId", userId);
                //parameters.Add("hrsRoleId", hrsRoleId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/RemoveUser/{userId}")]
        public JsonResult RemoveUser(string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                _helperSecurity.RemoveHrsUser(userId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/GetHrsUserModel/{userId}")]
        public JsonResult GetHrsUserModel(string userId)

        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var model = _helperSecurity.GetHrsUserModel(userId);
                //if (model.HrsSecurity.Role == null) model.HrsSecurity = null;
                //if (model.HseSecurity.Role == null) model.HseSecurity = null;
                result.HrsUserModel = model;
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("security/SaveHrsUserModel")]
        public JsonResult SaveHrsUserModel([FromBody] HrsUserModel model)

        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                CheckForModelErrors();
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                result.HrsUserModel = _helperSecurity.SaveHrsUserModel(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);
        }


        #endregion


    }
}