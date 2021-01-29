using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Queries;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using DAL.iMetal.Core.Models;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    [BsonIgnoreExtraElements]
    public class Contact: BaseDocument
    {
        public static MongoRawQueryHelper<Contact> Helper = new MongoRawQueryHelper<Contact>();

        public int SqlId { get; set; }
        public Person Person { get; set; } = new Person();
        public GuidList<Note> Notes { get; set; } = new GuidList<Note>();
        public ReferenceList<Companies.Company, CompanyRef> Companies { get; set; } = new ReferenceList<Companies.Company, CompanyRef>();
        public ReferenceList<Prospect, ProspectRef> Prospects { get; set; } = new ReferenceList<Prospect, ProspectRef>();
        public ReferenceList<CrmUser, CrmUserRef> CrmUsers { get; set; } = new ReferenceList<CrmUser, CrmUserRef>();
        public ReferenceList<Action,ActionRef> Actions { get; set; } = new ReferenceList<Action, ActionRef>();
        public string Position { get; set; } = string.Empty;
        public ContactRef ReportsTo { get; set; }
        public override string ToString()
        {
            if (Person.MiddleName == string.Empty)
            {
                return $"{Person.FirstName} {Person.LastName}";
            }
            else
            {
                return $"{Person.FirstName} {Person.MiddleName} {Person.LastName}";
            }
        }

        public string FullName => this.ToString();

        public ContactRef AsContactRef()
        {
            return new ContactRef(this);
        }

        public Contact()
        {
        }

        public Contact(iMetalContactModel contact, Companies.Company company)
        {
            SqlId = contact.SqlId;
            Companies.AddReferenceObject(company.AsCompanyRef());

            var phoneNumbers = new List<PhoneNumber>();
            if (!String.IsNullOrEmpty(contact.PhoneMobile))
            {
                phoneNumbers.Add(new PhoneNumber() { Number = contact.PhoneMobile, Type = PhoneType.Mobile });
            }

            if (!String.IsNullOrEmpty(contact.PhoneFax))
            {
                phoneNumbers.Add(new PhoneNumber() { Number = contact.PhoneFax, Type = PhoneType.Fax });
            }

            if (!String.IsNullOrEmpty(contact.PhoneOffice))
            {
                phoneNumbers.Add(new PhoneNumber() { Number = contact.PhoneOffice, Type = PhoneType.Office });
            }

            var emailAddresses = new List<EmailAddress>();

            if (!String.IsNullOrEmpty(contact.EmailBusiness))
            {
                emailAddresses.Add(new EmailAddress() { Address = contact.EmailBusiness, Type = EmailType.Business });
            }

            var addresses = new List<Locations.Address>();

            var address = new DAL.Vulcan.Mongo.Core.DocClass.Locations.Address();
                address.Type = AddressType.Primary;
                address.AddressLine1 = contact.Address1;
                address.StateProvince = contact.StateProvince;
                address.PostalCode = contact.PostalCode;
                address.City = contact.City;
                
                addresses.Add(address);
            

            Person = new Person()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                EmailAddresses = emailAddresses,
                PhoneNumbers = phoneNumbers,
                Addresses = addresses
            };
        }


    }


}