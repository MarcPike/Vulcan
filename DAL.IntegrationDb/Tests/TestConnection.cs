using System;
using System.Linq;
using NUnit.Framework;

namespace DAL.IntegrationDb.Tests
{
    [TestFixture()]
    public class TestConnection
    {
        [Test]
        public void OpenConnectionToIntegrationDbAndFindImportSalesHeaders()
        {
            using (var context = new IntegrationDb())
            {
                var salesHeaders = context.import_sales_headers.Where(x => x.import_source == "QuoteTool").Take(10).ToList();
                foreach (var salesHeader in salesHeaders)
                {
                    Console.WriteLine(ObjectDumper.Dump(salesHeader));
                }
            }
        }
    }
}
