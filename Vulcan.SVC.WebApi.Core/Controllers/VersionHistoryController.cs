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
    public class VersionHistoryController : BaseController
    {
        private readonly IHelperVersionHistory _helperVersionHistory;

        public VersionHistoryController(IHelperVersionHistory helperVersionHistory, IHelperUser helperUser) : base(helperUser)
        {
            _helperVersionHistory = helperVersionHistory;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("versionHistory/AddBugReport/{application}/{userId}/{notes}")]
        public async Task<JsonResult> AddBugReport(string application, string userId, string notes)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    _helperVersionHistory.AddBugReport(application, userId, crmUser.AsCrmUserRef(), notes);
                    result.CurrentVersionHistoryModel = _helperVersionHistory.GetCurrentVersionHistoryModel(application, userId);
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
        [Route("versionHistory/AddFeature/{application}/{userId}/{notes}")]
        public async Task<JsonResult> AddFeature(string application, string userId, string notes)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    _helperVersionHistory.AddFeature(application, userId, crmUser.AsCrmUserRef(), notes);
                    result.CurrentVersionHistoryModel = _helperVersionHistory.GetCurrentVersionHistoryModel(application, userId);
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
        [Route("versionHistory/GetAllVersionHistory/{application}/{userId}")]
        public async Task<JsonResult> GetAllVersionHistory(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.AllVersionHistoryModels = _helperVersionHistory.GetAllVersionHistory(application, userId);
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
        [Route("versionHistory/GetCurrentVersionHistoryModel/{application}/{userId}")]
        public async Task<JsonResult> GetCurrentVersionHistoryModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.CurrentVersionHistory = _helperVersionHistory.GetCurrentVersionHistoryModel(application, userId);
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
        [HttpPost]
        [Route("versionHistory/SaveVersionHistory")]
        public async Task<JsonResult> SaveVersionHistory([FromBody] VersionHistoryModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.CurrentVersionHistory = _helperVersionHistory.SaveVersionHistory(model);
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
