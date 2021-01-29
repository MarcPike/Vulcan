using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DAL.HRS.Mongo.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class IntegrationsController : BaseController
    {
        private readonly IHelperIntegrations _helperIntegrations;

        public IntegrationsController(IHelperIntegrations helperIntegrations)
        {
            _helperIntegrations = helperIntegrations;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("integrations/KronosExport/{entityName}")]
        public JsonResult KronosExport(string entityName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    throw new Exception("Only SystemAdmin can perform Kronos Exports");
                }

                result.KronosExportData = _helperIntegrations.KronosExport(entityName);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("integrations/HalogenExport/{entityName}")]
        public JsonResult HalogenExport(string entityName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                if (!hrsUser.SystemAdmin)
                {
                    throw new Exception("Only SystemAdmin can perform Halogen Exports");
                }

                result.HalogenExportData = _helperIntegrations.HalogenExport(entityName);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }


    }
}
