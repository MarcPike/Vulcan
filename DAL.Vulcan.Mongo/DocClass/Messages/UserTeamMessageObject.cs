using System;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.Messages
{
    public class UserTeamMessageObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public CrmUserRef FromUser { get; set; }
        public string Message { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime MessageDateTime { get; set; }
        public MessageMood Mood { get; set; } = MessageMood.Normal;

        public UserTeamMessageObject()
        {
        }

        public UserTeamMessageObject(CrmUserRef fromUser, string message, MessageMood mood = MessageMood.Normal)
        {

        }

    }


}