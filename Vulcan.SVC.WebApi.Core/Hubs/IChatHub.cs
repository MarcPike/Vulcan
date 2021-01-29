using DAL.Vulcan.Mongo.Core.Chat;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace Vulcan.SVC.WebApi.Core.Hubs
{
    public interface IChatHub
    {
        void ChatClosed(Chat chat);
        void ChatCreated(Chat chat);
        void NewChatMessage(Chat chat, ChatMessage message);
        void UserJoined(Chat chat, CrmUserRef user);
        void UserLeft(Chat chat, CrmUserRef user);
        ChatHub GetHub();
    }
}