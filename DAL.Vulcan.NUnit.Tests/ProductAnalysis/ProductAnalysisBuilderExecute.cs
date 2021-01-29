using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Analysis;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.ProductAnalysis
{
    [TestFixture]
    public class ProductAnalysisBuilderExecute
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void Execute()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ProductWinLossAnalysisBuilder.AddAllQuotes();
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");

            // Ran in Production overnight 2/23/2020
            // Elapsed: 07:35:57.3799871
        }

        [Test]
        public void AddQuote()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var quote = CrmQuote.Helper.Find(x => x.QuoteId == 16798).Single();


            ProductWinLossAnalysisBuilder.AddQuote(quote);
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");

            // Ran in Production overnight 2/23/2020
            // Elapsed: 07:35:57.3799871
        }

        [Test]
        public void AddTeam()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var ukSales = Team.Helper.Find(x => x.Name == "UK Sales Team").First().AsTeamRef();

            var quotes = CrmQuote.Helper.Find(x => x.Team.Id == ukSales.Id).ToList();

            foreach (var crmQuote in quotes)
            {
                ProductWinLossAnalysisBuilder.AddQuote(crmQuote);
            }

            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");

            // Ran in Production overnight 2/23/2020
            // Elapsed: 07:35:57.3799871
        }


        [Test]
        public void GetProductAnalysisModel()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var ukSales = Team.Helper.Find(x => x.Name == "UK Sales Team").First().AsTeamRef();
            var productCode = "4130M 8-5.25 C110";
            var productAnalysisQueryModel = new ProductAnalysisQueryModel()
            {
                ProductCode = productCode,
                Teams = new List<TeamRef>() { ukSales },
                DisplayCurrency = "GBP"
            };

            var productAnalysisResultModel = new ProductAnalysisResultModel(productAnalysisQueryModel);
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");
            Console.WriteLine(ObjectDumper.Dump(productAnalysisResultModel.Days7.Submitted));
        }

        [Test]
        public void GetIncomingAnalysisSummaryModel()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var productCode = "718 0.686 205PSQ";
            var incomingAnalysisQueryModel = new IncomingAnalysisQueryModel()
            {
                ProductCode = productCode,
                CoidList = new List<string>(),
                DisplayCurrency = "USD",
            };

            var model = new IncomingAnalysisResultModel(incomingAnalysisQueryModel);
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");
            Console.WriteLine(ObjectDumper.Dump(model));
        }

    }
}
