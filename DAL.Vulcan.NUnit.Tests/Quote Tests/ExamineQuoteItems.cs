using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Quote_Tests
{
    [TestFixture]
    public class ExamineQuoteItems
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void LookAtQuoteItems()
        {
            var quote = CrmQuote.Helper.Find(x => x.QuoteId == 147706).First();
            foreach (var item in quote.Items.Select(x=>x.AsQuoteItem()))
            {
                Console.WriteLine(item.QuoteSource);
            }
        }

        [Test]
        public void InspectProductId()
        {
            var productId = 5626374;
            //var productMasterQuery = new Prod
            
        }
    }
}
