using System;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class TeamMessage
    {
        public UserRef User { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime MessageDate { get; set; } = DateTime.Now;

        public string Message { get; set; }

        public TeamMessage()
        {
        }

        public TeamMessage(UserRef user, string message)
        {
            User = user;
            Message = message;
        }
    }
}