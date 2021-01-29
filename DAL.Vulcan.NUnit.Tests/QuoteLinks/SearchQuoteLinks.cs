using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Driver;
using NUnit.Framework;
using DAL.Vulcan.Mongo.DocClass.Companies;

namespace DAL.Vulcan.NUnit.Tests.QuoteLinks
{
    [TestFixture]
    public class SearchQuoteLinks
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void LookAtQuote()
        {
            var source = CrmQuote.Helper.Find(x => x.QuoteId == 109659).First();
            Console.WriteLine($"QuoteId: {source.QuoteId} QuoteLinkType: {source.QuoteLinkType} Payment Terms: {source.PaymentTerm}");
            var sourceCompany = source.Company.AsCompany();
            Console.WriteLine($"Quote Company: {sourceCompany.Code} - {sourceCompany.Name}");
            var sourceCompanyDefaults = DAL.Vulcan.Mongo.DocClass.Companies.CompanyDefaults.GetCompanyDefaults(source.Coid, sourceCompany, false);
            Console.WriteLine($"Source Company Defaults PaymentTerms: [{sourceCompanyDefaults.PaymentTerm}] Source Quote Payment Terms: [{source.PaymentTerm}]");

        }


        [Test]
        public void Execute()
        {
            var source = CrmQuote.Helper.Find(x => x.QuoteId == 110053).First();

            var linkType = source.QuoteLinkType;
            Assert.AreEqual(linkType,QuoteLinkType.Original);

            var linkedQuotes = CrmQuote.Helper
                .Find(x => x.QuoteLinkId == source.QuoteLinkId && x.QuoteId != source.QuoteId).ToList();

            var sourceCompany = source.Company.AsCompany();
            Console.WriteLine($"Source QuoteId: {source.QuoteId}");
            Console.WriteLine($"Source Quote Company: {sourceCompany.Code} - {sourceCompany.Name}");

            var sourceCompanyDefaults = DAL.Vulcan.Mongo.DocClass.Companies.CompanyDefaults.GetCompanyDefaults(source.Coid, sourceCompany, false);
            Console.WriteLine($"Source Company Defaults PaymentTerms: [{sourceCompanyDefaults.PaymentTerm}] Source Quote Payment Terms: [{source.PaymentTerm}]");


            Console.WriteLine("");
            foreach (var linkedQuote in linkedQuotes)
            {
                Console.WriteLine($"Linked QuoteId: {linkedQuote.QuoteId}");
                var company = linkedQuote.Company.AsCompany();
                Console.WriteLine($"Linked Quote Company: {company.Code} - {company.Name}");
                var companyDefaults = DAL.Vulcan.Mongo.DocClass.Companies.CompanyDefaults.GetCompanyDefaults(linkedQuote.Coid, company, false);
                Console.WriteLine($"Company Defaults (Linked Quote Company) PaymentTerms: [{companyDefaults.PaymentTerm}] Linked Quote Payment Terms: [{linkedQuote.PaymentTerm}]");
            }
        }

        [Test]
        public void GetCompanyDefaults()
        {
            var company = Mongo.DocClass.Companies.Company.Helper.Find(x => x.Code == "00011").First();
            var companyDefaults = DAL.Vulcan.Mongo.DocClass.Companies.CompanyDefaults.GetCompanyDefaults("INC", company, false);
            Console.WriteLine(companyDefaults.PaymentTerm);
        }


    }
}
