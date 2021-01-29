using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Queries;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class PurchaseOrderItemCalculatedCost
    {
        public List<PurchaseOrderItemsQuery> PurchaseOrderItemsFound { get; set; }
        public decimal CostPerInch => PurchaseOrderItemsFound.Where(x => x.CostPerInch > 0).Average(x => x.CostPerInch);
        public decimal CostPerPound => PurchaseOrderItemsFound.Where(x => x.CostPerLb > 0).Average(x => x.CostPerLb);
        public decimal CostPerKg => PurchaseOrderItemsFound.Where(x => x.CostPerKg > 0).Average(x => x.CostPerKg);
        public decimal TheoWeight => PurchaseOrderItemsFound.Max(x => x.TheoWeight);
        public string Status { get; set; }

        public PurchaseOrderItemCalculatedCost()
        {
        }

        public PurchaseOrderItemCalculatedCost(string coid, int productId)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("ProductId", productId);
            PurchaseOrderItemsFound =  PurchaseOrderItemsQuery.ExecuteAsync(coid, parameters).Result.ToList();

            if (PurchaseOrderItemsFound.Count > 0)
            {
                Status = "Ok";
            }
            else
            {
                Status = $"No Data Found for this product in {coid}";
            }

        }

    }
}