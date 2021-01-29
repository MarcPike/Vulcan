using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class CompensationController: BaseController
    {
        private readonly IHelperCompensation _helperCompensation;

        public CompensationController(IHelperCompensation helperCompensation)
        {
            _helperCompensation = helperCompensation;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("compensation/GetCompensationGrid")]
        public JsonResult GetCompensationGrid()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.CompensationGrid = _helperCompensation.GetCompensationGrid(tokenData.UserId);
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
        [Route("compensation/IsCompensationAllowedForEmployee/{employeeId}")]
        public JsonResult IsCompensationAllowedForEmployee(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            result.Allowed = true;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                var emp = Employee.Helper.FindById(employeeId);

                var hrsUserPayrollRegions = hrsUser.HrsSecurity?.PayrollRegionsForCompensation ?? new List<PayrollRegionRef>();
                var empPayrollRegion = emp.PayrollRegion;
                if (hrsUserPayrollRegions.All(x => x.Id != empPayrollRegion.Id))
                {
                    result.Allowed = false;
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                parameters.Add("employeeId", employeeId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: false, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("compensation/GetCompensationForEmployee/{employeeId}")]
        public JsonResult GetCompensationForEmployee(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                var emp = Employee.Helper.FindById(employeeId);

                var hrsUserPayrollRegions = hrsUser.HrsSecurity?.PayrollRegionsForCompensation ?? new List<PayrollRegionRef>();
                var empPayrollRegion = emp.PayrollRegion;
                if (hrsUserPayrollRegions.All(x => x.Id != empPayrollRegion.Id))
                {
                    throw new Exception(
                        $"User does not have access to this Employees payroll region and cannot view this Compensation");
                }

                result.CompensationModel = _helperCompensation.GetCompensationForEmployee(employeeId);
               

                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                parameters.Add("employeeId", employeeId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: false, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("compensation/SaveCompensation")]
        public JsonResult SaveCompensation([FromBody] CompensationModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                CheckForModelErrors();
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId).AsHrsUserRef();
                model.ModifiedBy = hrsUser;

                result.CompensationModel = _helperCompensation.SaveCompensation(model);
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
        [Route("compensation/RemoveCompensationHistory/{employeeId}/{historyId}")]
        public JsonResult RemoveCompensationHistory(string employeeId, string historyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                CheckForModelErrors();
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.CompensationModel = _helperCompensation.RemoveCompensationHistory(employeeId, historyId);
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
        [Route("compensation/RemoveBonusHistory/{employeeId}/{historyId}")]
        public JsonResult RemoveBonusHistory(string employeeId, string historyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                CheckForModelErrors();
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.CompensationModel = _helperCompensation.RemoveBonusHistory(employeeId, historyId);
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
        [Route("compensation/RemovePayGradeHistory/{employeeId}/{historyId}")]
        public JsonResult RemovePayGradeHistory(string employeeId, string historyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                CheckForModelErrors();
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.CompensationModel = _helperCompensation.RemovePayGradeHistory(employeeId, historyId);
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
