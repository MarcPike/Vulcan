using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.Quotes;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class ProductionStepCostBase
    {
        public string Coid { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public ProductMaster StartingProduct { get; set; }
        public ProductMaster FinishedProduct { get; set; }
        public RequiredQuantity RequiredQuantity { get; set; }
        public bool IsPriceBlended { get; set; } = true;
        public List<CostValue> CostValues { get; set; }
        public ResourceType ResourceType { get; set; } = ResourceType.Unknown;
        public string ResourceTypeName => ResourceType.ToString();
        public RequiredQuantity FinishQuantity => (RequiredQuantity == null || FinishedProduct == null ) ? null : RequiredQuantity.CalculateNewQuantityForNewProduct(RequiredQuantity, FinishedProduct);

        public decimal ProductionCost
        {
            get
            {
                if (RequiredQuantity == null) return 0;
                foreach (var costValue in CostValues)
                {
                    costValue.TotalPounds = StartingPounds;
                    costValue.TotalInches = TotalInches; 
                    costValue.TotalPieces = RequiredQuantity.Pieces;
                }

                return CostValues.Sum(x => x.TotalProductionCost);
            }
        }

        public decimal InternalCost
        {
            get
            {
                foreach (var costValue in CostValues)
                {
                    costValue.TotalPounds = StartingPounds;
                    costValue.TotalInches = TotalInches;
                    costValue.TotalPieces = RequiredQuantity.Pieces;
                }

                return CostValues.Sum(x => x.TotalInternalCost);
            }
        }

        public decimal ProductionPrice { get; set; }

        public decimal ProductionPriceOverride { get; set; } = 0;
        public decimal ProductionPriceMargin { get; set; }
        public decimal ProductionPriceMarginOverride { get; set; }

        public decimal TotalInches { get; set; } = 0;
        public decimal StartingPounds => (TotalInches * StartingProduct?.TheoWeight * StartingProduct?.FactorForLbs) ?? 0;
//        public decimal FinishPounds => FinishQuantity?.TotalPounds() ?? 0;
        public decimal FinishPounds => (TotalInches * FinishedProduct?.TheoWeight * FinishedProduct?.FactorForLbs) ?? 0;
        public decimal TotalFeet => FinishQuantity?.TotalFeet() ?? 0;

        public decimal StartingKilograms => StartingPounds * (decimal)0.453592;
        public decimal FinishKilograms => FinishPounds * (decimal)0.453592;

        public ProductionStepCostBase()
        {

        }

        public void CalculateMarginTotals()
        {
            ProductionPrice = ProductionCost;

            OverridePriceMargin();
            OverridePrice();

        }


        private void OverridePrice()
        {
            if (ProductionPriceOverride > 0)
            {
                ProductionPrice = ProductionPriceOverride;
                ProductionPriceMargin = QuoteCalculations.GetMargin(ProductionCost, ProductionPrice);
            }
        }

        private void OverridePriceMargin()
        {
            if (ProductionPriceMarginOverride > 0)
            {
                if (ProductionPriceMarginOverride >= 1)
                    ProductionPriceMarginOverride = ProductionPriceMarginOverride * (decimal) .01;
                ProductionPriceMargin = ProductionPriceMarginOverride;
                ProductionPrice = QuoteCalculations.GetSalePriceFromMargin(ProductionCost, ProductionPriceMargin);
            }
            else
            {
                ProductionPriceMargin = 0;
            }
        }

        public ProductionStepCostBase(string coid, string startingProductCode, string finishedProductCode, RequiredQuantity requiredQuantity, ResourceType resourceType, bool isPriceBlended)
        {
            Coid = coid;
            StartingProduct = new ProductMaster(coid, startingProductCode);
            FinishedProduct = new ProductMaster(coid, finishedProductCode);
            RequiredQuantity = requiredQuantity;
            ResourceType = resourceType;
            IsPriceBlended = isPriceBlended;
        }


    }

    //public class ManufacturingCost : ProductionStepCostBase
    //{

    //}

}