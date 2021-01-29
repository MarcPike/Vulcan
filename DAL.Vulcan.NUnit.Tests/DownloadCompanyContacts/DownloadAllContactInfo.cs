using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.DownloadCompanyContacts
{
    [TestFixture]
    public class DownloadAllContactInfo
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var contacts = Contact.Helper.GetAll();
            foreach (var contact in contacts)
            {
                if (contact.Person.EmailAddresses.Any())
                {
                    foreach (var contactCompany in contact.Companies)
                    {
                        Console.WriteLine($"{contact.Person.FirstName}\t{contact.Person.LastName}\t{contactCompany.Code}\t{contactCompany.Name}\t{contact.Person.EmailAddresses.LastOrDefault(x=>x.Type == EmailType.Business)?.Address}\t{contact.Person.EmailAddresses.LastOrDefault(x => x.Type == EmailType.Personal)?.Address}");
                    }
                }
            }
        }

    }
}
