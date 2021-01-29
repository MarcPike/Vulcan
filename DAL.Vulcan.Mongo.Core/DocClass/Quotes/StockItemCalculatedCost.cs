using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Queries;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class StockItemCalculatedCost
    {
        public List<StockItemsQuery> StockItemsFound { get; set; } 
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

            var StockItemsFound = StockItemsQuery.GetForId(coid, productId).Where(x=> x.CostPerQty > 0).ToList();
            if (StockItemsFound.Count > 0)
            {
                var theoWeight = StockItemsFound.First().TheoWeight;

                Status = "Ok";
            }
            else
            {
                Status = $"No Data Found for this ProductId in {coid}";
            }

        }

    }
}