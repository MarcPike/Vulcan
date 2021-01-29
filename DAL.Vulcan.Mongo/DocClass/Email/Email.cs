using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.DocClass.Email
{
    
    public class Email: BaseDocument
    {
        public class EmailAddressData
        {
            public string Name { get; set; }
            public string Address { get; set; }

            public EmailAddressData(string name, string address)
            {
                Name = name;
                Address = address;
            }

            public EmailAddressData()
            {
                
            }
        }
        public ItemId EmailId { get; set; }
        public EmailAddressData From { get; set; } 
        public List<EmailAddressData> To { get; set; }
        public List<EmailAddressData> Cc { get; set; } = new List<EmailAddressData>();
        public List<EmailAddressData> Bcc { get; set; } = new List<EmailAddressData>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public DateTime Sent { get; set; }
        public DateTime Received { get; set; }
        public bool AssociationsBuilt { get; set; }
        public List<ObjectId> AttachmentObjectIds { get; set; } = new List<ObjectId>();

        public Email()
        {
        }

        public Email(ItemId emailId, Microsoft.Exchange.WebServices.Data.EmailAddress from, EmailAddressCollection to,
            string subject, string body, AttachmentCollection attachments, 
            DateTime created, DateTime sent, DateTime received, EmailAddressCollection cc, EmailAddressCollection bcc)
        {
            EmailId = emailId;
            From = new EmailAddressData(from.Name, from.Address);
            To = to.Select(x => new EmailAddressData(x.Name, x.Address)).ToList();
            Cc = cc?.Select(x => new EmailAddressData(x.Name, x.Address)).ToList() ?? new List<EmailAddressData>();
            Bcc = bcc?.Select(x => new EmailAddressData(x.Name, x.Address)).ToList() ?? new List<EmailAddressData>();
            Subject = subject;
            Body = body;
            //Attachments = attachments;
            Created = created;
            Sent = sent;
            Received = received;

            if (attachments.Count > 0)
            {
                EmailAttachmentImport.Execute(this,attachments);
            }

            var rep = new RepositoryBase<Email>();
            rep.Upsert(this);

            BuildAssociations();
        }

        public EmailRef AsEmailRef()
        {
            return new EmailRef(this);
        }

        public void BuildAssociations()
        {
            if (AssociationsBuilt) return;
            EmailPublisherContact.Execute(this);
            EmailPublisherCrmUser.Execute(this);
            AssociationsBuilt = true;
            SaveToDatabase();
        }
    }
}
