using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.TeamSettings;

namespace DAL.Vulcan.Mongo.Models
{
    public class BasePriceDimensionModel
    {
        public string Id { get; set; } 

        public decimal OuterDiameterMin { get; set; }
        public decimal OuterDiameterMax { get; set; }
        public decimal BasePrice { get; set; }

        public List<WeightDiscountModel> WeightDiscounts { get; set; } = new List<WeightDiscountModel>();

        public BasePriceDimensionModel()
        {
        }

        public BasePriceDimensionModel(BasePriceDimension dim)
        {
            Id = dim.Id.ToString();
            OuterDiameterMin = dim.OuterDiameterMin;
            OuterDiameterMax = dim.OuterDiameterMax;
            BasePrice = dim.BasePrice;
            WeightDiscounts = dim.WeightDiscounts.Select(x => new WeightDiscountModel(x)).OrderBy(x=>x.MinWeight).ToList();
        }

    }
}
