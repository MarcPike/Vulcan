using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Portal;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class NewPortalInvitationModel
    {
        public CompanyRef Company { get; set; }
        public ContactRef Contact { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string EmailSubject { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;
        public CrmUserRef SalesPerson { get; set; }
    }

    public class PortalInvitationModel
    {
        public string Id { get; set; }
        public CompanyRef Company { get; set; }
        public ContactRef Contact { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string EmailSubject { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateSent { get; set; }
        public CrmUserRef SalesPerson { get; set; }
        public string Status { get; set; }

        public PortalInvitationModel()
        {
        }

        public PortalInvitationModel(PortalInvitation inv)
        {
            Id = inv.Id.ToString();
            Company = inv.Company;
            SalesPerson = inv.SalesPerson;
            Contact = inv.Contact;
            DateSent = inv.DateSent;
            EmailAddress = inv.EmailAddress;
            EmailSubject = inv.EmailSubject;
            EmailBody = inv.EmailBody;
            Status = inv.Status.ToString();
        }
    }

}
