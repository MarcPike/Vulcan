using DAL.Vulcan.Mongo.Chat;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace Vulcan.WebApi2.Hubs
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