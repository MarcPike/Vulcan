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
    [TestFixture]
    public class ConvertQuickQuoteItemsToNewModel
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
        }

        [Test]
        public void Execute()
        {
            var rep = new RepositoryBase<CrmQuote>();
            var quotesWithQuickQuoteItems = rep.AsQueryable().Where(x => x.QuickQuoteItems.Count > 0).ToList();
            foreach (var crmQuote in quotesWithQuickQuoteItems)
            {
                Console.WriteLine(crmQuote.QuoteId + " has (" + crmQuote.QuickQuoteItems.Count + ") items Status = "+crmQuote.Status);
            }
        }
    }
}
