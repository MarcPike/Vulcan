using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.TeamSettings
{
    public class ProductWeightTierPrice
    {
        public string ProductCode { get; set; }
        public List<TierWeightPrices> TierWeightPrices { get; set; } = new List<TierWeightPrices>();

        public decimal GetPriceForWeight(decimal weight)
        {
            var weightMatch = TierWeightPrices.FirstOrDefault(x => x.MinWeight <= weight && x.MaxWeight >= weight);
            if (weightMatch != null)
            {
                return weightMatch.Price;
            }

            return 0;
        }

    }
}