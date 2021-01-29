using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.Tests.Prepare_Database
{
    [TestFixture()]
    public class AddTestUserToLdapUsers
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void AddTestUser()
        {
            var telgeLocation = new RepositoryBase<Location>().AsQueryable().Single(x=>x.Office == "Telge").AsLocationRef();
            var rep = new RepositoryBase<LdapUser>();

            var testUserFound = rep.AsQueryable()
                .FirstOrDefault(x => x.Person.FirstName == "Hrs" && x.Person.LastName == "TestUser");
            if (testUserFound != null)
            {
                testUserFound.FirstName = "Hrs";
                testUserFound.LastName = "TestUser";
                testUserFound.MiddleName = "";

                rep.Upsert(testUserFound);
                return;
            }

            var testUser = new LdapUser()
            {
                Location = telgeLocation,
                NetworkId = "HrsTestUser",
                UserName = "TestUser, Hrs",
                FirstName = "Hrs",
                LastName = "TestUser",
                MiddleName = "",
                Person = new Person()
                {
                    FirstName = "Hrs",
                    LastName = "TestUser",
                    MiddleName = "",
                    Addresses = new List<Address>()
                    {
                        new Address() { AddressLine1 = "101 Snoop Dog Lane", City = "Rowdy", StateProvince = "Texas", Country = "United States", County = "Harris", Name = "Test House"}
                    },
                    EmailAddresses = new List<EmailAddress>()
                    {
                        new EmailAddress()
                        {
                            Address = "TestUser@howcogroup.com",
                            Type = EmailType.Business
                        }
                    },
                    PhoneNumbers = new List<PhoneNumber>()
                    {
                        new PhoneNumber()
                        {
                            Number = "(123)456-0321"
                        }
                    },
                    
                    
                },

            };

            rep.Upsert(testUser);
        }
    }
}
