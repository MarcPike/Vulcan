using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.Queries.PurchaseOrderItems;

namespace Vulcan.IMetal.Cache
{
    public class PurchaseOrderItemsCache : IPurchaseOrderItemsCache
    {
        public class CacheValues
        {
            public Guid Id { get; set; } = Guid.NewGuid();

            public string Coid { get; set; }
            public DateTime CachedDate { get; set; }
            public List<PurchaseOrderItemsAdvancedQuery> Values { get; set; } = new List<PurchaseOrderItemsAdvancedQuery>();
            public List<string> UniqueProductCategories = new List<string>();
            public List<string> UniqueProductConditions = new List<string>();

        }
        static readonly object Object = new object();
        private readonly TimeSpan _timeSpan = new TimeSpan(0, 3, 0, 0);

        private readonly List<CacheValues> _cacheValues = new List<CacheValues>();
        public CacheValues GetForCoid(string coid, bool refreshCache)
        {
            lock (Object)
            {
                try
                {
                    var cache = _cacheValues.SingleOrDefault(x => x.Coid == coid);
                    if ((cache == null) || ((cache.CachedDate + _timeSpan) < DateTime.Now) || refreshCache)
                    {
                        return ClearAndAddDataset(coid);
                    }
                    else
                    {
                        return cache;
                    }

                }
                catch (Exception e)
                {
                    throw new Exception($"Exception occurred trying to Get Incoming Stock Cache for {coid} Exception: {e.Message}");
                }
            }
        }

        public string GetIdForCoid(string coid)
        {
            if (coid == string.Empty) throw new Exception("Coid is empty");

            var cache = _cacheValues.SingleOrDefault(x => x.Coid == coid);

            if (cache == null) return string.Empty;

            if ((cache.CachedDate + _timeSpan) > DateTime.Now)
            {
                return cache.Id.ToString();
            }

            return string.Empty;
        }

        public PurchaseOrderItemsCacheInfo GetInfoForCoid(string coid)
        {
            var id = GetIdForCoid(coid);

            if (id == string.Empty) return new PurchaseOrderItemsCacheInfo()
            {
                Coid = coid
            };

            var cache = _cacheValues.Single(x => x.Id == Guid.Parse(id));

            return new PurchaseOrderItemsCacheInfo(cache, _timeSpan);
        }

        private CacheValues ClearAndAddDataset(string coid)
        {
            var cache = _cacheValues.SingleOrDefault(x => x.Coid == coid);
            if (cache != null) _cacheValues.Remove(cache);

            var query = PurchaseOrderItemsAdvancedQuery.AsQueryable(coid, null);

            cache = new CacheValues()
            {
                Coid = coid,
                CachedDate = DateTime.Now,
                Values = query.ToList()
            };

            _cacheValues.Add(cache);
            return cache;
        }
    }
}
