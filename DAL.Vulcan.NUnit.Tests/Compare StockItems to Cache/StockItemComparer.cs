using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.MetalogicsCacheData;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Driver;
using NUnit.Framework;
using Vulcan.IMetal.Queries.StockItems;

namespace DAL.Vulcan.NUnit.Tests.Compare_StockItems_to_Cache
{
    [TestFixture]
    public class StockItemComparer
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void ExecuteTake10()
        {
            var stockItemsQuery = StockItemsAdvancedQuery.AsQueryable("EUR");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var oldStockItems = stockItemsQuery.Where(x=>x.AvailablePieces > 0).ToList();
            oldStockItems = oldStockItems.Where(x => x.AvailableInches > 0).ToList();
            sw.Stop();

            var stockCacheId = CacheSettings.GetCurrentStockItemsCacheId();

            var newStockItems = StockItemsCache.Helper.Find(x => x.CacheId == stockCacheId && x.Coid == "EUR" && x.AvailablePieces > 0).ToList();

            foreach (var stockItemOld  in oldStockItems.Take(10))
            {
                var stockItemNew = newStockItems
                    .FirstOrDefault(x => x.CacheId == stockCacheId && 
                                         x.TagNumber == stockItemOld.TagNumber &&
                                         x.Coid == stockItemOld.Coid);
                Console.WriteLine($"{stockItemOld.ProductCode} - {stockItemNew.ProductCode}");
                Console.WriteLine($"{stockItemOld.TheoWeight} - {stockItemNew.TheoWeight}");
                Console.WriteLine($"{stockItemOld.CostPerLb} - {stockItemNew.CostPerLb}");
                Console.WriteLine($"{stockItemOld.CostPerKg} - {stockItemNew.CostPerKg}");
                Console.WriteLine($"{stockItemOld.AvailableInches} - {stockItemNew.AvailableInches}");
                Console.WriteLine($"{stockItemOld.AvailableWeightLbs} - {stockItemNew.AvailableWeightLbs}");
                Console.WriteLine($"{stockItemOld.Density} - {stockItemNew.Density}");
                Console.WriteLine($"{stockItemOld.VolumeDensity} - {stockItemNew.VolumeDensity}");
                Console.WriteLine($"{stockItemOld.ProductDensity} - {stockItemNew.ProductDensity}");


                Console.WriteLine("==============================================");
            }


        }
    }
}
