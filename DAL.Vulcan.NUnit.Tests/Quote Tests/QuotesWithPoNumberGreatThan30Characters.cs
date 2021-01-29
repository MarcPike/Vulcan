using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = System.Environment;

namespace DAL.Vulcan.NUnit.Tests.Quote_Tests
{
    [TestFixture()]
    public class QuotesWithPoNumberGreatThan30Characters
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Mongo.Base.Context.Environment.QualityControl;
            EnvironmentSettings.Database = MongoDatabase.VulcanCrm;
        }

        [Test]
        public void Execute()
        {

            var rep = new RepositoryBase<CrmQuote>();
            var quotes = rep.AsQueryable().Where(x => x.Status == PipelineStatus.Won)
                .Select(x => new {x.QuoteId, x.PoNumber}).ToList();

            foreach (var quote in quotes.Where(x=> x.PoNumber != null && x.PoNumber.Length >30 ).ToList())
            {
                Console.WriteLine(quote.QuoteId);
            }

        }
    }
}
