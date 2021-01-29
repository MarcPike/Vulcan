using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Ldap;

namespace DAL.HRS.Mongo.Models
{
    public class PersonModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public List<PhoneNumberModel> PhoneNumbers { get; set; } 
        public List<AddressModel> Addresses { get; set; } 
        public List<EmailAddressModel> EmailAddresses { get; set; } 

        public PersonModelBase(Person person)
        {
            FirstName = person.FirstName;
            LastName = person.LastName;
            MiddleName = person.MiddleName;
            PhoneNumbers = person.PhoneNumbers.Select(x=> new PhoneNumberModel(x)).ToList();
            Addresses = person.Addresses.Select(x => new AddressModel(x)).ToList();
            EmailAddresses = person.EmailAddresses.Select(x => new EmailAddressModel(x)).ToList();
            //Notes = person.Notes;
        }
    }
}
