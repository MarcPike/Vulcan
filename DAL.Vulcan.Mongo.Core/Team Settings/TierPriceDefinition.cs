using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.TeamSettings;

namespace DAL.Vulcan.Mongo.Core.Team_Settings
{
    public class TierPriceDefinition 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string StockGrade { get; set; }
        public string ProductCondition { get; set; }

        public List<BasePriceDimension> BasePriceDimensions { get; set; } = new List<BasePriceDimension>();

        public TierPriceDefinitionModel Clone(string newStockGrade, string newProductCondition)
        {
            var newPriceDef = new TierPriceDefinition()
            {
                StockGrade = newStockGrade,
                ProductCondition = newProductCondition
            };

            foreach (var basePriceDimension in BasePriceDimensions)
            {
                newPriceDef.BasePriceDimensions.Add(new BasePriceDimension()
                {
                    BasePrice = basePriceDimension.BasePrice,
                    OuterDiameterMin = basePriceDimension.OuterDiameterMin,
                    OuterDiameterMax = basePriceDimension.OuterDiameterMax,
                    WeightDiscounts = basePriceDimension.WeightDiscounts.Select(x=> new WeightDiscount() {MinWeight = x.MinWeight, MaxWeight = x.MaxWeight, BasePriceFactor = x.BasePriceFactor}).ToList()
                });
            }

            return new TierPriceDefinitionModel(newPriceDef);
        }


    }
}