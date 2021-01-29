using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BI.DAL.DataWarehouse.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BI.DAL.DataWarehouse.Core.Helpers
{
    public class HelperOds : IHelperOds
    {
        private RefreshCacheResultModel _wareHouseCacheInfo { get; set; }
        private static List<WarehouseModel> _warehouseCache = new List<WarehouseModel>();

        public List<string> GetCoidList()
        {
            return new List<string>()
            {
                "INC",
                "CAN",
                "EUR",
                "DUB",
                "MSA",
                "SIN"
            };
        }

        public List<BranchModel> GetBranchList(List<string> coidList)
        {
            var result = new List<BranchModel>();
            using (var context = new ODSContext())
            {

                foreach (var coid in coidList)
                {
                    result.AddRange(context.Branches.AsNoTracking().Where(x => x.Coid == coid)
                        .Select(x => new BranchModel()
                        {
                            Coid = x.Coid,
                            Name = x.Name,
                            Code = x.Code,
                            Status = x.Status,
                            Type = x.Type
                        }).ToList());
                }

            }

            return result;

        }

        public RefreshCacheResultModel RefreshWarehouseCache()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var context = new ODSContext())
            {
                _warehouseCache = context.Warehouses.Select(x => new WarehouseModel()
                { Coid = x.Coid, Status = x.Status, Warehouse = x.Name }).AsNoTracking().ToList();
            }
            sw.Stop();

            _wareHouseCacheInfo = new RefreshCacheResultModel()
            {
                RowCount = _warehouseCache.Count,
                Duration = sw.Elapsed,
                BuiltAt = DateTime.Now
            };

            return _wareHouseCacheInfo;
        }

        public List<WarehouseModel> GetWarehousesForCoids(List<string> coidList, bool refreshCache)
        {
            if (!_warehouseCache.Any() || refreshCache) RefreshWarehouseCache();

            return _warehouseCache.Where(x => coidList.Contains(x.Coid)).ToList();
        }

        public List<BusinessUnitWarehouseModel> GetBusinessUnitWarehouses(List<string> coidList)
        {
            var result = new List<BusinessUnitWarehouseModel>();
            using (var context = new ODSContext())
            {

                //foreach (var coid in coidList)
                //{
                    result.AddRange(context.QngVQbusinessUnitWarehouse.AsNoTracking().Where(x => coidList.Contains(x.Directory)).Select(x =>
                        new BusinessUnitWarehouseModel()
                        {
                            Coid = x.Directory,
                            Warehouse = x.Warehouse,
                            BusinessUnitId = x.BusinessUnitId
                        }).ToList());

                //}
                var businessUnits = context.QngVQbusinessUnit.Select(x => new { x.Id, x.BusinessUnit }).ToList();
                foreach (var model in result)
                {
                    model.BusinessUnit = businessUnits.FirstOrDefault(x => x.Id == model.BusinessUnitId)?.BusinessUnit ?? string.Empty;
                }

            }

            return result;
        }
    }
}
