using DAL.HRS.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Dynamic;
using System.Net;
using System.Reflection;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Repository;
using Swashbuckle.AspNetCore.Swagger;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Consumes("application/json")]

    public class UserController : BaseController
    {
        private readonly IHelperSecurity _helperSecurity;


        public UserController(IHelperSecurity helperSecurity)
        {
            _helperSecurity = helperSecurity;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/GetAllHrsUsers")]
        public JsonResult GetAllHrsUsers()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                result.HrsUsers = _helperUser.GetAllHrsUsers();
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
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("user/GetAllLdapUsers/{lastNameContains}")]
        public JsonResult GetAllLdapUsers(string lastNameContains)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                result.Employees = _helperUser.GetAllLdapUsers(lastNameContains);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("lastNameContains", lastNameContains);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/GetMyUserModel")]
        public JsonResult GetMyUserModel()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                result.HrsUserModel = _helperUser.GetUserModel(tokenData.UserId);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //if (tokenData.UserId != null)
                //    parameters.Add("UserId", tokenData.UserId ?? String.Empty);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/GetUserModel/{userId}")]
        public JsonResult GetUserModel(string userId)
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

                result.HrsUserModel = _helperUser.GetUserModel(userId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("UserId", userId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("user/SaveUser")]
        //public JsonResult SaveUser([FromBody] HrsUserModel model)
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

        //        result.HrsUserModel = _helperUser.SaveUser(model);
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
        [Route("user/GetNewUserList")]
        public JsonResult GetNewUserList()
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

                result.NewUserList = _helperUser.GetNewUserList();
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

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("user/AddHrsUser/{userId}/{roleName}")]
        //public JsonResult AddHrsUser(string userId, string roleName)
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

        //        result.HrsUserModel = _helperUser.AddHrsUser(userId, roleName);
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var parameters = GetParametersDictionary();
        //        parameters.Add("UserId", tokenData.UserId);
        //        parameters.Add("userId", userId);
        //        parameters.Add("roleName", roleName);
        //        _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
        //        result.Success = false;
        //        result.ErrorMessage = ex.Message;
        //    }
        //    return JsonResultWithStatusCode(result, resultStatusCode);

        //}

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("user/AddHseUser/{userId}/{roleName}")]
        //public JsonResult AddHseUser(string userId, string roleName)
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

        //        result.HrsUserModel = _helperUser.AddHseUser(userId, roleName);
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var parameters = GetParametersDictionary();
        //        parameters.Add("UserId", tokenData.UserId);
        //        parameters.Add("userId", userId);
        //        parameters.Add("roleName", roleName);
        //        _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
        //        result.Success = false;
        //        result.ErrorMessage = ex.Message;
        //    }
        //    return JsonResultWithStatusCode(result, resultStatusCode);

        //}

        [AllowAnonymous]
        [HttpGet]
        [Route("user/ChangeHrsUserRole/{userId}/{roleId}")]
        public JsonResult ChangeHrsUserRole(string userId, string roleId)
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

                result.HrsUserModel = _helperUser.ChangeHrsUserRole(userId, roleId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("userId", userId);
                //parameters.Add("roleId", roleId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("user/ChangeHseUserRole/{userId}/{roleId}")]
        public JsonResult ChangeHseUserRole(string userId, string roleId)
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

                result.HrsUserModel = _helperUser.ChangeHseUserRole(userId, roleId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("userId", userId);
                //parameters.Add("roleName", roleId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/CopyHrsSecurityRole/{fromUserId}/{toUserId}")]
        public JsonResult CopyHrsSecurityRole(string fromUserId, string toUserId)
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

                result.HrsUserModel = _helperUser.CopyHrsSecurityRole(fromUserId, toUserId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("fromUserId", fromUserId);
                //parameters.Add("toUserId", toUserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/CopyHseSecurityRole/{fromUserId}/{toUserId}")]
        public JsonResult CopyHseSecurityRole(string fromUserId, string toUserId)
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

                result.HrsUserModel = _helperUser.CopyHseSecurityRole(fromUserId, toUserId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //parameters.Add("fromUserId", fromUserId);
                //parameters.Add("toUserId", toUserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

    }
}