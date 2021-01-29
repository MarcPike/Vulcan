using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Ldap;

namespace DAL.HRS.Mongo.Models
{
    public class PhoneNumberModel
    {
        public Guid Id { get; set; }

        public string Type { get; set; } = "Office";

        public string Number { get; set; }
        public List<string> PhoneTypes = Enum.GetNames(typeof(PhoneType)).ToList();

        public List<string> SearchTags { get; set; } = new List<string>();

        public PhoneNumberModel() { Id = Guid.NewGuid(); }

        public PhoneNumberModel(PhoneNumber phoneNumber)
        {
            Id = phoneNumber.Id;
            Type = phoneNumber.Type.ToString();
            Number = phoneNumber.Number;
            SearchTags = phoneNumber.SearchTags;
        }

        public PhoneNumber ToBaseValue()
        {
            return new PhoneNumber()
            {
                Id = Id,
                Number = Number,
                Type = (PhoneType)Enum.Parse(typeof(PhoneType), Type, true),
                SearchTags = SearchTags
            };
        }
    }
}