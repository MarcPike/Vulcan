using System.Collections.Generic;
using BI.DAL.DataWarehouse.Core.Models;

namespace BI.DAL.DataWarehouse.Core.Helpers
{
    public interface IHelperOds
    {
        
        List<string> GetCoidList();
        List<BranchModel> GetBranchList(List<string> coidList);
        List<WarehouseModel> GetWarehousesForCoids(List<string> coidList, bool refreshCache);
        RefreshCacheResultModel RefreshWarehouseCache();
        List<BusinessUnitWarehouseModel> GetBusinessUnitWarehouses(List<string> coidList);
    }
}