using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;
using SVC.QNG.Exporter.Models;

namespace SVC.QNG.Exporter.UpdateDataScripts
{
    [TestFixture]
    public class AddTagNumber
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            using (var context = new ODSContext())
            {
                var itemsMissingTagNumber = context.Vulcan_CrmQuoteItem.AsNoTracking()
                    .Where(x => !x.IsQuickQuoteItem && !x.IsCostMadeup && x.TagNumber == string.Empty)
                    .Select(x=> new {x.Id, x.QuoteId, x.QuoteItemId, x.TagNumber}).ToList();

                var count = itemsMissingTagNumber.Count;
                var index = 0;
                foreach (var vulcanCrmQuoteItem in itemsMissingTagNumber)
                {
                    var quoteItem =
                        CrmQuote.GetQuoteItemFor(vulcanCrmQuoteItem.QuoteId, vulcanCrmQuoteItem.QuoteItemId);
                    if (quoteItem != null)
                    {
                        var tagNumber = quoteItem.QuotePrice?.MaterialCostValue?.BaseCost?.TagNumber ?? string.Empty;
                        if ((vulcanCrmQuoteItem.TagNumber == string.Empty) && (tagNumber != string.Empty))
                        {
                            var odsQuoteItem = context.Vulcan_CrmQuoteItem
                                .First(x => x.Id == vulcanCrmQuoteItem.Id);
                            odsQuoteItem.TagNumber = tagNumber;
                            context.SaveChanges();
                        }
                    }

                    index++;
                }

            }

        }


    }
}
