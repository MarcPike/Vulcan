using System;

namespace Vulcan.IMetal.Cache
{
    public class StockItemsCacheInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Coid { get; set; } = string.Empty;
        public DateTime CachedDate { get; set; } = DateTime.MinValue;
        public TimeSpan CacheSpan { get; set; } = TimeSpan.Zero;
        public DateTime Expires => CachedDate + CacheSpan;
        public bool RefreshRequired => Expires <= DateTime.Now;

        public StockItemsCacheInfo(StockItemsCache.CacheValues cache, TimeSpan span)
        {
            Id = cache.Id.ToString();
            Coid = cache.Coid;
            CachedDate = cache.CachedDate;
            CacheSpan = span;
        }

        public StockItemsCacheInfo()
        {
        }
    }

    public class PurchaseOrderItemsCacheInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Coid { get; set; } = string.Empty;
        public DateTime CachedDate { get; set; } = DateTime.MinValue;
        public TimeSpan CacheSpan { get; set; } = TimeSpan.Zero;
        public DateTime Expires => CachedDate + CacheSpan;
        public bool RefreshRequired => Expires <= DateTime.Now;

        public PurchaseOrderItemsCacheInfo(PurchaseOrderItemsCache.CacheValues cache, TimeSpan span)
        {
            Id = cache.Id.ToString();
            Coid = cache.Coid;
            CachedDate = cache.CachedDate;
            CacheSpan = span;
        }

        public PurchaseOrderItemsCacheInfo()
        {
        }
    }

}