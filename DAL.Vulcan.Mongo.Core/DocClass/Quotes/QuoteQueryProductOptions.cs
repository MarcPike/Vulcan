using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Queries;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class QuoteQueryProductOptions
    {
        public List<string> CoidList { get; set; } = new List<string>();
        public List<ProductMaster> Products { get; set; } = new List<ProductMaster>();
        public List<string> ProductCategories { get; set; } = new List<string>();
        public List<string> StockGrades { get; set; } = new List<string>();
        public List<string> MetalCategories { get; set; } = new List<string>();
        public List<string> ProductTypes { get; set; } = new List<string>();
        public QuoteQueryRange<decimal> OuterDiameterRange { get; set; } = new QuoteQueryRange<decimal>();
        public QuoteQueryRange<decimal> InsideDiameterRange { get; set; } = new QuoteQueryRange<decimal>();
        public QuoteQueryRange<decimal> LengthRange { get; set; } = new QuoteQueryRange<decimal>();
        public QuoteQueryRange<decimal> WidthRange { get; set; } = new QuoteQueryRange<decimal>();
        public QuoteQueryRange<decimal> ThickRange { get; set; } = new QuoteQueryRange<decimal>();

        public bool IsUsed => CoidList.Any() ||
                              Products.Any() ||
                              ProductCategories.Any() ||
                              StockGrades.Any() ||
                              MetalCategories.Any() ||
                              ProductTypes.Any() ||
                              OuterDiameterRange.IsUsed ||
                              InsideDiameterRange.IsUsed ||
                              LengthRange.IsUsed ||
                              ThickRange.IsUsed ||
                              WidthRange.IsUsed;
        public bool QuoteItemPassed(CrmQuoteItem crmQuoteItem)
        {
            if (!IsUsed) return true;

            var startingProduct = crmQuoteItem.QuotePrice.StartingProduct;
            var finishedProduct = crmQuoteItem.QuotePrice.FinishedProduct;

            if (CoidList.Any())
            {
                if ((!CoidList.Any(x => x.Equals(startingProduct.Coid))) && (!CoidList.Any(x => x.Equals(finishedProduct.Coid))))
                {
                    return false;
                }
            }

            if (Products.Any())
            {
                if ((Products.All(x => x.ProductId != startingProduct.ProductId)) && (Products.All(x => x.ProductId != finishedProduct.ProductId)))
                {
                    return false;
                }

            }

            var parameters = new Dictionary<string,object>();


            var stockItemStart = ProductMastersQuery.GetForId(startingProduct.Coid, startingProduct.ProductId);
            var stockItemFinish = ProductMastersQuery.GetForId(finishedProduct.Coid, finishedProduct.ProductId);

            var stockItems = new List<ProductMastersQuery>();
            if (stockItemStart != null) stockItems.Add(stockItemStart);
            if ((stockItemFinish != null) && (stockItemStart.ProductId != stockItemFinish.ProductId))
            {
                stockItems.Add(stockItemFinish);
            }

            // Assume this quote is using madeup cost
            if ((stockItemStart == null) && (stockItemFinish == null)) return false;

            var productCategoryPassed = !ProductCategories.Any();
            var stockGradesPassed = !StockGrades.Any();
            var metalCategoriesPassed = !MetalCategories.Any();
            var productTypesPassed = !ProductTypes.Any();
            var outerDiameterRangePassed = !OuterDiameterRange.IsUsed;
            var insideDiameterRangePassed = !InsideDiameterRange.IsUsed;
            var lengthRangePassed = !LengthRange.IsUsed;
            var widthRangePassed = !WidthRange.IsUsed;
            var thickRangePassed = !ThickRange.IsUsed;

            // Should only have one or two
            foreach (var stockItem in stockItems)
            {
                if (!productCategoryPassed)
                {
                    if (ProductCategories.Any(x => stockItem.ProductCategory.ToUpper().Contains(x)))
                    {
                        productCategoryPassed = true;
                    }
                }

                if (!stockGradesPassed)
                {
                    if (StockGrades.Any(x => stockItem.StockGrade.ToUpper().Contains(x)))
                    {
                        stockGradesPassed = true;
                    }
                }

                if (!metalCategoriesPassed)
                {
                    if (MetalCategories.Any(x => stockItem.MetalCategory.ToUpper().Contains(x)))
                    {
                        metalCategoriesPassed = true;
                    }
                }

                if (!productTypesPassed)
                {
                    if (ProductTypes.Any(x => stockItem.ProductType.ToUpper().Contains(x)))
                    {
                        productTypesPassed = true;
                    }

                }

                if (!outerDiameterRangePassed)
                {
                    if (stockItem.OuterDiameter >= OuterDiameterRange.Min &&
                        stockItem.OuterDiameter <= OuterDiameterRange.Max)
                    {
                        outerDiameterRangePassed = true;
                    }
                }

                if (!insideDiameterRangePassed)
                {
                    if (stockItem.InsideDiameter >= InsideDiameterRange.Min &&
                        stockItem.InsideDiameter <= InsideDiameterRange.Max)
                    {
                        insideDiameterRangePassed = true;
                    }
                }

                if (!lengthRangePassed)
                {
                    if (stockItem.Length >= LengthRange.Min &&
                        stockItem.Length <= LengthRange.Max)
                    {
                        lengthRangePassed = true;
                    }
                }

                if (!widthRangePassed)
                {
                    if (stockItem.Width >= WidthRange.Min &&
                        stockItem.Width <= WidthRange.Max)
                    {
                        widthRangePassed = true;
                    }
                }

                if (!thickRangePassed)
                {
                    if (stockItem.Thick >= ThickRange.Min &&
                        stockItem.Thick <= ThickRange.Max)
                    {
                        thickRangePassed = true;
                    }
                }

            }

            return (productCategoryPassed && productTypesPassed && metalCategoriesPassed && stockGradesPassed &&
                    insideDiameterRangePassed && outerDiameterRangePassed && lengthRangePassed && widthRangePassed && thickRangePassed);
        }
    }

}