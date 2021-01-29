using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;

namespace DAL.Common.Models
{
    public class PersonModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public List<PhoneNumberModel> PhoneNumbers { get; set; } = new List<PhoneNumberModel>();
        public List<AddressModel> Addresses { get; set; } = new List<AddressModel>();
        public List<EmailAddressModel> EmailAddresses { get; set; } = new List<EmailAddressModel>();

        public PersonModel()
        {
        }

        public PersonModel(Person person)
        {
            FirstName = person.FirstName;
            LastName = person.LastName;
            MiddleName = person.MiddleName;
            PhoneNumbers = person.PhoneNumbers.Select(x=> new PhoneNumberModel(x)).ToList();
            Addresses = person.Addresses.Select(x => new AddressModel(x)).ToList();
            EmailAddresses = person.EmailAddresses.Select(x => new EmailAddressModel(x)).ToList();
            //Notes = person.Notes;
        }

        public PersonModel(LdapUser user)
        {
            LastName = user.LastName;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            PhoneNumbers = user.Person.PhoneNumbers.Select(x => new PhoneNumberModel(x)).ToList();
            Addresses = user.Person.Addresses.Select(x => new AddressModel(x)).ToList();
            EmailAddresses = user.Person.EmailAddresses.Select(x => new EmailAddressModel(x)).ToList();
        }
    }
}
