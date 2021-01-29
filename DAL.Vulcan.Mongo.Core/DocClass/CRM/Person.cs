using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using DAL.Vulcan.Mongo.Core.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class Person : ObjectWithTags, ISupportInitialize
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
        public List<Address> Addresses { get; set; } = new List<Address>();
        public List<EmailAddress> EmailAddresses { get; set; } = new List<EmailAddress>();
        //public GuidList<Note> Notes { get; set; } = new GuidList<Note>();

        public void UpdateFromModel(PersonModelBase model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            MiddleName = model.MiddleName;
            PhoneNumbers = model.PhoneNumbers.Select(x=>x.ToBaseValue()).ToList();
            Addresses = model.Addresses.Select(x => x.ToBaseValue()).ToList();
            EmailAddresses = model.EmailAddresses.Select(x => x.ToBaseValue()).ToList();
        }

        public Person()
        {
            
        }

        public Person(LdapUser user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
        }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
        }
    }
}
