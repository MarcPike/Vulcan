using DAL.HRS.Mongo.DocClass.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Common.Models
{
    public class PhoneNumberModel
    {
        public Guid Id { get; set; }

        public string Type { get; set; } = "Office";

        public string Number { get; set; }
        public List<string> PhoneTypes = Enum.GetNames(typeof(PhoneType)).ToList();

        public List<string> SearchTags { get; set; } = new List<string>();

        public PhoneNumberModel() { Id = Guid.NewGuid(); }

        public PhoneNumberModel(DAL.Common.DocClass.PhoneNumber phoneNumber)
        {
            Id = phoneNumber.Id;
            Type = phoneNumber.Type.ToString();
            Number = phoneNumber.Number;
            SearchTags = phoneNumber.SearchTags;
        }

        public DAL.Common.DocClass.PhoneNumber ToBaseValue()
        {
            return new DAL.Common.DocClass.PhoneNumber()
            {
                Id = Id,
                Number = Number,
                Type = (PhoneType)Enum.Parse(typeof(PhoneType), Type, true),
                SearchTags = SearchTags
            };
        }
    }
}