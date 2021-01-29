using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using System;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.Messages
{
    public class MessageGroup: BaseDocument
    {
        public string Subject { get; set; } = string.Empty;
        public List<CrmUserRef> GroupUsers { get; set; } = new List<CrmUserRef>();
        public List<MessageObject> Messages { get; set; } = new List<MessageObject>();
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}