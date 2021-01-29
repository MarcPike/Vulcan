using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DAL.Vulcan.Mongo.Core.DocClass.Messages
{
    public class MessageObject: BaseDocument
    {
        public CrmUserRef FromUser { get; set; }
        public string Message { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime MessageDateTime { get; set; }

        public MessageObject() { }
        public MessageObject(CrmUserRef from, string message)
        {
            FromUser = from;
            Message = message;
            MessageDateTime = DateTime.Now;
        }
    }
}
