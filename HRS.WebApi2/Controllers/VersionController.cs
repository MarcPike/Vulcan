using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DAL.HRS.Mongo.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRS.WebApi2.Controllers
{
    public class VersionController : BaseController
    {
        private readonly IHelperVersionHistory _helperVersionHistory;

        public VersionController(IHelperVersionHistory helperVersionHistory)
        {
            _helperVersionHistory = helperVersionHistory;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("version/GetFullVersionHistory")]
        public JsonResult GetBenefitsGrid()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.VersionHistory = _helperVersionHistory.GetAllHistory();
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
        [Route("version/GetCurrentVersion")]
        public JsonResult GetCurrentVersion()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.CurrentVersion = _helperVersionHistory.GetCurrentVersion();
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
