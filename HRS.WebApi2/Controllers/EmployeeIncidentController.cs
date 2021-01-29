using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class EmployeeIncidentController : BaseController
    {
        private readonly IHelperEmployeeIncidents _helperEmployeeIncidents;

        public EmployeeIncidentController(IHelperEmployeeIncidents helperEmployeeIncidents)
        {
            _helperEmployeeIncidents = helperEmployeeIncidents;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employeeIncidents/GetEmployeeIncidentGridRows")]
        public JsonResult GetEmployeeIncidentGridRows()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.EmployeeIncidentGridRows = _helperEmployeeIncidents.GetEmployeeIncidentGridRows();
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
        [Route("employeeIncidents/GetAllEmployeeIncidents")]
        public JsonResult GetAllEmployeeIncidents()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.EmployeeIncidents = _helperEmployeeIncidents.GetAllEmployeeIncidents();
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

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("employeeIncidents/GetDrugTestModel/{employeeIncidentId}/{drugTestId}")]
        //public JsonResult GetDrugTestModel(string employeeIncidentId, string drugTestId)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var tokenData = GetTokenDataFromHeaders();
        //    try
        //    {
        //        ThrowExceptionForBadToken(tokenData.StatusCodeResult);



        //        result.DrugTestModel = _helperEmployeeIncidents.GetDrugTestModel(id);
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var parameters = GetParametersDictionary();
        //        parameters.Add("UserId", tokenData.UserId);
        //        _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
        //        result.ErrorMessage = ex.Message;
        //        result.Success = false;
        //    }
        //    return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        //}

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("employeeIncidents/GetNewDrugTestModel")]
        //public JsonResult GetNewDrugTestModel()
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var tokenData = GetTokenDataFromHeaders();
        //    try
        //    {
        //        ThrowExceptionForBadToken(tokenData.StatusCodeResult);

        //        result.DrugTestModel = _helperEmployeeIncidents.GetNewDrugTestModel();
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var parameters = GetParametersDictionary();
        //        parameters.Add("UserId", tokenData.UserId);
        //        _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
        //        result.ErrorMessage = ex.Message;
        //        result.Success = false;
        //    }
        //    return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        //}

        [AllowAnonymous]
        [HttpGet]
        [Route("employeeIncidents/GetNewEmployeeIncidentModel")]
        public JsonResult GetNewEmployeeIncidentModel()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var model = _helperEmployeeIncidents.GetNewEmployeeIncidentModel();
                model.HrsUser = _helperUser.GetHrsUser(tokenData.UserId).AsHrsUserRef();

                result.EmployeeIncidentModel = model;
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
        [Route("employeeIncidents/GetNewEmployeeIncidentModelUnsecured")]
        public JsonResult GetNewEmployeeIncidentModelUnSecured()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            //var tokenData = GetTokenDataFromHeaders();
            try
            {
                //ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var model = _helperEmployeeIncidents.GetNewEmployeeIncidentModel();
                //model.HrsUser = _helperUser.GetHrsUser(tokenData.UserId).AsHrsUserRef();

                result.EmployeeIncidentModel = model;
                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result,  HttpStatusCode.OK);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("employeeIncidents/SaveEmployeeIncidentUnsecured")]
        public JsonResult SaveEmployeeIncidentUnsecured([FromBody] EmployeeIncidentModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            //var tokenData = GetTokenDataFromHeaders();
            try
            {
                //ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                CheckForModelErrors();

                result.EmployeeIncidentModel = _helperEmployeeIncidents.SaveEmployeeIncident(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, HttpStatusCode.OK);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("employeeIncidents/GetEmployeeIncidentModel/{incidentId}")]
        public JsonResult GetEmployeeIncidentModel(string incidentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                CheckForModelErrors();

                var model = _helperEmployeeIncidents.GetEmployeeIncidentModel(incidentId);
                model.HrsUser = _helperUser.GetHrsUser(tokenData.UserId).AsHrsUserRef();
                result.EmployeeIncidentModel = model;
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
        [Route("employeeIncidents/RemoveEmployeeIncident/{id}")]
        public JsonResult RemoveEmployeeIncident(string id)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                _helperEmployeeIncidents.RemoveEmployeeIncident(id);
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
        [Route("employeeIncidents/SaveEmployeeIncident")]
        public JsonResult SaveEmployeeIncident([FromBody] EmployeeIncidentModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                CheckForModelErrors();

                result.EmployeeIncidentModel = _helperEmployeeIncidents.SaveEmployeeIncident(model);
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
