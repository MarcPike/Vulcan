using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.Queries.ProductCodes;

namespace Vulcan.IMetal.Cache
{
    public class ProductMasterCache : IProductMasterCache
    {
        public class CacheValues
        {
            public string Coid { get; set; }
            public DateTime CachedDate { get; set; }
            public List<ProductMasterAdvancedQuery> Values { get; set; } = new List<ProductMasterAdvancedQuery>();

        }
        static readonly object Object = new object();
        private readonly TimeSpan _timeSpan = new TimeSpan(0, 8, 0, 0);

        private readonly List<CacheValues> _cacheValues = new List<CacheValues>();
        public CacheValues GetForCoid(string coid, bool refreshCache)
        {
            lock (Object)
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
        }

        private CacheValues ClearAndAddDataset(string coid)
        {
            var cache = _cacheValues.SingleOrDefault(x => x.Coid == coid);
            if (cache != null) _cacheValues.Remove(cache);

            var query = ProductMasterAdvancedQuery.AsQueryable(coid, null);

            cache = new CacheValues()
            {
                Coid = coid,
                CachedDate = DateTime.Now,
                Values = query.OrderBy(x=>x.ProductCode).ToList()
            };

            _cacheValues.Add(cache);
            return cache;

        }

    }
}
