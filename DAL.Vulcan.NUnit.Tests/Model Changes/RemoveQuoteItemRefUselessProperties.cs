using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Model_Changes
{
    [TestFixture()]
    public class RemoveQuoteItemRefUselessProperties
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
        }

        [Test]
        public void Execute()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var rep = new RepositoryBase<CrmQuote>();
            var quotes = rep.AsQueryable().ToList();
            foreach (var crmQuote in quotes)
            {
                Console.WriteLine($"QuoteId {crmQuote.QuoteId} is ok");
            }
        }

    }
}
