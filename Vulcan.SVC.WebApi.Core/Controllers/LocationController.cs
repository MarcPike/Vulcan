using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class LocationController : BaseController
    {
        private IHelperApplication _helperApplication;
        private IHelperLocation _helperLocation;

        public LocationController(IHelperUser helperUser, IHelperApplication helperApplication, IHelperLocation helperLocation) : base(helperUser)
        {
            _helperApplication = helperApplication;
            _helperLocation = helperLocation;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("location/GetAllLocations/{application}/{userId}")]
        public async Task<JsonResult> GetAllLocations(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperApplication.VerifyApplication(application);

                    var locations = _helperLocation.GetAllLocations();
                    result.Locations = locations.Select(x=> new LocationModel(x)).ToList();
                    result.LocationRefs = locations.Select(x => x.AsLocationRef()).ToList();

                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("location/GetMyLocation/{application}/{userId}")]
        public async Task<JsonResult> GetMyLocation(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var user = GetUser(userId);
                    result.Location = _helperLocation.GetLocation(user.Location.Id.ToString());
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }


    }
}