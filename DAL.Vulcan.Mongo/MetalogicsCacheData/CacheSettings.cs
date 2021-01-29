using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Vulcan.Mongo.MetalogicsCacheData
{
    public class CacheSettings : BaseDocument
    {
        private static MongoRawQueryHelper<CacheSettings> _helper = new MongoRawQueryHelper<CacheSettings>();
        public Guid StockItemsActiveCacheId { get; set; }
        public DateTime LastStockItemsLoad { get; set; }
        public Guid PurchaseOrderItemsActiveCacheId { get; set; }
        public DateTime LastPurchaseOrderItemsLoad { get; set; }
        public Guid ProductMastersCacheId { get; set; }
        public DateTime LastProductMastersLoad { get; set; }

        public static CacheSettings Get()
        {
            var result = _helper.Find(_helper.FilterBuilder.Empty).FirstOrDefault() ?? new CacheSettings();
            return _helper.Upsert(result);
        }

        public static void SetStockItemsCacheId(Guid newId)
        {
            var cacheSettings = Get();
            cacheSettings.StockItemsActiveCacheId = newId;
            cacheSettings.LastStockItemsLoad = DateTime.Now;
            _helper.Upsert(cacheSettings);
        }

        public static void SetProductMastersCacheId(Guid newId)
        {
            var cacheSettings = Get();
            cacheSettings.ProductMastersCacheId = newId;
            cacheSettings.LastProductMastersLoad = DateTime.Now;
            _helper.Upsert(cacheSettings);
        }

        public static void SetPurchaseOrderItemsCacheId(Guid newId)
        {
            var cacheSettings = Get();
            cacheSettings.PurchaseOrderItemsActiveCacheId = newId;
            cacheSettings.LastPurchaseOrderItemsLoad = DateTime.Now;
            _helper.Upsert(cacheSettings);
        }

        public static Guid GetCurrentStockItemsCacheId()
        {
            return Get().StockItemsActiveCacheId;
        }

        public static Guid GetCurrentPurchaseOrderItemsCacheId()
        {
            return Get().PurchaseOrderItemsActiveCacheId;
        }

        public static Guid GetCurrentProductMastersCacheId()
        {
            return Get().ProductMastersCacheId;
        }

    }
}