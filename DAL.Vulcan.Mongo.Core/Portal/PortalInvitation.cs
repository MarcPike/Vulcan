using BLL.EMail;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using BLL.EMail.Core;

namespace DAL.Vulcan.Mongo.Core.Portal
{
    public class PortalInvitation : BaseDocument
    {
        public CompanyRef Company { get; set; }
        public ContactRef Contact { get; set; }
        public string EmailAddress { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public CrmUserRef SalesPerson { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateSent { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))] 
        [BsonRepresentation(BsonType.String)] 
        public PortalInvitationStatus Status { get; set; } = PortalInvitationStatus.Pending;

        public string Url => $@"www.portal.howcogroup.com/invitation/{Id.ToString()}";

        public void SendEmail()
        {
            var salesPerson = SalesPerson.AsCrmUser();

            var salesPersonEmailAddress = salesPerson.User.AsUser().Person.EmailAddresses
                .FirstOrDefault(x => x.Type == EmailType.Business)?.Address;

            if (salesPersonEmailAddress == null) throw new Exception("Could not resolve SalesPerson email address");

            EMailBuilder email = new EMailBuilder();
            email.Subject = EmailSubject;
            email.Body = EmailBody;
            email.Recipients.Add(EmailAddress);
            email.EMailFromAddress = salesPersonEmailAddress;
            email.Send(true);

            DateSent = DateTime.Now;
            Status = PortalInvitationStatus.Pending;
            this.SaveToDatabase();
        }
    }
}
