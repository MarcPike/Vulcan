using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Quotes;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Queries.StockItems;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class StockItemCalculatedCost
    {
        public List<StockItemsAdvancedQuery> StockItemsFound { get; set; } 
        public decimal CostPerInch => StockItemsFound.Where(x => x.CostPerInch > 0).Average(x => x.CostPerInch);
        public decimal CostPerPound => StockItemsFound.Where(x => x.CostPerLb > 0).Average(x => x.CostPerLb);
        public decimal CostPerKg => StockItemsFound.Where(x => x.CostPerKg > 0).Average(x => x.CostPerKg);
        public decimal TheoWeight => StockItemsFound.Max(x => x.TheoWeight);
        public decimal InsideDiameter => StockItemsFound.Max(x => x.InsideDiameter);
        public decimal OuterDiameter => StockItemsFound.Max(x => x.OuterDiameter);
        public string Status { get; set; }

        public StockItemCalculatedCost()
        {
        }

        public StockItemCalculatedCost(string coid, int productId)
        {

            var context = ContextFactory.GetStockItemsContextForCoid(coid);
            var stockItemsAdvancedQuery = StockItemsAdvancedQuery.AsQueryable(coid, context);
            StockItemsFound = stockItemsAdvancedQuery.Where(x => x.ProductId == productId && x.CostPerQty > 0).ToList();

            if (StockItemsFound.Count > 0)
            {
                var theoWeight = StockItemsFound.First().TheoWeight;

                Status = "Ok";
            }
            else
            {
                Status = $"No Data Found for this StartingProductCode in {coid}";
            }

        }

    }
}