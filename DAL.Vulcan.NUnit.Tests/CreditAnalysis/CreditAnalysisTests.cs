using System;
using System.Diagnostics;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.CreditAnalysis
{
    [TestFixture]
    public class CreditAnalysisTests
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void GetCompanyCreditAnalysisModel()
        {

            var quote = CrmQuote.Helper.Find(x => x.QuoteId == 16702).FirstOrDefault();


            var companyId = Mongo.DocClass.Companies.Company.Helper.
                Find(x => x.Location.Branch == "EUR" && x.Code == "04908")
                .First().Id.ToString();

            //var companyId = "593ee5cfb508d7372cf9d2fb";

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var model = new CompanyCreditAnalysisModel(quote.Company.Id, "USD");
            sw.Stop();
            Console.WriteLine($"Query Elapsed Time: {sw.Elapsed}");
            Console.WriteLine(ObjectDumper.Dump(model));
        }

    }
}
