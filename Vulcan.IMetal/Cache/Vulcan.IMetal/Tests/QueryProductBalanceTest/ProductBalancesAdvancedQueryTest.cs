using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Vulcan.IMetal.Queries.ProductBalances;

namespace Vulcan.IMetal.Tests.QueryProductBalanceTest
{
    [TestFixture()]
    public class ProductBalancesAdvancedQueryTest
    {
        [Test]
        public void BasicTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var query = ProductBalancesAdvancedQuery.AsQueryable("INC",null);
            //query = query.Where(x => x.ProductCode == "4140 6-4.5 HT36").AsQueryable();
            var results = query.ToList();

            sw.Stop();
            Console.WriteLine($"Query all Production Balances time: {sw.Elapsed} for {results.Count} results");

            //foreach (var cat in results.Select(x=>x.MetalCategory).Distinct().ToList())
            //{
            //    Console.WriteLine(cat);
            //}

            //foreach (var balance in results)
            //{
            //    Console.Write(ObjectDumper.Dump(balance));
            //}
        }
    }
}
