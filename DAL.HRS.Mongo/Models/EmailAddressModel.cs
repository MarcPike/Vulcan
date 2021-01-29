using System;
using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Ldap;

namespace DAL.HRS.Mongo.Models
{
    public class EmailAddressModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = "Office";
        public string Address { get; set; }
        public List<string> SearchTags { get; set; } = new List<string>();


        public EmailAddressModel()
        {
            Id = Guid.NewGuid();
        }

        public EmailAddressModel(EmailAddress emailAddress)
        {
            Id = emailAddress.Id;
            Type = emailAddress.Type.ToString();
            Address = emailAddress.Address;
            SearchTags = emailAddress.SearchTags;
        }


        public EmailAddress ToBaseValue()
        {
            return new EmailAddress
            {
                Id = Id,
                Address = Address,
                Type = (EmailType) Enum.Parse(typeof(EmailType), Type, true),
                SearchTags = SearchTags
            };
        }
    }
}