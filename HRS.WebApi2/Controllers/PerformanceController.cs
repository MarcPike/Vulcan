using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class PerformanceController: BaseController
    {
        private readonly IHelperEmployee _helperEmployee;
        private readonly IHelperPerformance _helperPerformance;

        public PerformanceController(IHelperEmployee helperEmployee, IHelperPerformance helperPerformance)
        {
            _helperEmployee = helperEmployee;
            _helperPerformance = helperPerformance;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("performance/GetPerformanceGrid")]
        public JsonResult GetPerformanceGrid()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.PerformanceGridModel = _helperPerformance.GetPerformanceGrid(tokenData.UserId);
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
        [Route("performance/GetPerformanceModelForEmployee/{employeeId}")]
        public JsonResult GetPerformanceModelForEmployee(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);


                var models = _helperPerformance.GetPerformanceModelsForEmployee(employeeId);
                result.Current = models.Current;
                result.History = models.History;
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
        [HttpPost]
        [Route("performance/SavePerformance")]
        public JsonResult SavePerformance([FromBody] PerformanceModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);


                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId).AsHrsUserRef();
                model.ModifiedBy = hrsUser;

                var saveResult = _helperPerformance.SavePerformance(model);

                result.Current = saveResult.Current;
                result.History = saveResult.History;
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
        [HttpPost]
        [Route("performance/ModifyPerformanceHistory")]
        public JsonResult ModifyPerformanceHistory([FromBody] PerformanceModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                result.PerformanceHistory = _helperPerformance.ModifyPerformanceHistory(model);
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
