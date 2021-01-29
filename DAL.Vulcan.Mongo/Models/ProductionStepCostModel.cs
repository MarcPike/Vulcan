using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;

namespace DAL.Vulcan.Mongo.Models
{
    public class ProductionStepCostModel
    {
        public string Coid { get; set; }
        public Guid Id { get; set; } 
        public ProductMaster StartingProduct { get; set; } = new ProductMaster();
        public ProductMaster FinishedProduct { get; set; } = new ProductMaster();
        public RequiredQuantity RequiredQuantity { get; set; } = new RequiredQuantity();

        public RequiredQuantity FinishQuantity { get; set; }

        public bool IsPriceBlended { get; set; }
        public List<CostValueModel> CostValues { get; set; }
        public ResourceType ResourceType { get; set; } 

        public decimal ProductionCost { get; set; }
            
        public decimal ProductionPrice { get; set; }

        public decimal ProductionPriceOverride { get; set; } = 0;
        public decimal ProductionPriceMargin { get; set; }
        public decimal ProductionPriceMarginOverride { get; set; }

        public decimal TestPriceOverride { get; set; } = 0;
        public decimal TestPriceMarginOverride { get; set; } = 0;

        public decimal TestPrice { get; set; }
        public decimal TestPriceMargin { get; set; }

        public decimal TotalInches { get; set; }
        public decimal StartingPounds { get; set; }
        public decimal FinishPounds { get; set; }

        public decimal StartingKilograms;
        public decimal FinishKilograms;

        public string ResourceTypeName => ResourceType.ToString();

        public ProductionStepCostModel()
        {
        }
        public ProductionStepCostModel(ProductionStepCostBase step, decimal totalInches)
        {
            Id = step.Id;
            StartingProduct = step.StartingProduct;
            FinishedProduct = step.FinishedProduct;
            RequiredQuantity = step.RequiredQuantity;
            IsPriceBlended = step.IsPriceBlended;
            CostValues = step.CostValues.Select(x=> new CostValueModel(x)).ToList();
            ResourceType = step.ResourceType;
            Coid = step.Coid;
            ProductionPrice = step.ProductionPrice;
            ProductionPriceOverride = step.ProductionPriceOverride;
            ProductionPriceMargin = step.ProductionPriceMargin;
            ProductionPriceMarginOverride = step.ProductionPriceMarginOverride;
            FinishQuantity = step.FinishQuantity;
            StartingPounds = step.StartingPounds;
            FinishPounds = step.FinishPounds;
            TotalInches = step.TotalInches;
            ProductionCost = step.ProductionCost;
            StartingKilograms = step.StartingKilograms;
            FinishKilograms = step.FinishKilograms;
        }
    }
}
