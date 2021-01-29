using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    [BsonIgnoreExtraElements]
    public class ContactRef: ReferenceObject<Contact>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string FullName
        {
            get
            {
                var result = string.Empty;
                if (FirstName != null) result += FirstName;
                if (MiddleName != null) result += " " + MiddleName;
                if (LastName != null) result += " " + LastName;
                result = result.Trim();
                return result;
            }
        }

        public List<EmailAddress> EmailAddresses { get; set; } = new List<EmailAddress>();
        public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();

        public override string ToString()
        {
            return GetFullName();
        }

        public string GetFullName()
        {
            if (MiddleName == string.Empty)
            {
                return $"{FirstName} {LastName}";
            }
            else
            {
                return $"{FirstName} {MiddleName} {LastName}";
            }
        }

    public ContactRef()
        {
            
        }

        public Contact AsContact()
        {
            return ToBaseDocument();
        }


        public ContactRef(Contact document) : base(document)
        {
            FirstName = document.Person.FirstName;
            LastName = document.Person.LastName;
            MiddleName = document.Person.MiddleName;
            EmailAddresses = document.Person.EmailAddresses;
            PhoneNumbers = document.Person.PhoneNumbers;
        }
    }
}