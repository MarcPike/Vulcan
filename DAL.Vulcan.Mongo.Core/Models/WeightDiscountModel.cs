using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.TeamSettings;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class WeightDiscountModel
    {
        public string Id { get; set; } 
        public decimal MinWeight { get; set; }
        public decimal MaxWeight { get; set; }
        public decimal BasePriceFactor { get; set; }

        public WeightDiscountModel()
        {
        }

        public WeightDiscountModel(WeightDiscount disc)
        {
            Id = disc.Id.ToString();
            MinWeight = disc.MinWeight;
            MaxWeight = disc.MaxWeight;
            BasePriceFactor = disc.BasePriceFactor;
        }

    }
}
