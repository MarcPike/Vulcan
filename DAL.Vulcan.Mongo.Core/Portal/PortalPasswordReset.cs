using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace DAL.Vulcan.Mongo.Core.Portal
{
    public class PortalPasswordReset : BaseDocument
    {
        public CompanyRef Company { get; set; }
        public ContactRef Contact { get; set; }
        public string EmailAddress { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateSent { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? EmailLinkClick { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Completed { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public PortalPasswordResetStatus Status { get; set; } = PortalPasswordResetStatus.Pending;

        public string Url => $@"www.portal.howcogroup.com/resetPassword/{Id.ToString()}";
    }
}