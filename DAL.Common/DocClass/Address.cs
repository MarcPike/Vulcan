using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;

namespace DAL.Common.DocClass
{
    public class Address : ObjectWithTags, IObjectWithGuidForId
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        //[JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)]
        public AddressType Type { get; set; } = AddressType.Office;
        public string AddressLine1 { get; set; } = String.Empty;
        public string AddressLine2 { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string County { get; set; } = String.Empty;
        public string StateProvince { get; set; } = String.Empty;
        public string PostalCode { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;
        public List<string> SearchTags { get; set; } = new List<string>();
        public string Name { get; set; } = string.Empty;
        public int ExternalSqlId { get; set; } = 0;
        public string ExternalCode { get; set; } = string.Empty;
        public string ExternalStatus { get; set; } = string.Empty;
        public bool ExternalExists { get; set; } = false;

        public int HashCode
        {
            get
            {
                var value = AddressLine1 + City + StateProvince + PostalCode;
                return value.GetHashCode();
            }
        }

        public bool IsValid()
        {
            return !String.IsNullOrWhiteSpace(AddressLine1)
                   && !String.IsNullOrWhiteSpace(City)
                   && !String.IsNullOrWhiteSpace(StateProvince)
                   && !String.IsNullOrWhiteSpace(PostalCode);
        }

        public Address()
        {
        }


    }
}