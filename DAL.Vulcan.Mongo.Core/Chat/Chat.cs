using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Chat
{
    public class Chat : BaseDocument
    {

        public TeamRef Team { get; set; }
        public List<CrmUserRef> ActiveUsers { get; set; } = new List<CrmUserRef>();
        public List<CrmUserRef> NonActiveUsers { get; set; } = new List<CrmUserRef>();
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public CrmUserRef CreatedBy { get; set; }
        public bool IsActive { get; set; } = true;

        public Chat()
        {
        }

        public Chat(TeamRef team, List<CrmUserRef> activeUsers, CrmUserRef createdBy, ChatMessage initialMessage = null)
        {
            if (activeUsers.All(x => x.Id != createdBy.Id))
            {
                activeUsers.Insert(0,createdBy);
            }

            Team = team;
            ActiveUsers = activeUsers;
            CreatedBy = createdBy;
            CreateDateTime = DateTime.Now;
            if (initialMessage != null) Messages.Add(initialMessage);
        }

        public void UserJoin(CrmUserRef user)
        {
            ActiveUsers.Add(user);
            Messages.Add(new ChatMessage(user, $"{user.FullName} joined this chat") { MessageType = ChatMessageType.UserJoin.ToString() });

        }

        public void UserLeft(CrmUserRef user)
        {
            var activeUser = ActiveUsers.SingleOrDefault(x => x.Id == user.Id);
            if (activeUser != null)
            {
                ActiveUsers.Remove(activeUser);
                NonActiveUsers.Add(activeUser);
            }

            if (ActiveUsers.Count == 0)
            {
                IsActive = false;
            }

            Messages.Add(new ChatMessage(user, $"{user.FullName} left this chat") { MessageType = ChatMessageType.UserLeft.ToString() });

        }

        public void AddMessage(ChatMessage message)
        {
            if (ActiveUsers.All(x => x.Id != message.User.Id))
            {
                throw new Exception("You are not in this Chat session");
            }
            Messages.Add(message);
        }

        public void ChatClosed(CrmUserRef user)
        {
            if (user.Id != CreatedBy.Id)
            {
                throw new Exception("Chat can only be closed by User");
            }
            IsActive = false;
        }
    }
}
