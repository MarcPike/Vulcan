using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Extensions;
using DAL.Vulcan.Mongo.Quotes;
using Vulcan.IMetal.Helpers;
using Vulcan.IMetal.Queries.ProductCodes;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class MadeUpCost
    {
        public string Coid { get; set; }
        public string ProductCode { get; set; }
        public decimal CostPerPound { get; set; } = 0;
        public decimal CostPerKilogram { get; set; } = 0;
        public decimal TheoWeight { get; set; } = 0;
        public decimal InsideDiameter { get; set; } = 0;
        public decimal OuterDiameter { get; set; }
        public string ProductType { get; set; } = "Bar";
        public string MetalCategory { get; set; }
        public string ProductCondition { get; set; }
        public string ProductCategory { get; set; } = string.Empty;
        public bool IsNewProduct { get; set; } = true;
        public string StockGrade { get; set; } = string.Empty;

        public OrderQuantity OrderQuantity { get; set; }
        public ProductMaster NonStockItemProduct { get; set; }
        public string DisplayCurrency { get; set; }

        public QuoteSource QuoteSource { get; set; } = QuoteSource.MadeUpCost;

        public bool IsValid
        {
            get
            {
                var result = false ||
                             (Coid != string.Empty) &&
                             (ProductCode != string.Empty) &&
                             ((CostPerKilogram + CostPerPound) > 0) &&
                             (TheoWeight > 0);
                return result;
            }
        }

        public (BaseCost BaseCost, ProductMaster ProductMaster) ConvertToCostValues(OrderQuantity orderQuantity)
        {
            var costPerPound = (CostPerPound > 0) ? CostPerPound : CostPerKilogram * (decimal) 2.20462;
            var costPerKilogram = (CostPerKilogram > 0) ? CostPerKilogram : CostPerPound * (decimal) 0.453592;

            var baseCost = BaseCost.FromMadeUpCost(Coid, this, orderQuantity, DisplayCurrency);

            var productMaster = new ProductMaster()
            {
                TheoWeight = TheoWeight,
                Coid = Coid,
                OuterDiameter = OuterDiameter,
                ProductCode = ProductCode,
                InsideDiameter = InsideDiameter,
                ProductId = 0,
                ProductType = ProductType,
                MetalCategory = MetalCategory,
                ProductCondition = ProductCondition,
                FactorForLbs = UomHelper.GetFactorForPounds(Coid),
                FactorForKilograms = UomHelper.GetFactorForKilograms(Coid),
                StockGrade = StockGrade
            };

            if (NonStockItemProduct != null)
            {
                productMaster = NonStockItemProduct;
            }

            return (baseCost, productMaster);
        }

        public static (decimal TotalPounds, decimal TotalKilograms) CalculateEstimatedWeight(decimal outerDiameter,
            decimal insideDiameter, string metalCategory, OrderQuantity orderQuantity)
        {
            decimal totalPounds = 0;
            decimal totalKilograms = 0;

            var productType = (insideDiameter > 0) ? "Tube" : "Bar";
            var wall = (insideDiameter > 0) ? ((outerDiameter - insideDiameter) / 2) : 0;
            decimal theoWeight = 0;
            if (metalCategory == "NICKEL")
            {
                if (productType == "Bar")
                {
                    theoWeight = ((outerDiameter * outerDiameter) * (decimal)0.22274) * (decimal)1.05;
                }
                else if (productType == "Tube")
                {
                    //theoWeight = ((outerDiameter - insideDiameter) * wall * (decimal) 0.89) + (decimal) 1.05;
                    theoWeight = ((outerDiameter - wall) * wall * (decimal)0.89) * (decimal)1.05;
                }
            }
            else
            {
                if (productType == "Bar")
                {
                    theoWeight = ((outerDiameter * outerDiameter) * (decimal)0.22274);
                }
                else if (productType == "Tube")
                {
                    //theoWeight = ((outerDiameter - insideDiameter) * wall * (decimal)0.89);
                    theoWeight = (outerDiameter - wall) * wall * (decimal)0.89;
                }

            }

            var totalInches = orderQuantity.GetTotalInches() * orderQuantity.Pieces;
            totalPounds = totalInches * theoWeight;
            totalKilograms = totalPounds * (decimal)0.453592;

            return (totalPounds.RoundAndNormalize(2), totalKilograms.RoundAndNormalize(2));
        }


        /*
            Alloy Solid Bar: OD2 * 0.22274
            Alloy Tube OD – ID * Wall * 0.89
            Nickel Solid Bar: OD2 * 2.6729 + 4%
            Nickel Tube: OD – ID * Wall * 0.89 + 4%
         */
        public static MadeUpCost CreateNew(string coid, decimal outerDiameter, decimal insideDiameter, string metalCategory, string productCondition, decimal costPerPound, decimal costPerKilogram, OrderQuantity orderQuantity, string productCode, string displayCurrency, string productCategory, string stockGrade)
        {
            costPerPound = (costPerPound > 0) ? costPerPound : costPerKilogram * (decimal)2.20462;
            costPerKilogram = (costPerKilogram > 0) ? costPerKilogram : costPerPound * (decimal)0.453592;

            var productType = (insideDiameter > 0) ? "Tube" : "Bar";
            var wall = (insideDiameter > 0) ? ((outerDiameter - insideDiameter) / 2) : 0;
            decimal theoWeight = 0;
            if (metalCategory == "NICKEL")
            {
                if (productType == "Bar")
                {
                    theoWeight = ((outerDiameter * outerDiameter) * (decimal)0.22274) * (decimal)1.05;
                }
                else if (productType == "Tube")
                {
                    //theoWeight = ((outerDiameter - insideDiameter) * wall * (decimal) 0.89) + (decimal) 1.05;
                    theoWeight = ((outerDiameter - wall) * wall * (decimal)0.89) * (decimal)1.05;
                }
            }
            else
            {
                if (productType == "Bar")
                {
                    theoWeight = ((outerDiameter * outerDiameter) * (decimal)0.22274);
                }
                else if (productType == "Tube")
                {
                    //theoWeight = ((outerDiameter - insideDiameter) * wall * (decimal)0.89);
                    theoWeight = (outerDiameter - wall) * wall * (decimal)0.89;
                }

            }

            return new MadeUpCost()
            {
                Coid = coid,
                TheoWeight = theoWeight,
                CostPerPound = costPerPound,
                CostPerKilogram = costPerKilogram,
                OrderQuantity = orderQuantity,
                OuterDiameter = outerDiameter,
                InsideDiameter = insideDiameter,
                ProductType = productType,
                ProductCode = productCode ?? "<NEW PRODUCT>",
                MetalCategory = metalCategory,
                ProductCondition = productCondition,
                ProductCategory = productCategory,
                StockGrade = stockGrade,
                
                IsNewProduct = true,
                DisplayCurrency = displayCurrency,
                QuoteSource = QuoteSource.MadeUpCost
            };

        }

        public static MadeUpCost FromNonStockItem(string coid, int productId, decimal costPerPound, decimal costPerKilogram, OrderQuantity orderQuantity, string displayCurrency)
        {
            //var nonStockProduct = ProductMasterAdvancedQuery.AsQueryable(coid, null)
            //    .SingleOrDefault(x => x.ProductId == productId);
            var nonStockProduct = new ProductMaster(coid, productId);

            if (nonStockProduct == null) return null;

            return new MadeUpCost()
            {
                Coid = nonStockProduct.Coid,
                TheoWeight = nonStockProduct.TheoWeight,
                CostPerPound = costPerPound,
                CostPerKilogram = costPerKilogram,
                OrderQuantity = orderQuantity,
                OuterDiameter = nonStockProduct.OuterDiameter,
                InsideDiameter = nonStockProduct.InsideDiameter,
                ProductType = nonStockProduct.ProductType,
                ProductCode = nonStockProduct.ProductCode,
                MetalCategory = nonStockProduct.MetalCategory,
                ProductCondition = nonStockProduct.ProductCondition,
                NonStockItemProduct = nonStockProduct,
                DisplayCurrency = displayCurrency,
                StockGrade = nonStockProduct.StockGrade,
                IsNewProduct = false,
                QuoteSource = QuoteSource.NonStockItem
            };
        }

    }
}