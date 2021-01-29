using DAL.iMetal.Core.Models;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DAL.Vulcan.Mongo.Core.DocClass.Locations
{
    public class Address : ObjectWithTags, IObjectWithGuidForId
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonConverter(typeof(JsonStringEnumConverter))] // JSON.Net
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

        public override string ToString()
        {
            if (AddressLine2 != null)
            {
                return $"{AddressLine1}\n{AddressLine2}\n{City}, {StateProvince} {PostalCode}";
            }

            return $"{AddressLine1}\n{City}, {StateProvince} {PostalCode}";
        }

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

        public Address(iMetalCompanyAddressModel model, AddressType type)
        {
            var newAddress = new Address()
            {
                Id = Guid.NewGuid(),
                Country = model.CountryName,
                AddressLine1 = model.Address.Replace("\n", string.Empty),
                City = model.Town,
                StateProvince = model.County,
                County = model.County,
                PostalCode = model.PostCode,
                Type = type,
                AddressLine2 = String.Empty,
                ExternalCode = model.Code,
                ExternalStatus = model.Status,
                ExternalSqlId = model.SqlId,
                ExternalExists = true
            };

            Id = Guid.NewGuid();
            Type = type;
            AddressLine1 = model.Address.Replace("\n", " ");
            City = model.Town;
            StateProvince = model.County;
            PostalCode = model.PostCode;
            Country = model.CountryName;
            ExternalCode = model.Code;
            ExternalStatus = model.Status;
            ExternalSqlId = model.SqlId;
            ExternalExists = true;
            Name = model.Name;
        }

    }
}