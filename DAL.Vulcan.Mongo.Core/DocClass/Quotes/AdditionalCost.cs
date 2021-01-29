using System;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class AdditionalCost
    {
        public Guid Id { get; set; }
        public string ResourceType { get; set; }
        public string PriceType { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalCost { get; set; } 
    }
}