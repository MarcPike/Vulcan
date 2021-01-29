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
    public class DashboardController : BaseController
    {
        private readonly IHelperDashboard _helperDashboard;

        public DashboardController(IHelperDashboard helperDashboard)
        {
            _helperDashboard = helperDashboard;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dashboard/GetHrsDashboard")]
        public JsonResult GetHrsDashboard()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                var securityRole = hrsUser.HrsSecurity.GetRole();

                if (securityRole == null) throw new Exception("Unable to view due to security restrictions - No Role");

                var module = securityRole.Modules.SingleOrDefault(x => x.ModuleType.Name == "HR Dashboard");
                
                if (module == null) throw new Exception("Unable to view due to security restrictions - No Module");

                if (!module.View)
                {
                    if (module == null) throw new Exception("Unable to view due to security restrictions - No View permission");
                }

                result.HrsDashboardResults = _helperDashboard.GetHrsDashboard(hrsUser);
                

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
        [Route("dashboard/GetHseDashboard")]
        public JsonResult GetHseDashboard()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                var securityRole = hrsUser.HseSecurity.GetRole();

                if (securityRole == null) throw new Exception("Unable to view due to security restrictions - No Role");

                var module = securityRole.Modules.SingleOrDefault(x => x.ModuleType.Name == "HSE Dashboard");

                if (module == null) throw new Exception("Unable to view due to security restrictions - No Module");

                if (!module.View)
                {
                    if (module == null) throw new Exception("Unable to view due to security restrictions - View permission = false");
                }

                result.HseDashboardResults = _helperDashboard.GetHseDashboard(hrsUser);


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
        [Route("dashboard/GetExecutiveDashboard")]
        public JsonResult GetExecutiveDashboard()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                var securityRole = hrsUser.HrsSecurity.GetRole();

                if (securityRole == null) throw new Exception("Unable to view due to security restrictions - No Role");

                var module = securityRole.Modules.SingleOrDefault(x => x.ModuleType.Name == "Executive Dashboard");

                if (module == null) throw new Exception("Unable to view due to security restrictions - No Module");

                if (!module.View)
                {
                    if (module == null) throw new Exception("Unable to view due to security restrictions - No View permission");
                }

                result.ExecutiveDashboardResults = _helperDashboard.GetExecutiveDashboard();


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
