using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.Team_Settings;
using DAL.Vulcan.Mongo.Core.TeamSettings;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class TierPriceDefinitionModel
    {
        public string Id { get; set; } 
        public string StockGrade { get; set; }
        public string ProductCondition { get; set; }
        public bool IsDirty { get; set; } = false;
        
        public List<BasePriceDimensionModel> BasePriceDimensions { get; set; } = new List<BasePriceDimensionModel>();

        public TierPriceDefinitionModel()
        {
        }

        public TierPriceDefinitionModel(TierPriceDefinition priceDef)
        {
            Id = priceDef.Id.ToString();
            StockGrade = priceDef.StockGrade;
            ProductCondition = priceDef.ProductCondition;
            BasePriceDimensions = priceDef.BasePriceDimensions.Select(x => new BasePriceDimensionModel(x)).OrderBy(x=>x.OuterDiameterMin).ToList();
        }

    }
}
