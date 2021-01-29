using System;

namespace DAL.Vulcan.Mongo.TeamSettings
{
    public class WeightDiscount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal MinWeight { get; set; } 
        public decimal MaxWeight { get; set; }
        public decimal BasePriceFactor { get; set; }

        
    }
}