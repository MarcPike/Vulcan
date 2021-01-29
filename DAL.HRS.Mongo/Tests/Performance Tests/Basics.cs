using System;
using System.Diagnostics;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Driver;
using NUnit.Framework;
using LdapUser = DAL.Common.DocClass.LdapUser;

namespace DAL.HRS.Mongo.Tests.Performance_Tests
{

    [TestFixture()]
    public class Basics
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void LdapUserFind()
        {
            var rep = new RepositoryBase<LdapUser>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var ldapUser = rep.AsQueryable()
                .FirstOrDefault(x => x.Person.FirstName == "Marc" && x.Person.LastName == "Pike");
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            sw.Start();

            var builder = Builders<LdapUser>.Filter;
            var filter = builder.Eq(x => x.Person.FirstName , "Marc") & builder.Eq(x => x.Person.LastName ,"Pike");

            var ldapUsers = rep.FindAll(filter);
            foreach (var user in ldapUsers)
            {
                Console.WriteLine(user.Id);
            }

            //ldapUser = rep.FindAll(filter).FirstOrDefault();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

        }



    }

}
