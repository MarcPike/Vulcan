using System;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Chat
{
    public enum ChatMessageType
    {
        Normal,
        UserLeft,
        UserJoin
    }

    public class ChatMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public CrmUserRef User { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]

        public DateTime CreatedOn { get; set; }
        public string HtmlMessage { get; set; }
        public string MessageType { get; set; } = ChatMessageType.Normal.ToString();

        public ChatMessage()
        {
        }

        public ChatMessage(CrmUserRef user, string htmlMessage)
        {
            User = user;
            HtmlMessage = htmlMessage;
            CreatedOn = DateTime.Now;
        }
    }
}