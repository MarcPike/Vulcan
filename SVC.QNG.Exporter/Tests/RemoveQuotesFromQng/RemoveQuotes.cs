using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SVC.QNG.Exporter.Models;

namespace SVC.QNG.Exporter.Tests.RemoveQuotesFromQng
{
    [TestFixture]
    class RemoveQuotes
    {
        private readonly ODSContext _context = new ODSContext();

        private List<Vulcan_CrmQuote> GetQuotesToRemove()
        {
                return _context.Vulcan_CrmQuote.Where(x => x.Coid == "DUB" ).ToList();
        }


        private void RemoveQuotesInList(List<Vulcan_CrmQuote> quotes)
        {
            foreach (var quote in quotes.ToList())
            {
                foreach (var quoteItem in quote.Vulcan_CrmQuoteItem.ToList())
                {
                    foreach (var productionCost in quoteItem.Vulcan_CrmQuoteItem_ProductionCost.ToList())
                    {
                        _context.Vulcan_CrmQuoteItem_ProductionCost.Remove(productionCost);
                    }

                    foreach (var testPiece in quoteItem.Vulcan_CrmQuoteItem_TestPieces.ToList())
                    {
                        _context.Vulcan_CrmQuoteItem_TestPieces.Remove(testPiece);
                    }

                    _context.Vulcan_CrmQuoteItem.Remove(quoteItem);
                }

                _context.Vulcan_CrmQuote.Remove(quote);

                _context.SaveChanges();
            }
        }

        [Test]
        public void Execute()
        {
            RemoveQuotesInList(GetQuotesToRemove());
        }

    }
}
