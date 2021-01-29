using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Contacts
{
    [TestFixture]
    public class CompanyContactTreeTesting
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void GetTestFirstCompanyWithNoContacts()
        {
            var testCompany = Mongo.DocClass.Companies.Company.Helper.Find(x => x.Location.Office == "Telge" && !x.Contacts.Any()).FirstOrDefault();
            Console.WriteLine($"Code: {testCompany.Code} Name: {testCompany.Name} Id: {testCompany.Id}");
        }

        [Test]
        public void AddContactsToTestCompany()
        {
            // Code: 02585 Name: Varel International, L.P. Id: 593ee5ccb508d7372cf9cd4e
            var company = Mongo.DocClass.Companies.Company.Helper.FindById("593ee5ccb508d7372cf9cd4e");

            var crmUsers = new List<Mongo.DocClass.CRM.CrmUser>();
            crmUsers.Add(Mongo.DocClass.CRM.CrmUser.Helper.Find(x=>x.User.LastName == "Pike").FirstOrDefault());
            crmUsers.Add(Mongo.DocClass.CRM.CrmUser.Helper.Find(x => x.User.LastName == "Gallegos").FirstOrDefault());

            var crmUserRefs = crmUsers.Select(x => x.AsCrmUserRef()).ToList();

            // David Preston
            var preston = new Contact()
            {
                Person = new Person()
                {
                    LastName = "Preston",
                    FirstName = "David"
                },
                Position = "President",
            };
            preston.CrmUsers.AddListOfReferenceObjects(crmUserRefs);
            preston.Companies.Add(company.AsCompanyRef());
            Contact.Helper.Upsert(preston);
            foreach (var crmUser in crmUsers)
            {
                crmUser.Contacts.Add(preston.AsContactRef());
                Mongo.DocClass.CRM.CrmUser.Helper.Upsert(crmUser);
            }

            // Stanton Fraser
            var fraser = new Contact()
            {
                Person = new Person()
                {
                    LastName = "Fraser",
                    FirstName = "Stanton"
                },
                Position = "Vice-President",
                ReportsTo = preston.AsContactRef()
            };
            fraser.CrmUsers.AddListOfReferenceObjects(crmUserRefs);
            fraser.Companies.Add(company.AsCompanyRef());
            Contact.Helper.Upsert(fraser);
            foreach (var crmUser in crmUsers)
            {
                crmUser.Contacts.Add(fraser.AsContactRef());
                Mongo.DocClass.CRM.CrmUser.Helper.Upsert(crmUser);
            }

            // Marc Pike
            var pike = new Contact()
            {
                Person = new Person()
                {
                    LastName = "Pike",
                    FirstName = "Marc"
                },
                Position = "Sr. Developer",
                ReportsTo = fraser.AsContactRef()
            };
            pike.CrmUsers.AddListOfReferenceObjects(crmUserRefs);
            pike.Companies.Add(company.AsCompanyRef());
            Contact.Helper.Upsert(pike);
            foreach (var crmUser in crmUsers)
            {
                crmUser.Contacts.Add(pike.AsContactRef());
                Mongo.DocClass.CRM.CrmUser.Helper.Upsert(crmUser);
            }

            // Marc Pike
            var gallegos = new Contact()
            {
                Person = new Person()
                {
                    LastName = "Gallegos",
                    FirstName = "Isidro"
                },
                Position = "Sr. Developer",
                ReportsTo = fraser.AsContactRef()
            };
            gallegos.CrmUsers.AddListOfReferenceObjects(crmUserRefs);
            gallegos.Companies.Add(company.AsCompanyRef());
            Contact.Helper.Upsert(gallegos);
            foreach (var crmUser in crmUsers)
            {
                crmUser.Contacts.Add(gallegos.AsContactRef());
                Mongo.DocClass.CRM.CrmUser.Helper.Upsert(crmUser);
            }

            company.Contacts.Add(preston.AsContactRef());
            company.Contacts.Add(fraser.AsContactRef());
            company.Contacts.Add(pike.AsContactRef());
            company.Contacts.Add(gallegos.AsContactRef());
            Mongo.DocClass.Companies.Company.Helper.Upsert(company);
        }

        [Test]
        public void GetCompanyTree()
        {
            var company = Mongo.DocClass.Companies.Company.Helper.FindById("593ee5ccb508d7372cf9cd4e");
            var companyTree = new CompanyContactTree(company.AsCompanyRef());
            Console.WriteLine(ObjectDumper.Dump(companyTree));
        }

    }
}
