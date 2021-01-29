using DAL.Vulcan.Mongo.Core.Chat;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class ChatModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public TeamRef Team { get; set; }
        public List<CrmUserRef> ActiveUsers { get; set; } = new List<CrmUserRef>();
        public List<CrmUserRef> NonActiveUsers { get; set; } = new List<CrmUserRef>();
        public List<ChatMessageModel> Messages { get; set; } = new List<ChatMessageModel>();
        public CrmUserRef CreatedBy { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool IsActive { get; set; } = true;


        public ChatModel()
        {
        }

        public ChatModel(string application, string userId, Chat.Chat chat)
        {
            Application = application;
            UserId = userId;
            Id = chat.Id.ToString();
            Team = chat.Team;
            ActiveUsers = chat.ActiveUsers;
            NonActiveUsers = chat.NonActiveUsers;
            Messages = chat.Messages.Select(x=> new ChatMessageModel(application, userId, chat.Id.ToString(),x)).ToList().OrderByDescending(x => x.CreatedOn).ToList();
            CreatedBy = chat.CreatedBy;
            CreateDateTime = chat.CreateDateTime;
            IsActive = chat.IsActive;
        }
    }

    public class ChatMessageModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string ChatId { get; set; }
        public Guid Id { get; set; }
        public CrmUserRef User { get; set; }
        public string HtmlMessage { get; set; }
        public DateTime CreatedOn { get; set; }

        public ChatMessageModel()
        {
        }

        public ChatMessageModel(string application, string userId, string chatId, ChatMessage message)
        {
            ChatId = chatId;
            Application = application;
            UserId = userId;
            Id = message.Id;
            User = message.User;
            CreatedOn = message.CreatedOn;
            HtmlMessage = message.HtmlMessage;
        }

        public ChatMessage AsChatMessage()
        {
            return new ChatMessage()
            {
                Id = this.Id,
                User = this.User,
                HtmlMessage = this.HtmlMessage,
                CreatedOn = this.CreatedOn
            };
        }

    }
}
