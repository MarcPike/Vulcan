using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DAL.Vulcan.Mongo.DocClass.Messages;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Vulcan.WebApi2.Hubs;
using Vulcan.WebApi2.Models;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class MessageController: BaseController
    {
        private readonly IHelperApplication _helperApplication;
        private readonly ITeamHub _teamHub;

        public MessageController(
            IHelperUser helperUser, 
            IHelperApplication helperApplication, 
            ITeamHub teamHub) : base(helperUser)
        {
            _helperApplication = helperApplication;
            _teamHub = teamHub;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("messaging/GetCreateModel/{application}/{userId}")]
        public async Task<JsonResult> GetCreateModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(application, userId);

                    result.CreateUserTeamMessageModel = new CreateUserTeamMessageModel(application, crmUser.User.Id, crmUser);
                    result.Moods = Enum.GetValues(typeof(MessageMood))
                        .Cast<MessageMood>()
                        .Select(v => v.ToString())
                        .ToList();

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
        [Route("messaging/GetSendModel/{application}/{userId}/{teamMessageId}")]
        public async Task<JsonResult> GetSendModel(string application, string userId, string teamMessageId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(application, userId);

                    result.SendUserTeamMessageModel = new SendUserTeamMessageModel()
                    {
                        Application = application,
                        UserId = crmUser.User.Id,
                        TeamMessageId = teamMessageId
                    };
                    result.Moods = Enum.GetValues(typeof(MessageMood))
                        .Cast<MessageMood>()
                        .Select(v => v.ToString())
                        .ToList();

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
        [Route("messaging/CreateTeamMessage")]
        public async Task<JsonResult> CreateTeamMessage([FromBody] CreateUserTeamMessageModel model)
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

                    var gotMood = Enum.TryParse(model.Mood, out MessageMood mood);
                    if (!gotMood) throw new Exception("Mood is unknown");

                    var rep = new RepositoryBase<UserTeamMessage>();

                    if (model.Recipients.Any())
                    {
                        if (model.Recipients.All(x => x.UserId != model.CreatedBy.Id))
                        {
                            model.Recipients.Add(model.CreatedBy);
                        }
                    }

                    var userTeamMessage = new UserTeamMessage()
                    {
                        Team = model.Team,
                        CreatedBy = model.CreatedBy,
                        OnlyIncludedUsers = model.Recipients
                    };
                    userTeamMessage.Messages.Add(new UserTeamMessageObject(model.CreatedBy, model.Message, mood));
                    rep.Upsert(userTeamMessage);

                    _teamHub.PublishUserTeamMessage(userTeamMessage.Id.ToString());

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
        [Route("messaging/GetMyTeamMessages/{application}/{userId}")]
        public async Task<JsonResult> GetMyTeamMessages(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var rep = new RepositoryBase<UserTeamMessage>();
                    var crmUser = GetCrmUser(application, userId);

                    var userTeamMessages = rep.AsQueryable().Where(x =>
                        x.OnlyIncludedUsers.Any(o => o.Id == crmUser.Id.ToString()) ||
                        ((x.OnlyIncludedUsers.Count == 0) && x.Team.Id == crmUser.ViewConfig.Team.Id)).ToList();

                    result.UserTeamMessageModels = userTeamMessages.Select(x => new UserTeamMessageModel(application, crmUser.User.Id, x)).ToList();
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
        [Route("messaging/AddToTeamMessage")]
        public async Task<JsonResult> AddToTeamMessage([FromBody] SendUserTeamMessageModel model)
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
                    var gotMood = Enum.TryParse(model.Mood, out MessageMood mood);
                    if (!gotMood) throw new Exception("Mood is unknown");

                    var rep = new RepositoryBase<UserTeamMessage>();
                    var userTeamMessage = rep.Find(model.TeamMessageId);

                    var crmUser = GetCrmUser(model.Application, model.UserId);

                    userTeamMessage.Messages.Add(new UserTeamMessageObject(crmUser.AsCrmUserRef(), model.Message, mood));
                    rep.Upsert(userTeamMessage);

                    _teamHub.PublishUserTeamMessage(model.TeamMessageId);

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
        [Route("messaging/GetTeamMembers/{application}/{userId}")]
        public async Task<JsonResult> GetTeamMembers(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(application, userId);
                    var team = crmUser.ViewConfig.Team.AsTeam();
                    result.Users = team.CrmUsers.ToList();

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
