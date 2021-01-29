using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Queries;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Queries;
using Vulcan.IMetal.Queries.Companies;

namespace DAL.Vulcan.Mongo.Core.DocClass.Companies
{
    public class Company: BaseDocument
    {
        public static MongoRawQueryHelper<Company> Helper = new MongoRawQueryHelper<Company>();
        public string Code { get; set; }
        public int SqlId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Branch => Location.Branch;
        public Location Location { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public GuidList<Note> Notes { get; set; } = new GuidList<Note>();
        public List<PhoneNumber> PhoneNumbers { get; set; }

        //public Address PrimaryAddress { get; set; }
        public string AddressLine1 { get; set; } = String.Empty;
        public string AddressLine2 { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string County { get; set; } = String.Empty;
        public string StateProvince { get; set; } = String.Empty;
        public string PostalCode { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;

        public string ExternalCustomerType { get; set; }
        public string ExternalSalesGroupName { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ImportedOn { get; set; } = DateTime.UtcNow;

        public List<CompanyRef> Suppliers { get; set; } = new List<CompanyRef>();
        public List<CompanyRef> SupplierFor { get; set; } = new List<CompanyRef>();
        public bool IsAlliance { get; set; } = false;

        public ReferenceList<Team, TeamRef> Teams => new ReferenceList<Team, TeamRef>();
        public ReferenceList<Contact, ContactRef> Contacts { get; set; } = new ReferenceList<Contact, ContactRef>();

        public List<CompanyUpdateHistory> CompanyUpdates { get; set; } = new List<CompanyUpdateHistory>();

        public string DefaultSalesGroupCode { get; set; } = string.Empty;
        public string DefaultCurrencyCode { get; set; } = string.Empty;

        public string OrderClassificationCode { get; set; }
        public string OrderClassificationDescription { get; set; }
        public bool IsActive { get; set; } = true;

        public Company Save()
        {
            SaveToDatabase();
            return this;
        }
        public override string ToString()
        {
            return $"{Code} - {Name}";
        }

        public CompanyRef AsCompanyRef()
        {
            return new CompanyRef(this);
        }

        public void LoadIsActiveFromIMetal()
        {
            var companySearchResult = CompanyQuery.GetForId(Location.GetCoid(),SqlId).Result;
            if (companySearchResult == null) throw new Exception("Company no longer exists in iMetal");

            var isActive  = companySearchResult.CompanyStatusCode == "A";

            if (isActive != IsActive)
            {
                IsActive = isActive;
                SaveToDatabase();
            }


        }

        public void LoadOrderClassificationFromIMetal()
        {
            var companySearchResult = CompanyQuery.GetForId(Location.GetCoid(), SqlId).Result;
            if (companySearchResult == null) throw new Exception("Company no longer exists in iMetal");

            OrderClassificationCode = companySearchResult.OrderClassificationCode ?? "<unspecified>" ;
            OrderClassificationDescription = companySearchResult.OrderClassificationDescription ?? "<unspecified>";

            SaveToDatabase();

        }

        public void LoadContactsFromIMetal()
        {
            var companySearchResult = CompanyQuery.GetForId(Location.GetCoid(), SqlId).Result;
            var externalContacts =companySearchResult.GetContacts().Result;


            var crmContacts = Contacts.Select(x => x.ToBaseDocument()).ToList();
            
            foreach (var contact in externalContacts.ToList())
            {
                if (crmContacts.All(x => x.SqlId != contact.SqlId))
                {
                    var newContact = new Contact(contact, this);
                    if (newContact.Person.EmailAddresses.Count == 0)
                        continue;
                    newContact.SaveToDatabase();
                    Contacts.AddReferenceObject(newContact.AsContactRef());
                    SaveToDatabase();
                }
            }

        }

        public List<Address> GetAllAddressesFromIMetal()
        {
            var companySearchResult = CompanyQuery.GetForId(Location.GetCoid(), SqlId).Result;
            if (companySearchResult == null) throw new Exception("Company no longer exists in iMetal");

            return companySearchResult.GetAllAddresses().Result.Select(x => new Address(x, AddressType.Shipping)).ToList(); ;
        }
    }
}
