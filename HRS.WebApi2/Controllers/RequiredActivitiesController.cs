using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class RequiredActivitiesController : BaseController
    {
        private readonly IHelperRequiredActivity _helperRequiredActivity;
        private readonly IHelperTraining _helperTraining;

        public RequiredActivitiesController(IHelperRequiredActivity helperRequiredActivity, IHelperTraining helperTraining)
        {
            _helperRequiredActivity = helperRequiredActivity;
            _helperTraining = helperTraining;
        }

        [AllowAnonymous]
        [HttpGet]
        //[Route("requiredActivities/GetAllRequiredActivitiesDueLessThanSixMonthsFromNow/{status}/{type}")]
        [Route("requiredActivities/GetAllRequiredActivitiesDueLessThanSixMonthsFromNow")]
        //public JsonResult GetAllRequiredActivitiesDueLessThanSixMonthsFromNow(string status, string type)
        public JsonResult GetAllRequiredActivitiesDueLessThanSixMonthsFromNow()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                //result.DashboardItems = _helperRequiredActivity.GetAllRequiredActivitiesDueLessThanSixMonthsFromNow(status, type, hrsUser);
                result.DashboardItems = _helperRequiredActivity.GetAllRequiredActivitiesDueLessThanSixMonthsFromNow(hrsUser);
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
        [Route("training/GetAllRequiredActivitesForActiveEmployees")]
        public JsonResult GetAllRequiredActivitesForActiveEmployees()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                

                result.RequiredActivities = RequiredActivitiesQuery.GetAllForActiveEmployees(hrsUser);
                result.RequiredActivityTypes = Enum.GetNames(typeof(RequiredActivityType)).ToList();
                result.RequiredActivityStatuses = Enum.GetNames(typeof(RequiredActivityStatus)).ToList();
                result.RequiredActivityTypePropertyValues = _helperTraining.GetRequiredActivityTypes(hrsUser.Entity.Id);
                result.RequiredActivityCompleteStatusTypes = _helperTraining.GetRequiredActivityCompleteStatusTypes(hrsUser.Entity.Id);

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
        [Route("training/SaveRequiredActivity")]
        public JsonResult SaveRequiredActivity([FromBody] RequiredActivityModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                var module = hrsUser.HrsSecurity.GetRole().Modules.FirstOrDefault(x => x.ModuleType.Name == "Required Activities");

                if (module == null) throw new Exception("You do not have access to this module");

                if (!module.Modify) throw new Exception("You do not have Modify access to this module");

                result.RequiredActivityModel = _helperRequiredActivity.SaveRequiredActivity(model);

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
        [Route("requiredActivities/ChangeRequiredActivityStatus/{status}/{type}")]
        public JsonResult ChangeRequiredActivityStatus(string requiredActivityId, string status)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.DashboardItems = _helperRequiredActivity.ChangeRequiredActivityStatus(requiredActivityId, status);
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
