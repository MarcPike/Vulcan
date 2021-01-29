using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.ProductMasterTest
{
    [TestFixture()]
    public class TestProductMaster
    {
        [Test]
        public void OpenAllQuoteItems()
        {
            var rep = new RepositoryBase<CrmQuoteItem>();
            var quoteItems = rep.AsQueryable().ToList();
            foreach (var crmQuoteItem in quoteItems)
            {
                var startingProduct = crmQuoteItem.QuotePrice.StartingProduct;
                Assert.IsTrue(startingProduct.StockGrade != string.Empty);
            }
        }

        [Test]
        public void GetProductMasterTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var startingProduct = new Mongo.DocClass.Quotes.ProductMaster("INC", 25670398);

            sw.Stop();
            Console.WriteLine($"{sw.Elapsed.Seconds} seconds");
            Console.WriteLine(ObjectDumper.Dump(startingProduct));
            sw.Start();

        }

        [Test]
        public void GetLongDescription()
        {
            var productMaster = new Mongo.DocClass.Quotes.ProductMaster("INC", 8995);

            var requiredQuantity = new RequiredQuantity(new OrderQuantity(1, 20, "in"), "INC", (decimal)14.878579712);

            var longDescription = productMaster.GetLongDescription(requiredQuantity, "TEST PART # 1", "BMS-N201 REV AA");

            Console.WriteLine(longDescription);
        }
    }
}
