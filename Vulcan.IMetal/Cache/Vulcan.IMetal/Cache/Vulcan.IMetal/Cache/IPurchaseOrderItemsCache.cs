namespace Vulcan.IMetal.Cache
{
    public interface IPurchaseOrderItemsCache
    {
        PurchaseOrderItemsCache.CacheValues GetForCoid(string coid, bool refreshCache);

        string GetIdForCoid(string coid);
        PurchaseOrderItemsCacheInfo GetInfoForCoid(string coid);

    }
}