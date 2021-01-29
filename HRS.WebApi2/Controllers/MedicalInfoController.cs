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
    public class MedicalInfoController : BaseController
    {
        private readonly IHelperMedicalInfo _helperMedicalInfo;

        public MedicalInfoController(IHelperMedicalInfo helperMedicalInfo)
        {
            _helperMedicalInfo = helperMedicalInfo;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("medicalInfo/GetMedicalInfoGrid")]
        public JsonResult GetMedicalInfoGrid()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                result.MedicalInfoGridData = _helperMedicalInfo.GetMedicalInfoGrid(hrsUser);
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
        [Route("medicalInfo/GetMedicalInfoModel/{employeeId}")]
        public JsonResult GetMedicalInfoModel(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                result.MedicalInfoModel = _helperMedicalInfo.GetMedicalInfo(employeeId,hrsUser.AsHrsUserRef());
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
        [Route("medicalInfo/SaveMedicalInfo")]
        public JsonResult SaveMedicalInfo([FromBody] MedicalInfoModel model)
        { 
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                CheckForModelErrors();
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.MedicalExamModel = _helperMedicalInfo.SaveMedicalInfo(model);
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
        [Route("medicalInfo/GetNewDrugTest")]
        public JsonResult GetNewDrugTest()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.DrugTest = _helperMedicalInfo.GetNewDrugTest();
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
        [Route("medicalInfo/GetNewMedicalLeave")]
        public JsonResult GetNewMedicalLeave()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.MedicalLeave = _helperMedicalInfo.GetNewMedicalLeave();
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
        [Route("medicalInfo/GetNewMedicalExam")]
        public JsonResult GetNewMedicalExam()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.MedicalExam = _helperMedicalInfo.GetNewMedicalExam();
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
        [Route("medicalInfo/GetNewOtherMedicalInfo")]
        public JsonResult GetNewOtherMedicalInfo()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.OtherMedicalInfo = _helperMedicalInfo.GetNewOtherMedicalInfo();
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
        //[Route("medicalInfo/GetMedicalLeavesGrid")]
        //public JsonResult GetMedicalLeaveGrid()
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var tokenData = GetTokenDataFromHeaders();
        //    var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
        //    try
        //    {
        //        ThrowExceptionForBadToken(tokenData.StatusCodeResult);

        //        result.MedicalLeaves = _helperMedicalInfo.GetMedicalLeaves(hrsUser);
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

    }
}
