using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Logger;
using DAL.Vulcan.Mongo.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Vulcan.WebApi2.Hubs;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class SignalRExternalController : BaseController
    {
        //private IHelperTeamHub _helperTeamHub;
        private readonly IHelperTeam _helperTeam;
        private readonly IHelperApplication _helperApplication;
        private readonly ITeamHub _teamHub;
        private readonly IHelperNotifications _helperNotifications;
        private const string APPLICATION = "vulcancrm";

        public SignalRExternalController(
            IHelperTeam helperTeam,
            IHelperUser helperUser,
            IHelperApplication helperApplication,
            ITeamHub teamHub,
            IHelperNotifications helperNotifications) : base(helperUser)
        {
            _helperTeam = helperTeam;
            _helperApplication = helperApplication;
            _teamHub = teamHub;
            _helperNotifications = helperNotifications;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("signalRExternal/QuoteExportStatusChanged/{quoteId}")]
        public async Task<JsonResult> QuoteExportStatusChanged(string quoteId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    _teamHub.QuoteExportStatusChanged(quoteId);
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
        [Route("signalRExternal/PublishUserTeamMessage/{teamMessageId}")]
        public async Task<JsonResult> PublishUserTeamMessage(string teamMessageId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    _teamHub.PublishUserTeamMessage(teamMessageId);
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
        [Route("signalRExternal/SendNewReminderToUser/{userId}/{notificationId}/{type}")]
        public async Task<JsonResult> SendNewReminderToUser(string userId, string notificationId, string type)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    var crmUserRef = _helperUser.GetCrmUser(APPLICATION, userId);
                    var notification = _helperNotifications.GetNotification(notificationId);
                    var notificationRef = notification.AsNotificationRef();
                    _teamHub.SendRefreshRemindersForUser(crmUserRef.AsCrmUserRef(), notificationRef, type);
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
        [Route("signalRExternal/SendRefreshNotificationsForUser/{userId}")]
        public async Task<JsonResult> SendRefreshNotificationsForUser(string userId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    var crmUser = _helperUser.GetCrmUser(APPLICATION, userId);
                    var notification = crmUser.Notifications.Select(x => x.AsNotification())
                        .OrderByDescending(x => x.NotificationDate).FirstOrDefault();
                    if (notification != null)
                    {
                        _teamHub.SendRefreshNotificationsForUser(crmUser.AsCrmUserRef(), notification.Label);
                    }
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
        [Route("signalRExternal/RefreshActionsForUser/{userId}")]
        public async Task<JsonResult> RefreshActionsForUser(string userId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    var crmUser = _helperUser.GetCrmUser(APPLICATION, userId);

                    _teamHub.SendRefreshActionsForUser(crmUser.AsCrmUserRef());
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
        [Route("signalRExternal/SendRefreshEmailToUser/{userId}")]
        public async Task<JsonResult> SendRefreshEmailToUser(string userId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    var crmUser = _helperUser.GetCrmUser(APPLICATION, userId);

                    _teamHub.SendRefreshEmailForUser(userId);
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
        [Route("signalRExternal/SendRefreshEmailForContact/{contactId}")]
        public async Task<JsonResult> SendRefreshEmailForContact(string contactId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    _teamHub.SendRefreshEmailForContact(contactId);
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