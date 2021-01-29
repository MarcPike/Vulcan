using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SVC.QNG.Exporter.Models;

namespace SVC.QNG.Exporter.Tests
{
    [TestFixture]
    class CheckIfQuoteExists
    {
        [Test]
        public void CheckForQuote()
        {
            using (var context = new ODSContext())
            {
                var quoteId = 131044;
                var quote = context.Vulcan_CrmQuote.SingleOrDefault(x => x.QuoteId == quoteId);
                Console.WriteLine(quote != null
                    ? $"QuoteId: {quote.QuoteId} Company: {quote.CompanyName} SalesPerson: {quote.SalesPerson} has been exported"
                    : "Quote does not exist");
            }
        }
    }
}
