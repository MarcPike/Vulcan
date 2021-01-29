using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Queries.PurchaseOrderItems;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class PurchaseOrderItemCalculatedCost
    {
        public List<PurchaseOrderItemsAdvancedQuery> PurchaseOrderItemsFound { get; set; }
        //public decimal CostPerInch => PurchaseOrderItemsFound.Where(x => x.CostPerInch > 0).Average(x => x.CostPerInch);
        //public decimal CostPerPound => PurchaseOrderItemsFound.Where(x => x.CostPerLb > 0).Average(x => x.CostPerLb);
        //public decimal CostPerKg => PurchaseOrderItemsFound.Where(x => x.CostPerKg > 0).Average(x => x.CostPerKg);
        public decimal TheoWeight => PurchaseOrderItemsFound.Max(x => x.TheoWeight);
        public string Status { get; set; }

        public PurchaseOrderItemCalculatedCost()
        {
        }

        public PurchaseOrderItemCalculatedCost(string coid, int productId)
        {

            var context = ContextFactory.GetPurchaseOrdersContextForCoid(coid);
            var purchaseOrderItemsAdvancedQuery = PurchaseOrderItemsAdvancedQuery.AsQueryable(coid, context);
            PurchaseOrderItemsFound = purchaseOrderItemsAdvancedQuery.Where(x => x.ProductId == productId && x.UnitValues.Balance.Weight > 0).ToList();

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