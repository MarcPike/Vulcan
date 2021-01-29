using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vulcan.IMetal.Tests.NonStockItems
{
    [TestFixture]
    class TestNonStockItems
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void LookForGlennBProductCode()
        {
            //NonStockItemsCache = new List<ProductsWithNoInventoryModel>();
            //var productMasters = _productMasterCache.GetForCoid(coid, false).Values.
            //    Where(x => !x.StockType.Contains("Machined")).
            //    Select(x => new { x.Coid, x.ProductId, x.ProductCode, x.InsideDiameter, x.OuterDiameter, x.ProductCondition, x.ProductCategory, x.ProductSize, x.MetalType, x.StockGrade, x.StratificationRank }).ToList();
            //var stockItems = _stockItemsCache.GetForCoid(coid, false).StockItems.Where(x => x.AvailableQuantity > 0).Select(x => x.ProductId).ToList();
            //var purchaseOrderItems = _purchaseOrderItemsCache.GetForCoid(coid, false).Values
            //    .Select(x => x.ProductId).ToList();
            //foreach (var productMaster in productMasters)
            //{
            //    var productId = productMaster.ProductId;
            //    if ((stockItems.All(x => x != productId)) && (purchaseOrderItems.All(x => x != productId)))
            //    {
            //        NonStockItemsCache.Add(new ProductsWithNoInventoryModel()
            //        {
            //            Coid = coid,
            //            ProductId = productId,
            //            ProductCode = productMaster.ProductCode,
            //            InsideDiameter = productMaster.InsideDiameter,
            //            OuterDiameter = productMaster.OuterDiameter,
            //            ProductCondition = productMaster.ProductCondition,
            //            ProductCategory = productMaster.ProductCategory,
            //            ProductSize = productMaster.ProductSize,
            //            MetalType = productMaster.MetalType,
            //            StockGrade = productMaster.StockGrade,
            //            StratificationRank = productMaster.StratificationRank
            //        });
            //    }
            //}
            //result.NonStockItems = NonStockItemsCache.OrderBy(x => x.ProductCode);

        }
    }
}
