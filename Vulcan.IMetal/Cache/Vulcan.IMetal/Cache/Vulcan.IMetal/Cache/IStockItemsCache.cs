using System;
using System.Collections.Generic;
using Vulcan.IMetal.Queries.ProductBalances;

namespace Vulcan.IMetal.Cache
{
    public interface IStockItemsCache
    {
        string GetIdForCoid(string coid);
        StockItemsCache.CacheValues GetForCoid(string coid, bool refreshCache);
        StockItemsCacheInfo GetInfoForCoid(string coid);
    }
}