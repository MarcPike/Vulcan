using System;
using System.Diagnostics;
using NUnit.Framework;
using Vulcan.IMetal.Queries.PurchaseOrderItems;

namespace Vulcan.IMetal.Tests.QueryPurchaseOrderTest
{
    [TestFixture]
    public class QueryPurchaseOrderTesting
    {
        [Test]
        public void BasicTest()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var query = new QueryPurchaseOrderItems("INC");
            query.FilterMetalCategory.Equals("LOW ALLOY");
            query.FilterProductCategory.Contains("B4145M");
            //query.FilterOutsideDiameter.Between(2,20);
            query.FilterLength.Between(10,12);
            stopwatch.Stop();
            Console.WriteLine("Query Construction: " + stopwatch.Elapsed);
            stopwatch.Start();
            var results = query.Execute();
            stopwatch.Stop();
            Console.WriteLine($"Query Execution: {stopwatch.Elapsed} ({results.Count}) rows found ");
            foreach (var stockItemResult in results)
            {
                Console.WriteLine(ObjectDumper.Dump($"StockItemId: {stockItemResult.Id} Product Category: {stockItemResult.ProductCategory} Available FilterLength: {stockItemResult.Length} OutsideDiameter: {stockItemResult.OutsideDiameter}"));
            }

        }
    }
}
