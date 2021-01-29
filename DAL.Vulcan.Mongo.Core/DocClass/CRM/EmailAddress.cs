using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class EmailAddress : IObjectWithGuidForId
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonConverter(typeof(JsonStringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)]
        public EmailType Type { get; set; } = EmailType.Business;
        public string Address { get; set; } = String.Empty;
        public List<string> SearchTags { get; set; } = new List<string>();

    }
}