using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    [TestFixture]
    public class LoadContacts
    {
        [Test]
        public void Execute()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var companies = new RepositoryBase<Mongo.DocClass.Companies.Company>().AsQueryable().ToList();
            foreach (var company in companies)
            {
                company.LoadContactsFromIMetal();
            }
        }

        [Test]
        public void Check()
        {
            var companiesWithContacts = new RepositoryBase<Mongo.DocClass.Companies.Company>().AsQueryable().Where(x=>x.Contacts.Count > 0).ToList();
            foreach (var companiesWithContact in companiesWithContacts.OrderBy(X=>X.Name))
            {
                Console.WriteLine($"Company: {companiesWithContact.Name} Contacts: {companiesWithContact.Contacts.Count}");
            }
        }
    }
}
