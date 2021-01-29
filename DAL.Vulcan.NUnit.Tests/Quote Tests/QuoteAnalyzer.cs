using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Test;
using MongoDB.Bson;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Quote_Tests
{
    [TestFixture]
    public class QuoteAnalyzer
    {
        [Test]
        public void FindMissingQuote()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var quote = new RepositoryBase<CrmQuote>().AsQueryable().FirstOrDefault(x=> x.QuoteId == 11680);
            Console.WriteLine(quote.ReportDate);
            quote.SetReportDate();
            Console.WriteLine(quote.ReportDate);
            quote.SaveToDatabase();
        }

        [Test]
        public void QuoteDump()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var quote = new RepositoryBase<CrmQuote>().AsQueryable().First(x => x.QuoteId == 10480);
            //Console.WriteLine(ObjectDumper.Dump(quote));
            var badItemRef = quote.Items[2];

            quote.Status = PipelineStatus.Draft;

            var badItem = quote.Items[2];
            quote.Items.Remove(badItem);
            quote.SaveToDatabase();
        }

        [Test]
        public void QuoteItemDump()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var quote = new RepositoryBase<CrmQuote>().AsQueryable().FirstOrDefault(x => x.QuoteId == 8807);
            foreach (var crmQuoteItem in quote.Items.Select(x => x.AsQuoteItem()).OrderBy(x => x.Index).ToList())
            {
                Console.WriteLine(ObjectDumper.Dump(crmQuoteItem));
            }
        }

        [Test]
        public void QuotesWithMissingItems()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var quotes = new RepositoryBase<CrmQuote>().AsQueryable().ToList();

            foreach (var quote in quotes)
            {
                foreach (var itemRef in quote.Items)
                {
                    var item = itemRef.AsQuoteItem();
                    if (item == null)
                    {
                        Console.WriteLine($"QuoteId: {quote.QuoteId} Item Index: {itemRef.Index} is null");
                        Console.WriteLine(ObjectDumper.Dump(itemRef));
                        quote.Items.Remove(itemRef);
                        quote.SaveToDatabase();
                    }
                }
            }
        }

        [Test]
        public void CheckQuotesWithCompany()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var rep = new RepositoryBase<CrmQuote>();
            var quotes = rep.AsQueryable().Where(x => x.Company.Id == "593ee5cfb508d7372cf9d369").ToList();
            foreach (var crmQuote in quotes)
            {
                Console.WriteLine(ObjectDumper.Dump(crmQuote));
            }
        }

        [Test]
        public void QuoteMiniModelTest()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var quote = new RepositoryBase<CrmQuote>().AsQueryable().FirstOrDefault(x => x.QuoteId == 14000);
            var miniModel = new QuoteMiniModel(quote, PipelineStatus.Won);
            Console.WriteLine(ObjectDumper.Dump(miniModel));
        }

        [Test]
        public void TestQuotePipeline()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var helperQuotes = new HelperQuote();
            var quotes = new List<CrmQuote>();
            var problemQuote = new RepositoryBase<CrmQuote>().AsQueryable().FirstOrDefault(x => x.QuoteId == 7373);
            quotes.Add(problemQuote);
            var pipeline = new QuotePipelineModel("vulcancrm", "5a660b24b508d745a088acf0", quotes.ToList());
            Console.WriteLine(ObjectDumper.Dump(pipeline));
        }

        [Test]
        public void QuoteCount()
        {
            var today = DateTime.Now.Date;
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            Console.WriteLine($"Houston Quotes entered today: {new RepositoryBase<CrmQuote>().AsQueryable().Count(x => x.CreateDateTime >= today && x.Team.Name == "Houston Sales")}");
            Console.WriteLine($"Louisiana Quotes entered today: {new RepositoryBase<CrmQuote>().AsQueryable().Count(x => x.CreateDateTime >= today && x.Team.Name == "Louisiana Ragin Cajuns")}");
        }

        [Test]
        public void QuoteCompanyCount()
        {
            var today = DateTime.Now.Date;
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            Console.WriteLine($"Unique Companies: {new RepositoryBase<CrmQuote>().AsQueryable().Select(x=>x.Company.Id).Distinct().ToList().Count()}");
        }

        [Test]
        public void QuoteTotalCount()
        {
            var today = DateTime.Now.Date;
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            Console.WriteLine($"Unique Companies: {new RepositoryBase<CrmQuote>().AsQueryable().Where(x => x.Status == PipelineStatus.Won || x.Status == PipelineStatus.Submitted).ToList().Count()}");
        }

    }
}
