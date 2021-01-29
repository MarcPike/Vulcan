using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Queries;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class ProductBucketHelper
    {
        public List<string> ProductCategories { get; set; } = new List<string>();
        public List<string> ProductConditions { get; set; } = new List<string>();
        public List<WarehouseQuery> WarehouseCodes { get; set; } = new List<WarehouseQuery>();
        public ProductBucketHelper(string coid)
        {

            ProductCategories.AddRange(ProductMastersQuery.GetAllProductCategories(coid).Where(x=>!x.StartsWith("MC")).Select(x=>x).Distinct().OrderBy(x => x));

            ProductConditions.AddRange(ProductMastersQuery.GetAllProductConditions(coid).Select(x => x).Distinct().OrderBy(x => x));

            WarehouseCodes.AddRange(WarehouseQuery.ExecuteAsync(coid).Result.OrderBy(x => x.Code));
        }
    }
}