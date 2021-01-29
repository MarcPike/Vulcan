using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.ContactTreeCleanup
{
    [TestFixture]
    class CleanupDeadContactsInProduction
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void IdentityTreeDataInvalid()
        {
            var contactsWithReportsTo = Contact.Helper.Find(x => x.ReportsTo != null).ToList();
            foreach (var contact in contactsWithReportsTo)
            {
                var reportsTo = contact.ReportsTo.AsContact();
                if (reportsTo == null)
                {
                    Console.WriteLine($"{contact.Person.FirstName} {contact.Person.LastName} does not report to {contact.ReportsTo.FullName}");
                }
                
            }
        }

    }
}
