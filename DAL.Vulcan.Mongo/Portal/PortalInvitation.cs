using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.EMail;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.Vulcan.Mongo.Portal
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

        [JsonConverter(typeof(StringEnumConverter))] 
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
