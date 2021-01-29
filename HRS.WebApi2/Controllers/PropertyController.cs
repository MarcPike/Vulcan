using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Reflection;
using DAL.Common.Helper;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class PropertyController : BaseController
    {
        private readonly IHelperProperties _helperProperties;
        private readonly IHelperCommon _helperCommon;

        public PropertyController(IHelperProperties helperProperties, IHelperCommon helperCommon)
        {
            _helperProperties = helperProperties;
            _helperCommon = helperCommon;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("properties/AddProperty/{propertyType}/{description}")]
        public JsonResult AddProperty(string propertyType, string description)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                result.PropertyModel = _helperProperties.AddProperty(propertyType, description, hrsUser.Entity.Id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("token.UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("properties/GetPropertiesUnsecured")]
        public JsonResult GetPropertiesUnsecured()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            //var tokenData = GetTokenDataFromHeaders();
            try
            {
                //ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                //var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                result.Properties = _helperProperties.GetProperties(Entity.GetRefByName("Howco").Id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("token.UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("properties/GetAllProperties")]
        public JsonResult GetAllProperties()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.Properties = _helperProperties.GetAllProperties();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("token.UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("properties/GetProperties")]
        public JsonResult GetProperties()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                result.Properties = _helperProperties.GetProperties(hrsUser.Entity.Id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("token.UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("properties/GetPropertyValuesForProperty/{propertyType}")]
        public JsonResult GetPropertyValuesForProperty(string propertyType)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                result.PropertyModels = _helperProperties.GetPropertyValuesForProperty(propertyType, hrsUser.Entity.Id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("token.UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("properties/GetOnlyActivePropertyValuesForProperty/{propertyType}")]
        public JsonResult GetOnlyActivePropertyValuesForProperty(string propertyType)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                result.PropertyModels = _helperProperties.GetOnlyActivePropertyValuesForProperty(propertyType, hrsUser.Entity.Id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("token.UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("properties/GetProperty/{propertyType}")]
        public JsonResult GetProperty(string propertyType)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                result.PropertyModel = _helperProperties.GetProperty(propertyType, hrsUser.Entity.Id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("token.UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("properties/GetNewPropertyValueModel/{propertyType}")]
        public JsonResult GetNewPropertyValueModel(string propertyType)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                var newPropValue = new PropertyValue()
                {
                    Type = propertyType,
                    Code = string.Empty,
                    Description = string.Empty,
                    Entity = hrsUser.Entity,
                    Locations = new List<LocationRef>(),
                };

                result.PropertyValueModel = new PropertyValueModel(newPropValue);
                result.LocationsForCompany = Location.GetReferencesForEntityId(hrsUser.Entity.Id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("token.UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }



        [AllowAnonymous]
        [HttpPost]
        [Route("properties/SaveProperty")]
        public JsonResult SaveProperty([FromBody] PropertyModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                foreach (var modelPropertyValue in model.PropertyValues)
                {
                    if (modelPropertyValue.Entity == null)
                    {
                        var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                        modelPropertyValue.Entity = hrsUser.Entity;
                    }
                }

                result.PropertyModel = _helperProperties.SaveProperty(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("properties/SavePropertyTypeInfoOnlyNoValues")]
        public JsonResult SavePropertyTypeInfoOnlyNoValues([FromBody] PropertyModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                result.PropertyModel = _helperProperties.SavePropertyTypeInfoOnlyNoValues(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("token.UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("properties/RemoveProperty/{propertyType}")]
        public JsonResult RemoveProperty(string propertyType)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                _helperProperties.RemoveProperty(propertyType);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("token.UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("properties/GetBaseHours")]
        public JsonResult GetBaseHours()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BaseHours = _helperProperties.GetBaseHours();
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
        [Route("properties/SaveBaseHours")]
        public JsonResult SaveBaseHours([FromBody] BaseHoursModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BaseHours = _helperProperties.SaveBaseHours(model);
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
        [Route("properties/GetTrainingHours")]
        public JsonResult GetTrainingHours()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingHours = _helperProperties.GetTrainingHours();
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
        [Route("properties/SaveTrainingHours")]
        public JsonResult SaveTrainingHours([FromBody] TrainingHoursModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingHours = _helperProperties.SaveTrainingHours(model);
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
        [Route("properties/GetTargetPercentages")]
        public JsonResult GetTargetPercentages()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TargetPercentagesModel = _helperProperties.GetTargetPercentages();
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
        [Route("properties/SaveTargetPercentages")]
        public JsonResult SaveTargetPercentages([FromBody] TargetPercentagesModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TargetPercentagesModel = _helperProperties.SaveTargetPercentages(model);
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
