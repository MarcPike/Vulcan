using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.Models
{
    public class AddressModel 
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Type { get; set; } = "Office";

        public string AddressLine1 { get; set; } = String.Empty;
        public string AddressLine2 { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string County { get; set; } = String.Empty;
        public string StateProvince { get; set; } = String.Empty;
        public string PostalCode { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;

        public List<string> AddressTypes = Enum.GetNames(typeof(AddressType)).ToList();
        public List<string> SearchTags { get; set; } = new List<string>();
        public AddressModel()
        {
            
        }

        public AddressModel(Address address) 
        {
            Id = address.Id;
            Type = address.Type.ToString();
            AddressLine1 = address.AddressLine1;
            AddressLine2 = address.AddressLine2;
            City = address.City;
            County = address.County;
            StateProvince = address.StateProvince;
            PostalCode = address.PostalCode;
            Country = address.Country;
            SearchTags = address.SearchTags;
        }

        public Address ToBaseValue()
        {
            return new Address()
            {
                Id = Id,
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                Type = (AddressType) Enum.Parse(typeof(AddressType), Type, true),
                City = City,
                StateProvince = StateProvince,
                PostalCode = PostalCode,
                County = County,
                Country = Country,
                SearchTags = SearchTags
            };
        }
    }
}