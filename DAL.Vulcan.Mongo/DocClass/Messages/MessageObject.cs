using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.Messages
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
