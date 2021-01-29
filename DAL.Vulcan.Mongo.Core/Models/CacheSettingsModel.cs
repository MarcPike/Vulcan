using System;
using System.Collections.Generic;
using System.Text;
using DAL.Vulcan.Mongo.Core.MetalogicsCacheData;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class CacheSettingsModel
    {

        public CacheSettingsModel()
        {
            var cacheSettings = CacheSettings.Get();
            LastProductMastersLoad = cacheSettings.LastProductMastersLoad;
            LastPurchaseOrderItemsLoad = cacheSettings.LastPurchaseOrderItemsLoad;
            LastStockItemsLoad = cacheSettings.LastStockItemsLoad;
            ProductMastersCacheId = cacheSettings.ProductMastersCacheId.ToString();
            PurchaseOrderItemsActiveCacheId = cacheSettings.PurchaseOrderItemsActiveCacheId.ToString();
            StockItemsActiveCacheId = cacheSettings.StockItemsActiveCacheId.ToString();
        }

        public string StockItemsActiveCacheId { get; set; }

        public string PurchaseOrderItemsActiveCacheId { get; set; }

        public string ProductMastersCacheId { get; set; }

        public DateTime LastStockItemsLoad { get; set; }

        public DateTime LastPurchaseOrderItemsLoad { get; set; }

        public DateTime LastProductMastersLoad { get; set; }
    }
}
