using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Test;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    [TestFixture]
    public class CompanyFind
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        public Mongo.DocClass.Companies.Company FindJebco()
        {
            var rep = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var company = rep.AsQueryable().Single(x => x.Name.Contains("Jebco"));

            return company;
        }

        [Test]
        public Mongo.DocClass.Companies.Company FindCode()
        {
            var rep = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var company = rep.AsQueryable().Single(x => x.Code.Equals("00208"));

            return company;
        }


        [Test]
        public void CanFindCompany()
        {
            var company = FindJebco();
            Assert.IsNotNull(company);

            foreach (var companyAddress in company.Addresses)
            {
                Console.WriteLine(ObjectDumper.Dump(companyAddress));
            }

        }

    }
}
 