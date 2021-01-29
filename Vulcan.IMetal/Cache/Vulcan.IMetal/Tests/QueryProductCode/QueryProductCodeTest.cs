using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Vulcan.IMetal.Queries.ProductCodes;
using Vulcan.IMetal.Queries.StockItems;

namespace Vulcan.IMetal.Tests.QueryProductCode
{
    [TestFixture]
    public class QueryProductCodeTest
    {

        [Test]
        public void GetAllProductsForIncWithTime()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var productCodes = ProductMasterAdvancedQuery.GetProductCodesForCoid("INC");
            sw.Stop();
            Console.WriteLine($"{sw.Elapsed.Seconds} seconds : Rows Found: {productCodes.Count}");
            sw.Start();

        }

        [Test]
        public void GetAllProductsForINC()
        {
            var query = ProductMasterAdvancedQuery.AsQueryable("INC");
            query = query.Where(x => x.ProductCode == "4130 3.75-2.25 75QF");
           
            var results = query.ToList();
            foreach (var productCodesAdvancedQuery in results)
            {
                Console.WriteLine(ObjectDumper.Dump(productCodesAdvancedQuery));
            }
        }

        [Test]
        public void GetAllProductsForEachMetalType()
        {
            var query = StockItemsAdvancedQuery.AsQueryable("INC");
            var stainless = query.Where(x => x.MetalType == "NICKEL").ToList();

            foreach(var cat in stainless)
            {
                Console.WriteLine(cat);
            }

        }

        [Test]
        public void GetRegret()
        {
            var query = ProductMasterAdvancedQuery.AsQueryable("INC");
            query = query.Where(x => x.ProductCode.StartsWith("REGRET"));

            var results = query.ToList();
            foreach (var productCodesAdvancedQuery in results)
            {
                Console.WriteLine(ObjectDumper.Dump(productCodesAdvancedQuery));
            }
        }



        [Test]
        public void GetFlatBarForEur()
        {
            var query = ProductMasterAdvancedQuery.AsQueryable("EUR");
            query = query.Where(x => x.ProductCode.StartsWith("625F"));

            var results = query.ToList();
            foreach (var productCodesAdvancedQuery in results.Where(x=> x.ProductCode.StartsWith("625F")).ToList())
            {
                Console.WriteLine(ObjectDumper.Dump(productCodesAdvancedQuery));
            }
        }


        [Test]
        public void NegativeTheoWeightWhy()
        {
            var query = ProductMasterAdvancedQuery.AsQueryable("INC");
            query = query.Where(x => x.ProductId == 8995);
            var results = query.ToList();
            foreach (var productCodesAdvancedQuery in results)
            {
                Console.WriteLine(ObjectDumper.Dump(productCodesAdvancedQuery));
            }
        }

        [Test]
        public void CheckCanadaProducts()
        {
            var query = ProductMasterAdvancedQuery.AsQueryable("CAN");
            query = query.Where(x => (x.ProductCode == "4140 11.555-9.055 80L") || (x.ProductCode == "4140 7.25-4.25 110P"));
            var results = query.ToList();
            foreach (var productCodesAdvancedQuery in results)
            {
                Console.WriteLine(ObjectDumper.Dump(productCodesAdvancedQuery));
            }
        }

    }
}
