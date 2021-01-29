using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using DAL.Common.DocClass;
using DAL.Common.Models;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.HRS.SqlServer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using HrRepresentative = DAL.HRS.Mongo.DocClass.Employee.HrRepresentative;


namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class LocationController : BaseController
    {
        private readonly IHelperLocation _helperLocation;

        public LocationController(IHelperUser helperUser, IHelperLocation helperLocation) : base()
        {
            _helperLocation = helperLocation;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("location/GetAllLocations")]
        public JsonResult GetAllLocations()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var locations = _helperLocation.GetAllLocations();
                result.Locations = locations;
                result.LocationRefs = locations.Select(x => x.AsLocationRef()).OrderBy(x => x.Office).ToList();

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("location/GetLocation/{locationId}")]
        public JsonResult GetLocation(string locationId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.LocationModel = _helperLocation.GetLocation(locationId);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("location/GetNewLocationModel")]
        public JsonResult GetNewLocationModel()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.LocationModel = _helperLocation.GetNewLocationModel();

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("location/GetNewMapLocation/{x}/{y}")]
        public JsonResult GetNewMapLocation(double x, double y)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.MapLocation = _helperLocation.GetNewMapLocation(x, y);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("location/SaveLocation")]
        public JsonResult SaveLocation([FromBody] LocationModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.LocationModel = _helperLocation.SaveLocation(model);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("location/GetMyLocation")]
        public JsonResult GetMyLocation()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var user = GetUser(tokenData.UserId);

                result.Location = _helperLocation.GetLocation(user.Location.Id.ToString());

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("location/GetRepresentativeForLocation")]
        public JsonResult GetRepresentativeForLocation([FromBody] LocationRef location)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var user = GetUser(tokenData.UserId);

                result.HrRep =  HrRepresentative.GetRepresentativeForLocation(location);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("location/SaveHrRepresentative")]
        public JsonResult HrRepresentativeModel([FromBody] HrRepresentativeModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var user = GetUser(tokenData.UserId);

                result.Location = _helperLocation.SaveHrRepresentative(model);

                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

    }
}