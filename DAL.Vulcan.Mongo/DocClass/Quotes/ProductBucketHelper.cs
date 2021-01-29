using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Queries.ProductCodes;
using Vulcan.IMetal.Queries.StockItems;
using Vulcan.IMetal.Queries.Warehouses;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class ProductBucketHelper
    {
        public List<string> ProductCategories { get; set; } = new List<string>();
        public List<string> ProductConditions { get; set; } = new List<string>();
        public List<WarehousesAdvancedQuery> WarehouseCodes { get; set; } = new List<WarehousesAdvancedQuery>();
        public ProductBucketHelper(string coid)
        {

            ProductCategories.AddRange(ProductMasterAdvancedQuery.AsQueryable(coid).Where(x=>!x.ProductCategory.StartsWith("MC")).Select(x=>x.ProductCategory).Distinct().OrderBy(x => x));

            ProductConditions.AddRange(ProductMasterAdvancedQuery.AsQueryable(coid).Select(x => x.ProductCondition).Distinct().OrderBy(x => x));

            WarehouseCodes.AddRange(WarehousesAdvancedQuery.GetForCoid(coid).OrderBy(x => x.Code));
        }
    }
}