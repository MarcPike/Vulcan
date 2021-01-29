using System;
using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;

namespace DAL.Common.DocClass
{
    public class PhoneNumber : IObjectWithGuidForId
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        //[JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)]
        public PhoneType Type { get; set; } = PhoneType.Mobile;
        public string Number { get; set; } = String.Empty;
        public List<string> SearchTags { get; set; } = new List<string>();

    }
}