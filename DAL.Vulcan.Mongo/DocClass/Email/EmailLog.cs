using System;
using System.Collections.Generic;
using AspNetCore.Identity.MongoDB.Validators;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Email
{
    public class EmailLog: BaseDocument
    {
        public DateTime ExecutedOn { get; set; }
        public List<EmailRef> EmailsAdded { get; set; } = new List<EmailRef>();
        public TimeSpan ProcessTime { get; set; }
    }
}