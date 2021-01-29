using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Chat;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Models;
using Microsoft.AspNetCore.SignalR;

namespace Vulcan.WebApi2.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private static ChatHub _chatHub;

        private const string APP_NAME = "vulcancrm";

        public ChatHub(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            _chatHub = this;
        }

        public ChatHub GetHub()
        {
            return _chatHub;
        }

        public void UserLeft(Chat chat, CrmUserRef user)
        {
            var chatModel = new ChatModel(APP_NAME, user.UserId, chat);
            _hubContext.Clients.All.SendAsync("userLeftChat", chatModel, user);
        }

        public void UserJoined(Chat chat, CrmUserRef user)
        {
            var chatModel = new ChatModel(APP_NAME, user.UserId, chat);
            _hubContext.Clients.All.SendAsync("userJoinedChat", chatModel, user);
        }

        public void NewChatMessage(Chat chat, ChatMessage message)
        {
            var chatModel = new ChatModel(APP_NAME, message.User.UserId, chat);
            _hubContext.Clients.All.SendAsync("newChatMessage", chatModel, message);
        }

        public void ChatCreated(Chat chat)
        {
            var chatModel = new ChatModel(APP_NAME, chat.CreatedByUserId, chat);
            _hubContext.Clients.All.SendAsync("newChatCreated", chatModel);
        }

        public void ChatClosed(Chat chat)
        {
            var chatModel = new ChatModel(APP_NAME, chat.CreatedByUserId, chat);
            _hubContext.Clients.All.SendAsync("chatClosed", chatModel);
        }

    }
}
