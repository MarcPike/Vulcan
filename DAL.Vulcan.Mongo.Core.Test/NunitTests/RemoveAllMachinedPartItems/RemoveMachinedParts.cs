using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.Mongo.Core.Test.NunitTests.RemoveAllMachinedPartItems
{
    [TestFixture]
    class RemoveMachinedParts
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void RemoveAllMachinedParts()
        {
            var quotes = CrmQuote.Helper.Find(x => x.Items.Any(i => i.ItemSummaryViewModel.IsMachinedPart)).ToList();
            foreach (var crmQuote in quotes)
            {
                foreach (var crmQuoteItemRef in crmQuote.Items.Where(x=>x.ItemSummaryViewModel.IsMachinedPart).ToList())
                {
                    var id = crmQuoteItemRef.Id;
                    crmQuote.Items.Remove(crmQuoteItemRef);
                    CrmQuoteItem.Helper.DeleteOne(id);

                }
            }
        }
    }
}
