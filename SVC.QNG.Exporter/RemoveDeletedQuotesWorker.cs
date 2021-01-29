using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;
using SVC.QNG.Exporter.Models;

namespace SVC.QNG.Exporter
{
    [TestFixture]
    public class RemoveDeletedQuotesWorker
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void RemoveTest()
        {
            Stopwatch sw = new Stopwatch();
            using (var context = new ODSContext())
            {
                sw.Start();
                var removeCount = Execute(context);
                sw.Stop();
                Console.WriteLine($"Elapsed: {sw.Elapsed} Removed {removeCount} rows");
            }
        }

        public int Execute(ODSContext context)
        {
            var project = CrmQuote.Helper.ProjectionBuilder.Expression(x => x.QuoteId);
            var filter = CrmQuote.Helper.FilterBuilder.Empty;
            var allVulcanQuoteIds = CrmQuote.Helper.FindWithProjection(filter, project).ToList();
            var allODSQuoteIds = context.Vulcan_CrmQuote.Select(x => x.QuoteId).ToList();

            var removeThese = allODSQuoteIds.Except(allVulcanQuoteIds).ToList();
            var removeCount = 0;
            foreach (var i in removeThese)
            {
                var deleteQuote = context.Vulcan_CrmQuote.Single(x => x.QuoteId == i);
                foreach (var vulcanCrmQuoteItem in deleteQuote.Vulcan_CrmQuoteItem.ToList())
                {
                    foreach (var vulcanCrmQuoteItemTestPiece in vulcanCrmQuoteItem.Vulcan_CrmQuoteItem_TestPieces.ToList())
                    {
                        context.Vulcan_CrmQuoteItem_TestPieces.Remove(vulcanCrmQuoteItemTestPiece);
                    }

                    foreach (var vulcanCrmQuoteItemProductionCost in vulcanCrmQuoteItem.Vulcan_CrmQuoteItem_ProductionCost.ToList())
                    {
                        context.Vulcan_CrmQuoteItem_ProductionCost.Remove(vulcanCrmQuoteItemProductionCost);
                    }

                    context.Vulcan_CrmQuoteItem.Remove(vulcanCrmQuoteItem);
                }

                context.Vulcan_CrmQuote.Remove(deleteQuote);
                context.SaveChanges();
                removeCount++;
            } 
            
            Console.WriteLine($"{removeCount} Quotes removed from QNG because they no longer exist");

            return removeCount;
        }
    }
}
