using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.RemoveQuotes
{
    [TestFixture]
    class RemoveQuotes
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        public List<CrmQuote> GetQuotesToRemove()
        {
            return CrmQuote.Helper.Find(x => x.Coid == "DUB" /*&& x.CreateDateTime <= DateTime.Parse("8/1/2020")*/).ToList();
        }

        public void RemoveQuotesInList(List<CrmQuote> quotes)
        {
            foreach (var quote in quotes)
            {
                foreach (var itemRef in quote.Items)
                {
                    CrmQuoteItem.Helper.DeleteOne(itemRef.Id);
                }

                CrmQuote.Helper.DeleteOne(quote.Id);
            }
        }

        [Test]
        public void Execute()
        {
            RemoveQuotesInList(GetQuotesToRemove());
        }
    }
}
