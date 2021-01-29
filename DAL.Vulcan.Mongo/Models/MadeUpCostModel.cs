using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;

namespace DAL.Vulcan.Mongo.Models
{
    public class MadeUpCostModel
    {
        public string Coid { get; set; } = string.Empty;
        public string Application { get; set; }
        public string UserId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public decimal OuterDiameter { get; set; }
        public decimal InsideDiameter { get; set; }
        public decimal CostPerPound { get; set; } = 0;

        public decimal CostPerKilogram { get; set; } = 0;

        public OrderQuantity OrderQuantity { get; set; }
        public string ProductType { get; set; } 

        public decimal TheoWeight { get; set; } = 0;
        public string MetalCategory { get; set; }
        public string ProductCondition { get; set; }
        public string ProductCategory { get; set; }

        public ProductMaster NonStockItemProduct { get; set; } = null;
        public bool IsNewProduct { get; set; } = false;

        public string DisplayCurrency { get; set; } = string.Empty;

        public string CompanyId { get; set; }
        public QuoteSource QuoteSource { get; set; } = QuoteSource.MadeUpCost;

        public bool IsValid
        {
            get
            {
                if (NonStockItemProduct != null) return true;

                var result = false || 
                             (Coid != string.Empty) && 
                             (ProductCode != string.Empty) && 
                             ((CostPerKilogram + CostPerPound) > 0) &&
                             (TheoWeight > 0);
                return result;
            }
        }

        public MadeUpCostModel()
        {

        }

        public MadeUpCostModel(MadeUpCost madeUpCost)
        {
            Coid = madeUpCost.Coid;
            TheoWeight = madeUpCost.TheoWeight;
            CostPerKilogram = madeUpCost.CostPerKilogram;
            CostPerPound = madeUpCost.CostPerPound;
            ProductCode = madeUpCost.ProductCode;
            OuterDiameter = madeUpCost.OuterDiameter;
            InsideDiameter = madeUpCost.InsideDiameter;
            ProductType = madeUpCost.ProductType;
            MetalCategory = madeUpCost.MetalCategory;
            ProductCondition = madeUpCost.ProductCondition;
            NonStockItemProduct = madeUpCost.NonStockItemProduct;
            IsNewProduct = madeUpCost.IsNewProduct;
            DisplayCurrency = madeUpCost.DisplayCurrency;
            ProductCategory = madeUpCost.ProductCategory;
        }

        public MadeUpCost AsMadeUpCost()
        {
            if (!IsValid) return null;

            return new MadeUpCost()
            {
                Coid = Coid,
                TheoWeight = TheoWeight,
                CostPerPound = CostPerPound,
                CostPerKilogram = CostPerKilogram,
                ProductCode = ProductCode,
                InsideDiameter = InsideDiameter,
                OuterDiameter = OuterDiameter,
                ProductType = ProductType,
                MetalCategory = MetalCategory,
                ProductCondition = ProductCondition,
                NonStockItemProduct = NonStockItemProduct,
                IsNewProduct = IsNewProduct,
                DisplayCurrency = DisplayCurrency,
                QuoteSource =  QuoteSource,
                ProductCategory = ProductCategory
            };

        }

    }
}