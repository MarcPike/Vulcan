using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.DocClass.Messages
{
    public class MessageGroup: BaseDocument
    {
        public string Subject { get; set; } = string.Empty;
        public List<CrmUserRef> GroupUsers { get; set; } = new List<CrmUserRef>();
        public List<MessageObject> Messages { get; set; } = new List<MessageObject>();
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}