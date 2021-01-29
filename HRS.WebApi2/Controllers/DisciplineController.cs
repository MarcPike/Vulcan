using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class DisciplineController : BaseController
    {
        private readonly IHelperDiscipline _helperDiscipline;
        

        public DisciplineController(IHelperDiscipline helperDiscipline)
        {
            _helperDiscipline = helperDiscipline;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("discipline/GetDisciplineGrid")]
        public JsonResult GetDisciplineGrid()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.DisciplineGridModel = _helperDiscipline.GetDisciplineGrid(tokenData.UserId);
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
        [Route("discipline/GetDisciplineModelListForEmployee/{employeeId}")]
        public JsonResult GetDisciplineModelListForEmployee(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var models = _helperDiscipline.GetDisciplineModelsListForEmployee(employeeId);
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
        [Route("discipline/SaveDiscipline")]
        public JsonResult SaveDiscipline([FromBody] DisciplineModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId).AsHrsUserRef();
                model.ModifiedBy = hrsUser;
                var saveResult = _helperDiscipline.SaveDiscipline(model);

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
        [Route("discipline/ModifyDisciplineHistory")]
        public JsonResult ModifyDisciplineHistory([FromBody] DisciplineModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.DisciplineHistory = _helperDiscipline.ModifyDisciplineHistory(model);
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