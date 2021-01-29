using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devart.Data.Linq.Monitoring;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.StockItems;
using Vulcan.IMetal.Queries.StockItems;

namespace Vulcan.IMetal.Test.QueryStockItemsTest
{
    [TestFixture]
    public class AdvancedQueryTesting
    {
        private readonly LinqMonitor _monitor = new LinqMonitor();

        public string Coid { get; set; }
        public StockItemsContext Context;

        [SetUp]
        public void Setup()
        {
            Coid = "INC";
            Context = ContextFactory.GetStockItemsContextForCoid(Coid);
            Context.Connection.Open();
        }

        [TearDown]
        public void TearDown()
        {
            Context.Connection.Close();
        }

        [Test]
        public void GetCategories()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var categories = StockItemsAdvancedQuery.GetProductCategories("INC");
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");
            foreach (var category in categories)
            {
                Console.WriteLine(category);
            }
        }

        [Test]
        public void GetConditions()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var conditions = StockItemsAdvancedQuery.GetProductConditions("INC");
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");
            foreach (var condition in conditions)
            {
                Console.WriteLine(condition);
            }
        }

        [Test]
        public void Get925Density()
        {
            Context.Connection.Open();
            var stockItemQuery = StockItemsAdvancedQuery.AsQueryable(Coid, Context);

            stockItemQuery = stockItemQuery.Where(x => x.StockGrade == "925");

            var result = stockItemQuery.ToList();

        }

        [Test]
        public void GetMalaysiaMachinedParts()
        {
            Context.Connection.Open();
            var stockItemQuery = StockItemsAdvancedQuery.AsQueryable(Coid, Context);

            stockItemQuery = stockItemQuery.Where(x => x.IsMachinedPart);
            var result = stockItemQuery.ToList();


        }


        [Test]
        public void GetAllInventory()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Context.Connection.Open();
            var stockItemQuery = StockItemsAdvancedQuery.AsQueryable(Coid, Context);

            //stockItemQuery = stockItemQuery.Where(x => x.ProductCode == "4140 7.25-4.25 110P");
            stockItemQuery = stockItemQuery.Where(x => x.Density != 0);

            var result = stockItemQuery.ToList();

            sw.Stop();
            Console.WriteLine($"Query execution time: {sw.Elapsed} rows: {result.Count}");
            Console.WriteLine(ObjectDumper.Dump(result[0]));
            Context.Connection.Close();
            _monitor.IsActive = false;

        }

        [Test]
        public void RawQueryTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Context.Connection.Open();
            var stockItemQuery = StockItemsAdvancedQuery.AsQueryable(Coid, Context);

            //stockItemQuery = stockItemQuery.Where(x => x.InsideDiameter == 0).AsQueryable();
            //stockItemQuery = stockItemQuery.Where(x => x.OuterDiameter == 10).AsQueryable();
            
            stockItemQuery = stockItemQuery.Where(x => x.AvailableLength > 12);
            stockItemQuery = stockItemQuery.Where(x => x.StockGrade == "718");
            stockItemQuery = stockItemQuery.Where(x => x.ProductCondition == "SAAH");

            ////stockItemQuery = stockItemQuery.Where(x => x.ProductControlCode == "MW");
            stockItemQuery = stockItemQuery.Where(x => x.AvailableWeight > 0);
            //stockItemQuery = stockItemQuery.Where(x => x.TagNumber == "1088305" && x.HeatNumber == "725169");

            sw.Stop();
            Console.WriteLine("Query construction time: "+sw.Elapsed);
            sw.Start();
            _monitor.IsActive = true;
            var result = stockItemQuery.ToList();
            foreach (var stockItem in result.ToList())
            {
                //Console.WriteLine($"Tag: {stockItem.TagNumber} Size: {stockItem.ProductSize} Code: {stockItem.ProductCode} StockHoldReason: {stockItem.StockHoldReason} Available Length {stockItem.AvailableLength} Cost/Lb: {stockItem.CostPerLb} Condition: {stockItem.ProductCondition}");
                //Console.WriteLine($"Tag: {stockItem.TagNumber} Size: {stockItem.ProductSize} Cost per Qty: {stockItem.CostPerQty} Total Inches: {stockItem.AvailableInches} Cost per Inch: {stockItem.CostPerInch}");
                //var myCostPerKg = (stockItem.TotalCost / stockItem.PhysicalWeight) * stockItem.FactorForKilograms;
                //Console.WriteLine($"My Cost Per Kg == {myCostPerKg}");

                Console.Write(ObjectDumper.Dump(stockItem));
            }
            sw.Stop();
            Console.WriteLine($"Query execution time: {sw.Elapsed} rows: {result.Count}");
            Context.Connection.Close();
            _monitor.IsActive = false;

        }

        [Test]
        public void GetUniqueQuantityCodes()
        {
            Context.Connection.Open();
            var stockItemQuery = StockItemsAdvancedQuery.AsQueryable(Coid, Context);
            var uniqueQuantityUnits = stockItemQuery.GroupBy(x => x.QuantityUnit).Select(n => new
            {
                QuantityUnit = n.Key,
                Counter = n.Count()
            }).OrderBy(n => n.QuantityUnit);

            foreach (var uniqueQuantityUnit in uniqueQuantityUnits)
            {
                Console.WriteLine($"QuantityUnit: {uniqueQuantityUnit.QuantityUnit} - {uniqueQuantityUnit.Counter}");
            }

        }

        [Test]
        public void StockHoldTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Context.Connection.Open();
            var stockItemQuery = StockItemsAdvancedQuery.AsQueryable(Coid, Context);
            stockItemQuery = stockItemQuery.Where(x => x.StockHoldUser != null).AsQueryable();

            sw.Stop();
            Console.WriteLine("Query construction time: " + sw.Elapsed);
            sw.Start();
            _monitor.IsActive = true;
            var result = stockItemQuery.ToList();
            foreach (var stockItem in result.ToList())
            {
                //Console.WriteLine($"Tag: {stockItem.TagNumber} Size: {stockItem.ProductSize} Code: {stockItem.ProductCode} StockHoldReason: {stockItem.StockHoldReason} Available Length {stockItem.AvailableLength} Cost/Lb: {stockItem.CostPerLb} Condition: {stockItem.ProductCondition}");
                //Console.WriteLine($"Tag: {stockItem.TagNumber} Size: {stockItem.ProductSize} Cost per Qty: {stockItem.CostPerQty} Total Inches: {stockItem.AvailableInches} Cost per Inch: {stockItem.CostPerInch}");
                //var myCostPerKg = (stockItem.TotalCost / stockItem.PhysicalWeight) * stockItem.FactorForKilograms;
                //Console.WriteLine($"My Cost Per Kg == {myCostPerKg}");

                //Console.WriteLine($"Tag$: {stockItem.TagNumber} Code: {stockItem.ProductCode} StockHold Reason:{stockItem.StockHoldReason} User: {stockItem.StockHoldUser}");
                Console.WriteLine($"StockHold Reason:{stockItem.StockHoldReason} User: {stockItem.StockHoldUser}");
            }
            sw.Stop();
            Console.WriteLine($"Query execution time: {sw.Elapsed} rows: {result.Count}");
            Context.Connection.Close();
            _monitor.IsActive = false;

        }

    }
}
