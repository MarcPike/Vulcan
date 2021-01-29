using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.RequestForQuoteExternal;
using DocumentFormat.OpenXml.Spreadsheet;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.Vulcan.Mongo.Models
{
    public class RfqExternalModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public int RequestForQuoteId { get; set; } = 0;
        public string ContactEmailAddress { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string RfqText { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public RfqExternalStatus Status { get; set; }
        public int KeywordMatches { get; set; } = 0;
        public TeamRef Team { get; set; }
        public CrmUserRef SalesPerson { get; set; }

        public string AcceptText { get; set; } = string.Empty;
        public string RejectText { get; set; } = string.Empty;

        public DateTime Requested { get; set; }
        public DateTime? Reviewed { get; }

        public RfqExternalModel() { }

        public RfqExternalModel(string application, string userId, RfqExternal r)
        {
            Application = application;
            UserId = userId;
            Id = r.Id.ToString();
            RequestForQuoteId = r.RequestForQuoteId;
            ContactEmailAddress = r.ContactEmailAddress;
            ContactName = r.ContactName;
            CompanyName = r.CompanyName;
            ContactPhone = r.ContactPhone;
            RfqText = r.RfqText;
            Status = r.Status;
            KeywordMatches = r.KeywordMatches;
            Team = r.Team;
            SalesPerson = r.SalesPerson;
            Requested = r.Requested;
            Reviewed = r.Reviewed;
            AcceptText = r.AcceptText;
            RejectText = r.RejectText;
        }

    }
}