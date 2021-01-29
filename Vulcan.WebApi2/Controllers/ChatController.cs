using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DAL.Marketing.Helpers;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.Chat;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Vulcan.WebApi2.Hubs;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class ChatController : BaseController
    {
        private readonly IHelperChat _helperChat;
        private readonly ChatHub _chatHub;

        public ChatController(IHelperChat helperChat, IChatHub chatHub, IHelperUser helperUser) : base(helperUser)
        {
            _helperChat = helperChat;
            BindToHelperChatActions();
            _chatHub = chatHub.GetHub();
        }

        private void HandleUserLeft(Chat chat, CrmUserRef user)
        {
            _chatHub.UserLeft(chat, user);
        }

        private void HandleUserJoined(Chat chat, CrmUserRef user)
        {
            _chatHub.UserJoined(chat, user);
        }

        private void HandleNewChatMessage(Chat chat, ChatMessage message)
        {
            _chatHub.NewChatMessage(chat, message);
        }

        private void HandleChatCreated(Chat chat)
        {
            _chatHub.ChatCreated(chat);
        }

        private void HandleChatClosed(Chat chat)
        {
            _chatHub.ChatClosed(chat);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("chat/GetCrmUsersForMyTeam/{application}/{userId}")]
        public async Task<JsonResult> GetCrmUsersForMyTeam(string application, string userId)
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
                    if (crmUser == null) throw new Exception("User not found");

                    var team = crmUser.ViewConfig.Team.AsTeam();
                    var userList = new List<CrmUserRef>();
                    var repUser = new RepositoryBase<CrmUser>();
                    foreach (var crmUserRef in team.CrmUsers)
                    {
                        var thisUser = repUser.Find(crmUserRef.Id);
                        if (thisUser != null)
                        {
                            userList.Add(thisUser.AsCrmUserRef());
                        }
                    }

                    result.CrmUserRefList = userList.OrderBy(x => x.FullName);
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
        [Route("chat/AddMessage")]
        public async Task<JsonResult> AddMessage([FromBody] ChatMessageModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperChat.AddMessage(model);
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
        [Route("chat/CloseChat/{application}/{userId}/{chatId}")]
        public async Task<JsonResult> CloseChat(string application, string userId, string chatId)
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
                    if (crmUser == null) throw new Exception("User not found");


                    BindToHelperChatActions();
                    _helperChat.CloseChat(chatId, crmUser.Id.ToString());
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
        [Route("chat/GetActiveChats/{application}/{userId}")]
        public async Task<JsonResult> GetActiveChats(string application, string userId)
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
                    if (crmUser == null) throw new Exception("User not found");

                    result.ActiveChatModels = _helperChat.GetActiveChats(application, crmUser.Id.ToString());
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
        [Route("chat/GetNewChatModel/{application}/{userId}")]
        public async Task<JsonResult> GetNewChatModel(string application, string userId)
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
                    if (crmUser == null) throw new Exception("User not found");


                    result.NewChatModel = _helperChat.GetNewChatModel(application, crmUser.Id.ToString());
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
        [Route("chat/GetNewChatMessageModel/{application}/{userId}/{chatId}")]
        public async Task<JsonResult> GetNewChatMessageModel(string application, string userId, string chatId)
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
                    if (crmUser == null) throw new Exception("User not found");

                    result.NewChatMessageModel = _helperChat.GetNewChatMessageModel(application, crmUser.Id.ToString(), chatId);
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
        [Route("chat/SaveChat")]
        public async Task<JsonResult> SaveChat([FromBody] ChatModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperChat.SaveChat(model);
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
        [Route("chat/UserJoin/{application}/{userId}/{chatId}")]
        public async Task<JsonResult> UserJoin(string application, string userId, string chatId)
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
                    if (crmUser == null) throw new Exception("User not found");


                    _helperChat.UserJoin(chatId, crmUser.Id.ToString());
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
        [Route("chat/UserLeft/{application}/{userId}/{chatId}")]
        public async Task<JsonResult> UserLeft(string application, string userId, string chatId)
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
                    if (crmUser == null) throw new Exception("User not found");

                    _helperChat.UserLeft(chatId, crmUser.Id.ToString());
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

        private void BindToHelperChatActions()
        {
            _helperChat.BindChatCreatedAction(HandleChatCreated);
            _helperChat.BindChatClosedAction(HandleChatClosed);
            _helperChat.BindNewChatMessageAction(HandleNewChatMessage);
            _helperChat.BindUserJoinAction(HandleUserJoined);
            _helperChat.BindUserLeftAction(HandleUserLeft);
        }

    }
}
