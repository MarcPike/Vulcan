using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.ReRuniMetalExports
{
    [TestFixture]
    public class UpdateExportStatusPending
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        /*
         
97323 - Success
100448 - Success
101345 - Success
102108 - Success
102505 - Success
102651 - Success
103140 - Success
103335 - Success
103439 - Success
103503 - Success
103566 - Success

            100448
         */

        [Test]
        public void UpdateStatusToPending()
        {
            var quotesSubmittedToday = CrmQuote.Helper
                .Find(x => x.ExportAttempts.Any(e => e.ExecutionDate >= DateTime.Now.Date  )).ToList();

            foreach (var quote in quotesSubmittedToday.Where(x=>x.QuoteId != 100448))
            {
                Console.WriteLine($"{quote.QuoteId} - {quote.ExportStatus}");
                quote.ExportStatus = ExportStatus.Pending;
                CrmQuote.Helper.Upsert(quote);
            }
        }

        [Test]
        public void fixbad()
        {
            var quote = CrmQuote.Helper.Find(x => x.QuoteId == 103311).FirstOrDefault();
            quote.ExportStatus = ExportStatus.Processing;
            CrmQuote.Helper.Upsert(quote);

        }

    }
}
