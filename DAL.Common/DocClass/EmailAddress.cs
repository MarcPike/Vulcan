using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DAL.Common.DocClass
{
    public class EmailAddress : IObjectWithGuidForId
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        //[JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)]
        public EmailType Type { get; set; } = EmailType.Business;
        public string Address { get; set; } = String.Empty;
        public List<string> SearchTags { get; set; } = new List<string>();

    }
}