using System;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class ExternalLoginController : BaseController
    {
        private readonly IHelperExternalLogin _helperExternalLogin;

        public ExternalLoginController(IHelperUser helperUser, IHelperExternalLogin helperExternalLogin) : base(helperUser)
        {
            _helperExternalLogin = helperExternalLogin;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalLogin/GetNewExternalLogin/{adminUserId}")]
        public async Task<JsonResult> GetNewExternalLogin(string adminUserId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", adminUserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = _helperUser.GetCrmUser("vulcancrm", adminUserId);
                    if (!crmUser.IsAdmin) throw new Exception("You are not authorized");

                    result.ExternalLoginModel = _helperExternalLogin.GetNewExternalLogin(adminUserId);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalLogin/Login/{userName}/{password}")]
        public async Task<JsonResult> Login(string userName, string password)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = HttpStatusCode.OK;
            try
            {
                await Task.Run(() =>
                {
                    result.ExternalLoginModel = _helperExternalLogin.Login(userName, password);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
                statusCode = HttpStatusCode.Unauthorized;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalLogin/GetAllExternalLogins/{application}/{userId}")]
        public async Task<JsonResult> GetAllExternalLogins(string adminUserId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", adminUserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = _helperUser.GetCrmUser("vulcancrm", adminUserId);
                    if (!crmUser.IsAdmin) throw new Exception("You are not authorized");

                    result.ExternalLogins = _helperExternalLogin.GetAllExternalLogins(adminUserId);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ExternalLogin/SaveExternalLogin")]
        public async Task<JsonResult> SaveExternalLogin([FromBody] ExternalLoginModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", model.AdminUserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = _helperUser.GetCrmUser("vulcancrm", model.AdminUserId);
                    if (!crmUser.IsAdmin) throw new Exception("You are not authorized");

                    result.ExternalLogins = _helperExternalLogin.GetAllExternalLogins(model.AdminUserId);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

    }
}
