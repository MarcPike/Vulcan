using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperChat
    {
        ChatModel AddMessage(ChatMessageModel model);
        void CloseChat(string chatId, string userId);
        void CreateNewChat(ChatModel model);
        List<ChatModel> GetActiveChats(string application, string userId);
        ChatModel GetNewChatModel(string application, string createdByUserId);
        ChatMessageModel GetNewChatMessageModel(string application, string userId, string chatId);
        void SaveChat(ChatModel model);
        void UserJoin(string chatId, string userId);
        void UserLeft(string chatId, string userId);
        void BindChatCreatedAction(Action<Chat.Chat> chatCreatedAction);
        void BindNewChatMessageAction(Action<Chat.Chat, Chat.ChatMessage> newChatMessageAction);
        void BindUserJoinAction(Action<Chat.Chat, CrmUserRef> userJoinAction);
        void BindUserLeftAction(Action<Chat.Chat, CrmUserRef> userLeftAction);
        void BindChatClosedAction(Action<Chat.Chat> chatClosedAction);
    }
}