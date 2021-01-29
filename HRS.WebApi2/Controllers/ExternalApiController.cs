using DAL.HRS.Mongo.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Dynamic;
using System.Net;
using System.Reflection;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class ExternalApiController : BaseController
    {
        private readonly IHelperExternalApi _helperExternalApi;

        public ExternalApiController(IHelperExternalApi helperExternalApi)
        {
            _helperExternalApi = helperExternalApi;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalApi/GetHrsSecurityForUser/{activeDirectoryId}")]
        public JsonResult GetHrsSecurity(string activeDirectoryId)
        {
            dynamic result = new ExpandoObject();
            try
            {
                result = _helperExternalApi.GetHrsSecurityForUser(activeDirectoryId);
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                return JsonResultWithStatusCode(ex.Message, HttpStatusCode.BadRequest);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalApi/GetAllLocations")]
        public JsonResult GetAllLocations()
        {
            dynamic result = new ExpandoObject();
            try
            {
                result = _helperExternalApi.GetAllLocations();
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                return JsonResultWithStatusCode(ex.Message, HttpStatusCode.BadRequest);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalApi/GetHseSecurityForUser/{activeDirectoryId}")]
        public JsonResult GetHseSecurity(string activeDirectoryId)
        {
            dynamic result = new ExpandoObject();
            try
            {
                result = _helperExternalApi.GetHseSecurityForUser(activeDirectoryId);
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                return JsonResultWithStatusCode(ex.Message, HttpStatusCode.BadRequest);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalApi/GetEmployeeList")]
        public JsonResult GetEmployeeList()
        {
            dynamic result = new ExpandoObject();
            try
            {

                result = _helperExternalApi.GetEmployeeList();
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                return JsonResultWithStatusCode(ex.Message, HttpStatusCode.BadRequest);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalApi/GetEmployeeDetailsForQng/{dateOf}/{activeDirectoryId}")]
        public JsonResult GetEmployeeDetailsForQng(string dateOf, string activeDirectoryId)
        {
            dynamic result = new ExpandoObject();
            try
            {
                var forDate = DateTime.Parse(dateOf);
                result = _helperExternalApi.GetEmployeeDetailsForQng(forDate, activeDirectoryId);
            }
            catch (Exception ex)
            {

                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                return JsonResultWithStatusCode(ex.Message, HttpStatusCode.BadRequest);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalApi/GetCompensationForQng/{dateOf}/{activeDirectoryId}/{isActive}")]
        public JsonResult GetCompensationForQng(string dateOf, string activeDirectoryId, bool isActive)
        {
            dynamic result = new ExpandoObject();
            try
            {
                var forDate = DateTime.Parse(dateOf);
                result = _helperExternalApi.GetCompensationForQng(forDate, activeDirectoryId);
            }
            catch (Exception ex)
            {

                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                return JsonResultWithStatusCode(ex.Message, HttpStatusCode.BadRequest);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalApi/GetEmployeeIncidentsByVarDataField/{varDataField}/{minDateAsString}/{maxDateAsString}/{activeDirectoryId}")]
        public JsonResult GetEmployeeIncidentsByVarDataField(string varDataField, string minDateAsString, string maxDateAsString, string activeDirectoryId)
        {
            dynamic result = new ExpandoObject();
            try
            {
                var minDate = DateTime.Parse(minDateAsString);
                var maxDate = DateTime.Parse(maxDateAsString);
                result = _helperExternalApi.GetEmployeeIncidentsByVarDataField(varDataField, minDate, maxDate, activeDirectoryId);
            }
            catch (Exception ex)
            {

                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                return JsonResultWithStatusCode(ex.Message, HttpStatusCode.BadRequest);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalApi/GetEmployeeIncidents/{minDateAsString}/{maxDateAsString}/{activeDirectoryId}")]
        public JsonResult GetEmployeeIncidents(string minDateAsString, string maxDateAsString, string activeDirectoryId)
        {
            dynamic result = new ExpandoObject();
            try
            {
                var minDate = DateTime.Parse(minDateAsString);
                var maxDate = DateTime.Parse(maxDateAsString);
                result = _helperExternalApi.GetEmployeeIncidents(minDate, maxDate, activeDirectoryId);
            }
            catch (Exception ex)
            {

                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                return JsonResultWithStatusCode(ex.Message, HttpStatusCode.BadRequest);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalApi/GetTrainingInfo/{minDateAsString}/{maxDateAsString}/{activeDirectoryId}")]
        public JsonResult GetTrainingInfo(string minDateAsString, string maxDateAsString, string activeDirectoryId)
        {
            dynamic result = new ExpandoObject();
            try
            {
                var minDate = DateTime.Parse(minDateAsString);
                var maxDate = DateTime.Parse(maxDateAsString);
                result = _helperExternalApi.GetTrainingInfo(minDate, maxDate, activeDirectoryId);
            }
            catch (Exception ex)
            {

                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                return JsonResultWithStatusCode(ex.Message, HttpStatusCode.BadRequest);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

    }
}
