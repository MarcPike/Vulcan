using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Test;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.CompanyDefaults
{
    [TestFixture()]
    public class CompanyDefaultsTesting
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void TestCompanyDefaults()
        {
            var company = new RepositoryBase<Mongo.DocClass.Companies.Company>()
                .AsQueryable().FirstOrDefault(x => x.Location.Branch == "EUR" && x.Code == "02071");
                

            var companyDefaults = Mongo.DocClass.Companies.CompanyDefaults.GetCompanyDefaults("EUR", company, false);
            Console.WriteLine(ObjectDumper.Dump(companyDefaults));
        }

        [Test]
        public void GetAcDefaults()
        {
            var companyDefaults = Mongo.DocClass.Companies.CompanyDefaults.Helper
                .Find(x => x.Coid == "INC" && x.SalesGroupCode == "AC").ToList();

            var quoteCount = 0;
            foreach (var companyDefault in companyDefaults)
            {
                var company = Mongo.DocClass.Companies.Company.Helper.FindById(companyDefault.CompanyId);
                quoteCount += CrmQuote.Helper
                    .Find(x => x.SalesGroupCode == "AC" && x.Company.Id == company.Id.ToString()).ToList().Count();
                Console.WriteLine($"{company.Code} - {company.Name}");
            }
            Console.WriteLine($"Total Quotes wrong: {quoteCount}");
        }

    }
}
