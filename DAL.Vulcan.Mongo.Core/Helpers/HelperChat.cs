using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.Chat;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Helpers
{

    public class HelperChat : HelperBase, IHelperChat
    {

        public Action<Chat.Chat> ChatCreatedAction;
        public Action<Chat.Chat, Chat.ChatMessage> NewChatMessageAction;
        public Action<Chat.Chat, CrmUserRef> UserJoinAction;
        public Action<Chat.Chat, CrmUserRef> UserLeftAction;
        public Action<Chat.Chat> ChatClosedAction;

        public List<ChatModel> GetActiveChats(string application, string userId)
        {
            var chats = new RepositoryBase<Chat.Chat>().AsQueryable().Where(x => x.ActiveUsers.Any(u => u.Id == userId) && x.IsActive)
                .ToList();
            return chats.Select(x => new ChatModel(application,userId, x)).ToList();
        }

        public ChatModel GetNewChatModel(string application,  string createdByUserId)
        {
            var user = GetUserRef(createdByUserId);

            var chat = new Chat.Chat()
            {
                CreatedBy = user, Team = user.AsCrmUser().ViewConfig.Team, ActiveUsers = new List<CrmUserRef>() {user}
            };
            return new ChatModel(application,createdByUserId, chat);
        }

        public ChatMessageModel GetNewChatMessageModel(string application, string userId, string chatId)
        {
            var user = GetUserRef(userId);
            var chatMessage = new ChatMessage(user,string.Empty);
            return new ChatMessageModel(application, userId, chatId, chatMessage);
        }

        public void CreateNewChat(ChatModel model)
        {
            var repChat = GetChat(model.Id, out var chat, checkExist: false);
            if (chat == null)
            {
                chat = new Chat.Chat(model.Team, model.ActiveUsers, model.CreatedBy, model.Messages.FirstOrDefault()?.AsChatMessage());
                chat.Id = ObjectId.Parse(model.Id);
                repChat.Upsert(chat);
                //ChatCreatedEvent?.Invoke(chat);
                ChatCreatedAction?.Invoke(chat);
            }
        }

        public void SaveChat(ChatModel model)
        {
            var repChat = GetChat(model.Id, out var chat, checkExist: false);
            if (chat == null)
            {
                CreateNewChat(model);
            }
            else
            {
                if (chat.IsActive && (model.IsActive == false))
                {
                    CloseChat(model.Id, model.CreatedBy.Id);
                    return;
                }

                foreach (var newUser in model.ActiveUsers.Where(x=> chat.ActiveUsers.All(u=>u.Id != x.Id)).ToList())
                {
                    UserJoin(chat.Id.ToString(), newUser.Id);
                }
                chat.ActiveUsers = model.ActiveUsers;

                foreach (var leftUser in model.NonActiveUsers.Where(x => chat.NonActiveUsers.All(u => u.Id != x.Id)).ToList())
                {
                    UserLeft(chat.Id.ToString(), leftUser.Id);
                }
                chat.NonActiveUsers = model.NonActiveUsers;

                foreach (var newChatMessage in model.Messages.Where(x=> chat.Messages.All(m=>m.Id != x.Id )).ToList())
                {
                    var newChatMessageModel = new ChatMessageModel()
                    {
                        ChatId = chat.Id.ToString(),
                        Id = newChatMessage.Id,
                        CreatedOn = newChatMessage.CreatedOn,
                        User = newChatMessage.User,
                        HtmlMessage = newChatMessage.HtmlMessage
                    };

                    AddMessage(newChatMessageModel);

                }
                chat.Messages = model.Messages.Select(x=> x.AsChatMessage()).ToList().OrderByDescending(x=>x.CreatedOn).ToList();

                repChat.Upsert(chat);
            }
        }

        public void CloseChat(string chatId, string userId)
        {
            var repChat = GetChat(chatId, out var chat);
            var user = GetUserRef(userId);

            if (chat.CreatedBy.Id == userId)
            {
                if (!chat.IsActive) return;

                chat.IsActive = false;
                repChat.Upsert(chat);
                ChatClosedAction?.Invoke(chat);
            }
            else
            {
                throw new Exception($"Only {user.FullName} can Close this chat");
            }
        }


        public ChatModel AddMessage(ChatMessageModel model)
        {
            var repChat = GetChat(model.ChatId, out var chat);
            var user = model.User;

            var chatMessage = new ChatMessage()
            {
                Id = model.Id,
                User = model.User,
                HtmlMessage = model.HtmlMessage,
                CreatedOn = model.CreatedOn
            };

            chat.AddMessage(chatMessage);
            repChat.Upsert(chat);
            NewChatMessageAction?.Invoke(chat, chatMessage);
            return new ChatModel(model.Application, model.UserId, chat);
        }

        public void UserJoin(string chatId, string userId)
        {
            var repChat = GetChat(chatId, out var chat);
            var user = GetUserRef(userId);

            chat.UserJoin(user);
            repChat.Upsert(chat);
            UserJoinAction?.Invoke(chat, user);
        }

        public void UserLeft(string chatId, string userId)
        {
            var repChat = GetChat(chatId, out var chat);
            var user = GetUserRef(userId);

            chat.UserLeft(user);
            repChat.Upsert(chat);
            UserLeftAction?.Invoke(chat, user);
        }

        public void BindChatCreatedAction(Action<Chat.Chat> chatCreatedAction)
        {
            if (ChatCreatedAction == null)
                ChatCreatedAction = chatCreatedAction;
        }

        public void BindNewChatMessageAction(Action<Chat.Chat, Chat.ChatMessage> newChatMessageAction)
        {
            if (NewChatMessageAction == null)
                NewChatMessageAction = newChatMessageAction;
        }

        public void BindUserJoinAction(Action<Chat.Chat, CrmUserRef> userJoinAction)
        {
            if (UserJoinAction == null)
                UserJoinAction = userJoinAction;
        }
        public void BindUserLeftAction(Action<Chat.Chat, CrmUserRef> userLeftAction)
        {
            if (UserJoinAction == null)
                UserJoinAction = userLeftAction;
        }

        public void BindChatClosedAction(Action<Chat.Chat> chatClosedAction)
        {
            if (ChatClosedAction == null)
                ChatClosedAction = chatClosedAction;
        }


        private static RepositoryBase<Chat.Chat> GetChat(string chatId, out Chat.Chat chat, bool checkExist = true)
        {
            var repChat = new RepositoryBase<Chat.Chat>();
            chat = repChat.Find(chatId);

            if (checkExist)
            {
                if (chat == null) throw new Exception("Chat not found");
            }
            return repChat;
        }
        private static CrmUserRef GetUserRef(string userId)
        {
            var tokenUser = new RepositoryBase<CrmUserToken>().Find(userId);
            if (tokenUser != null)
            {
                return tokenUser.CrmUserRef;
            }

            var user = new RepositoryBase<CrmUser>().Find(userId);
            if (user == null) throw new Exception("User not found");
            return user.AsCrmUserRef();
        }

    }
}
