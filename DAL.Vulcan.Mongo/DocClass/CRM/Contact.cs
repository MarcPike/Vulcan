using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Email;
using DAL.Vulcan.Mongo.DocClass.Locations;
using Vulcan.IMetal.Context.Company;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
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
        public ReferenceList<Email.Email, EmailRef> Emails { get; set; } = new ReferenceList<Email.Email, EmailRef>();
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

        public Contact(global::Vulcan.IMetal.Context.Company.Contact contact, Companies.Company company)
        {
            SqlId = contact.Id;
            Companies.AddReferenceObject(company.AsCompanyRef());

            var phoneNumbers = new List<PhoneNumber>();
            if (!String.IsNullOrEmpty(contact.Mobile))
            {
                phoneNumbers.Add(new PhoneNumber() { Number = contact.Mobile, Type = PhoneType.Mobile });
            }

            if (!String.IsNullOrEmpty(contact.Fax))
            {
                phoneNumbers.Add(new PhoneNumber() { Number = contact.Fax, Type = PhoneType.Fax });
            }

            if (!String.IsNullOrEmpty(contact.FastDial))
            {
                phoneNumbers.Add(new PhoneNumber() { Number = contact.FastDial, Type = PhoneType.Office });
            }

            var emailAddresses = new List<EmailAddress>();

            if (!String.IsNullOrEmpty(contact.Email))
            {
                emailAddresses.Add(new EmailAddress() { Address = contact.Email, Type = EmailType.Business });
            }

            var addresses = new List<Locations.Address>();

            var address = new DAL.Vulcan.Mongo.DocClass.Locations.Address();
            if (contact.Address != null)
            {
                address.Type = AddressType.Primary;
                address.AddressLine1 = contact.Address.Address1;
                address.StateProvince = contact.Address.County;
                address.PostalCode = contact.Address.Postcode;
                address.City = contact.Address.Town;

                addresses.Add(address);
            }

            Person = new Person()
            {
                FirstName = contact.Forename,
                LastName = contact.Surname,
                EmailAddresses = emailAddresses,
                PhoneNumbers = phoneNumbers,
                Addresses = addresses
            };
        }


    }


}