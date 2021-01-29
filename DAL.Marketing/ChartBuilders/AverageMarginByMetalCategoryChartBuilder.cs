using System.Collections.Generic;
using System.Linq;
using DAL.Marketing.ChartModels;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Extensions;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Marketing.ChartBuilders
{
    public class AverageMarginByMetalCategoryChartBuilder : AverageMarginBase
    {
        private class ChartValues
        {
            public string MetalCategory { get; set; }
            public readonly MarginCollection Material = new MarginCollection();
            public readonly MarginCollection Selling = new MarginCollection();
        }

        private List<ChartValues> Values { get; set; } = new List<ChartValues>();

        public override void EvaluateData(CrmQuote quote, CrmQuoteItem quoteItem, PipelineStatus status,
            decimal materialMargin,
            decimal sellingMargin)
        {

            var thisCategory = quoteItem.QuotePrice.FinishedProduct.MetalCategory;

            var thisCategoryValue = Values.FirstOrDefault(x => x.MetalCategory == thisCategory);

            if (thisCategoryValue == null)
            {
                thisCategoryValue = new ChartValues() {MetalCategory = thisCategory};
                Values.Add(thisCategoryValue);
            }

            switch (status)
            {
                case PipelineStatus.Submitted:
                    thisCategoryValue.Material.Active.Add(materialMargin);
                    thisCategoryValue.Selling.Active.Add(sellingMargin);
                    break;
                case PipelineStatus.Won:
                    thisCategoryValue.Material.Won.Add(materialMargin);
                    thisCategoryValue.Selling.Won.Add(sellingMargin);
                    break;
                case PipelineStatus.Loss:
                    if (!quote.Bid)
                    {
                        thisCategoryValue.Material.Lost.Add(materialMargin);
                        thisCategoryValue.Selling.Lost.Add(sellingMargin);
                    }
                    break;
                case PipelineStatus.Expired:
                    thisCategoryValue.Material.Expired.Add(materialMargin);
                    thisCategoryValue.Selling.Expired.Add(sellingMargin);
                    break;
            }

        }

        public (MarginByMetalCategoryChartData MaterialMargin, MarginByMetalCategoryChartData SellingMargin) GetChartData()
        {
            var materialMargin = new MarginByMetalCategoryChartData();
            var sellingMargin = new MarginByMetalCategoryChartData();
            foreach (var chartValue in Values.OrderBy(x=>x.MetalCategory))
            {
                materialMargin.AddValue(chartValue.MetalCategory, "Won", chartValue.Material.Won.Margin.RoundAndNormalize(2));
                materialMargin.AddValue(chartValue.MetalCategory, "Active", chartValue.Material.Active.Margin.RoundAndNormalize(2));
                materialMargin.AddValue(chartValue.MetalCategory, "Expired", chartValue.Material.Expired.Margin.RoundAndNormalize(2));
                materialMargin.AddValue(chartValue.MetalCategory, "Lost", chartValue.Material.Lost.Margin.RoundAndNormalize(2));

                sellingMargin.AddValue(chartValue.MetalCategory, "Won", chartValue.Selling.Won.Margin.RoundAndNormalize(2));
                sellingMargin.AddValue(chartValue.MetalCategory, "Active", chartValue.Selling.Active.Margin.RoundAndNormalize(2));
                sellingMargin.AddValue(chartValue.MetalCategory, "Expired", chartValue.Selling.Expired.Margin.RoundAndNormalize(2));
                sellingMargin.AddValue(chartValue.MetalCategory, "Lost", chartValue.Selling.Lost.Margin.RoundAndNormalize(2));
            }

            return (materialMargin, sellingMargin);
        }

    }


}