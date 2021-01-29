namespace Vulcan.IMetal.Cache
{
    public interface IProductMasterCache
    {
        ProductMasterCache.CacheValues GetForCoid(string coid, bool refreshCache);
    }
}