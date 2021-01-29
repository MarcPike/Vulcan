using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devart.Data.Linq;
using Vulcan.IMetal.Helpers;
using Vulcan.IMetal.Queries.ProductBalances;
using Vulcan.IMetal.Queries.StockItems;

namespace Vulcan.IMetal.Cache
{
    public class StockItemsCache : IStockItemsCache
    {
        public class CacheValues: IDisposable
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Coid { get; set; }
            public DateTime CachedDate { get; set; }
            public List<StockItemsCondensed> StockItems { get; set; } = new List<StockItemsCondensed>();
            public List<StockItemsCondensed> MachinedParts { get; set; } = new List<StockItemsCondensed>();
            public bool Refreshed { get; set; } = false;

            public void Dispose()
            {
                StockItems.Clear();
            }
        }
        static readonly object Object = new object();
        private readonly TimeSpan _timeSpan = new TimeSpan(0,3,0,0);

        protected readonly List<CacheValues> _cacheValues = new List<CacheValues>();

        public StockItemsCacheInfo GetInfoForCoid(string coid)
        {
            var id = GetIdForCoid(coid);

            if (id == string.Empty) return new StockItemsCacheInfo()
            {
                Coid = coid,
            };

            var cache = _cacheValues.Single(x => x.Id == Guid.Parse(id));

            return new StockItemsCacheInfo(cache, _timeSpan);
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

        public CacheValues GetForCoid(string coid, bool refreshCache)
        {
            if (coid == string.Empty) throw new Exception("Coid is empty");

            lock (Object)
            {
                try
                {
                    var cache = _cacheValues.SingleOrDefault(x => x.Coid == coid);
                    if (cache == null)
                    {
                        return ClearAndAddDataset(coid);
                    }
                    else if (((cache.CachedDate + _timeSpan) < DateTime.Now) || refreshCache)
                    {
                        cache.Dispose();
                        cache = null;

                        //GC.Collect();
                        //GC.WaitForPendingFinalizers();

                        return ClearAndAddDataset(coid);
                    }
                    else
                    {
                        cache.Refreshed = false;
                        return cache;
                    }

                }
                catch (Exception e)
                {
                    throw new Exception($"Exception occurred trying to Get StockItem Cache for {coid} Exception: {e.Message}");
                }
            }
        }

        protected virtual CacheValues ClearAndAddDataset(string coid)
        {
            var cache = _cacheValues.SingleOrDefault(x => x.Coid == coid);
            if (cache != null) _cacheValues.Remove(cache);

            var helperCurrencyForIMetal = new HelperCurrencyForIMetal();
            var currency = helperCurrencyForIMetal.GetDefaultCurrencyForCoid(coid);

           var queryAllStockItems = StockItemsAdvancedQuery.AsQueryable(coid, null).Select(x=> 
                new StockItemsCondensed(x, currency));

           var allStockItems = queryAllStockItems.ToList();

            cache = new CacheValues()
            {
                Coid = coid,
                CachedDate = DateTime.Now,
                StockItems = allStockItems.Where(x=>!x.IsMachinedPart && !x.IsZeroWeightService).ToList(),
                MachinedParts = allStockItems.Where(x=>x.IsMachinedPart && !x.IsZeroWeightService).ToList(),
                Refreshed = true
             };

            _cacheValues.Add(cache);

            return cache;

        }

    }

}
