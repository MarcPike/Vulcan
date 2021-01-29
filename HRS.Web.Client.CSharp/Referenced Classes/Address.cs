using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class Address 
    {
        public Guid Id { get; set; } 
        public AddressType Type { get; set; } 
        public string AddressLine1 { get; set; } 
        public string AddressLine2 { get; set; } 
        public string City { get; set; } 
        public string County { get; set; } 
        public string StateProvince { get; set; } 
        public string PostalCode { get; set; } 
        public string Country { get; set; } 
        public List<string> SearchTags { get; set; } 
        public string Name { get; set; } 
        public int ExternalSqlId { get; set; } 
        public string ExternalCode { get; set; } 
        public string ExternalStatus { get; set; } 
        public bool ExternalExists { get; set; } 


    }
}