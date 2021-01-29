using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Finders;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Vulcan.SVC.WebApi.Core.Hubs;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class ActionController : BaseController
    {
        //private readonly IHelperUser _helperUser;
        private readonly IHelperApplication _helperApplication;
        private readonly IHelperAction _helperAction;
        private readonly ITeamHub _teamHub;

        public ActionController(
            IHelperUser helperUser,
            IHelperApplication helperApplication,
            IHelperAction helperAction,
            ITeamHub teamHub) : base(helperUser)

        {
            _helperApplication = helperApplication;
            _helperAction = helperAction;
            _teamHub = teamHub;
            //_teamHub.Initialize();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("action/GetActionScheduleModel/{application}/{userId}/{actionId}")]
        public async Task<JsonResult> GetActionScheduleModel(string application, string userId, string actionId)
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
                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    result.ActionScheduleModel = _helperAction.GetActionScheduleModel(application, crmUser.User.Id, actionId);
                    result.Success = true;
                });

            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.Success = false;
                result.ErrorMessage = e.Message;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("action/ApplyActionSchedule")]
        public async Task<JsonResult> ApplyActionSchedule([FromBody] ActionScheduleModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    CheckForModelErrors();

                    result.Success = _helperAction.ApplyActionSchedule(model);
                });

            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.Success = false;
                result.ErrorMessage = e.Message;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("action/GetOpenActionsForUser/{application}/{userId}/{minDate}/{maxDate}")]
        public async Task<JsonResult> GetOpenActionsForUser(string application, string userId, DateTime minDate, DateTime maxDate)
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
                    result.ActionRefs = _helperAction.GetOpenActionsForUser(application, crmUser.User.Id, minDate, maxDate);
                    result.Success = true;

                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.Success = false;
                result.ErrorMessage = e.Message;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("action/GetClosedActionsForUser/{application}/{userId}/{minDate}/{maxDate}")]
        public async Task<JsonResult> GetClosedActionsForUser(string application, string userId, DateTime minDate, DateTime maxDate)
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
                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    result.ActionRefs = _helperAction.GetClosedActionsForUser(application, crmUser.User.Id, minDate, maxDate);
                    result.Success = true;

                });

            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.Success = false;
                result.ErrorMessage = e.Message;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("action/SearchActionsForUser/{application}/{userId}/{searchFor}/{isDeepSearch}/{minDate}/{maxDate}")]
        public async Task<JsonResult> SearchActionsForUser(string application, string userId, string searchFor, bool isDeepSearch, DateTime minDate, DateTime maxDate)
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
                    var allActionRefs = _helperAction.GetOpenActionsForUser(application, crmUser.User.Id, minDate, maxDate) ?? new List<ActionRef>();
                    if (allActionRefs.Count > 0)
                    {
                        var actionSearch = new ActionSearch();
                        var searchLevel = (isDeepSearch) ? ActionSearchLevel.Deep : ActionSearchLevel.Shallow;
                        actionSearch.ExecuteFind(
                            searchFor, allActionRefs.Select(x => x.AsAction()).ToList(), searchLevel);
                        result.SearchResults = actionSearch.Results.Select(x => x.Value).ToList();
                    }
                    else
                    {
                        result.SearchResults = allActionRefs;
                    }
                    result.Success = true;
                });

            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.Success = false;
                result.ErrorMessage = e.Message;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("action/GetAction/{application}/{userId}/{actionId}")]
        public async Task<JsonResult> GetAction(string application, string userId, string actionId)
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

                    var crmUser = _helperUser.GetCrmUser(application, userId);

                    result.ActionModel = _helperAction.GetActionModel(application, crmUser.User.Id, actionId);
                    result.ActionTypes = Enum.GetNames(typeof(ActionType)).ToList();

                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.Success = false;
                result.ErrorMessage = e.Message;
            }
            return JsonResultWithStatusCode(result, statusCode);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("action/CreateNewAction/{application}/{userId}")]
        public async Task<JsonResult> CreateNewAction(string application, string userId)
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

                    var crmUser = _helperUser.GetCrmUser(application, userId);

                    result.ActionModel = _helperAction.CreateNewAction(application, crmUser.User.AsUser());
                    result.ActionTypesSupported = Enum.GetNames(typeof(ActionType)).ToList();

                    result.Success = true;

                });

            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.Success = false;
                result.ErrorMessage = e.Message;
            }
            return JsonResultWithStatusCode(result, statusCode);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("action/SaveAction")]
        public async Task<JsonResult> SaveAction([FromBody] ActionModel model)
        {
            
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    CheckForModelErrors();
                    _helperApplication.VerifyApplication(model.Application);
                    var action = _helperAction.SaveAction(model);
                    var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);

                    result.ActionModel = ActionModel.GetActionModel(model.Application, crmUser.User.Id, model.Id);
                    result.ActionRef = action.AsActionRef();
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.Success = false;
                result.ErrorMessage = e.Message;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("action/RemoveAction/{application}/{userId}/{actionId}")]
        public async Task<JsonResult> RemoveAction(string application, string userId, string actionId)
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

                    _helperAction.RemoveAction(application, crmUser.User.Id, actionId);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.Success = false;
                result.ErrorMessage = e.Message;
            }
            return JsonResultWithStatusCode(result, statusCode);
        }

    }
}