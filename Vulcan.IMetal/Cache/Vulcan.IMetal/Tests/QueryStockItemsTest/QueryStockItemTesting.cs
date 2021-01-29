using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Vulcan.IMetal.Queries.StockItems;

namespace Vulcan.IMetal.Test.QueryStockItemsTest
{
    [TestFixture]
    public class QueryStockItemTesting
    {
        [Test]
        public void BasicTest()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var query = StockItemsAdvancedQuery.AsQueryable("INC");
            var queryable = query.Where(x => x.ProductId == 34856762);
            var results = queryable.ToList();
            stopwatch.Stop();
            Console.WriteLine("Query Construction: "+stopwatch.Elapsed);
            stopwatch.Start();
            foreach (var stockItem in results)
            {
                //Console.WriteLine($"Tag: {stockItem.TagNumber} Code: {stockItem.ProductCode} Available Length {stockItem.AvailableLengthBase} Cost/Lb: {stockItem.CostPerLb} Avail Weight/Kgs: {stockItem.AvailableWeightKgs}");
                //Console.WriteLine(ObjectDumper.Dump(stockItem));
                Console.WriteLine(ObjectDumper.Dump(stockItem));
            }
            stopwatch.Stop();
            Console.WriteLine("Query Execution: " + stopwatch.Elapsed);

        }
    }
}
