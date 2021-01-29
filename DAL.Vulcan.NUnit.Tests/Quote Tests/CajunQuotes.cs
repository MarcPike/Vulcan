using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.Quote_Tests
{
    [TestFixture]
    public class CajunQuotes
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var sinceDate = new DateTime(2019,1,1);
            var count = new RepositoryBase<CrmQuote>().AsQueryable()
                .Count(x => x.Team.Name == "Louisiana Ragin Cajuns" && x.ReportDate >= sinceDate && x.Status == PipelineStatus.Won);
            Console.WriteLine(count);
        }

        [Test]
        public void FindRegretQuote()
        {
            var quoteItem = CrmQuoteItem.Helper.Find(x => x.QuickQuoteData != null && x.QuickQuoteData.Regret)
                .FirstOrDefault();

            Console.WriteLine(quoteItem.GetQuote().QuoteId);
        }
    }
}
