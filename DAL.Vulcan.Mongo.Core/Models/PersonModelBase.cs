using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Models
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
