using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Models
{
    public class ProspectModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        //public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        //public string ShortName { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public LocationRef Location { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public GuidList<Note> Notes { get; set; } = new GuidList<Note>();
        public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
        public List<ContactRef> Contacts { get; set; } = new List<ContactRef>();
        public CompanyRef Company { get; set; }
        public DateTime? AddedToSystem { get; set; }
        public List<string> SearchTags { get; set; } = new List<string>();
        public bool HasBeenSaved { get; set; } = false;

        public ProspectModel()
        {
            
        }

        public ProspectModel(string application, string userId, LocationRef location)
        {
            Application = application;
            UserId = userId;
            Location = location;
            Branch = location.AsLocation().Branch;
        }

        public ProspectModel(Prospect prospect, string application, string userId)
        {
            Application = application;
            UserId = userId;
            Id = prospect.Id.ToString();
            Name = prospect.Name;
            //ShortName = prospect.ShortName;
            //Code = prospect.Code;
            Location = prospect.Location;
            Branch = prospect.Branch;
            Addresses = prospect.Addresses;
            Notes = prospect.Notes;
            PhoneNumbers = prospect.PhoneNumbers;
            Contacts = prospect.Contacts;
            Company = prospect.Company;
            AddedToSystem = prospect.AddedToSystem;
            SearchTags = prospect.SearchTags;
            HasBeenSaved = true;
        }
}
}
