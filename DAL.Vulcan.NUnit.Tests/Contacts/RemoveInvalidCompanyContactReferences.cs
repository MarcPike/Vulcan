using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Contacts
{
    [TestFixture()]
    public class RemoveInvalidCompanyContactReferences
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
        }

        [Test]
        public void Execute()
        {
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();
            var repContact = new RepositoryBase<Contact>();
            var companiesWithContact = repCompany.AsQueryable().Where(x => x.Contacts.Any()).ToList();
            foreach (var company in companiesWithContact)
            {
                foreach (var contactRef in company.Contacts)
                {
                    if (repContact.Find(contactRef.Id) == null)
                    {
                        Console.WriteLine($"Remove {contactRef.GetFullName()} from {company.ShortName}");
                    }
                }
            }
        }
    }
}
