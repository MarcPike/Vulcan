using System;
using System.Dynamic;
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
    public class NotificationsController : BaseController
    {
        private readonly IHelperNotifications _helperNotifications;
        private readonly IHelperApplication _helperApplication;

        public NotificationsController(
            IHelperUser helperUser,
            IHelperNotifications helperNotifications,
            IHelperApplication helperApplication) : base(helperUser)
        {
            _helperNotifications = helperNotifications;
            _helperApplication = helperApplication;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("notifications/GetUserNotifications/{application}/{userId}")]
        public async Task<JsonResult> GetUserNotifications(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.Notifications = _helperNotifications.GetMyNotifications(application, userId);
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
        [Route("notifications/GetUserTeamNotifications/{application}/{userId}")]
        public async Task<JsonResult> GetUserTeamNotifications(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.Notifications = _helperNotifications.GetMyTeamNotifications(application, userId);

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
        [Route("notifications/GetNotificationModel/{application}/{userId}/{activityId}")]
        public async Task<JsonResult> GetNotificationModel(string application, string userId, string activityId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.NotificationModel = new NotificationModel(application, userId, _helperNotifications.GetNotification(activityId));
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
