using System;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.TeamSettings
{
    public class BasePriceDimension
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal OuterDiameterMin { get; set; }
        public decimal OuterDiameterMax { get; set; }
        public decimal BasePrice { get; set; }

        public List<WeightDiscount> WeightDiscounts { get; set; } = new List<WeightDiscount>();

        public BasePriceDimension()
        {
            
        }
    }
}