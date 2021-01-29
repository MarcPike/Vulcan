using System.Collections.Generic;
using System.Linq;
using DAL.Marketing.Core.ChartModels;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Extensions;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Marketing.Core.ChartBuilders
{
    public class AverageMarginBySalesPersonChartBuilder : AverageMarginBase
    {
        private class ChartValues
        {
            public string SalesPerson;
            public MarginCollection Material = new MarginCollection();
            public MarginCollection Selling = new MarginCollection();
        }

        private List<ChartValues> Values { get; set; } = new List<ChartValues>();

        public override void EvaluateData(CrmQuote quote, CrmQuoteItem quoteItem, PipelineStatus status,
            decimal materialMargin,
            decimal sellingMargin)
        {

            var thisCategory = quote.SalesPerson.FullName;

            var thisCategoryValue = Values.FirstOrDefault(x => x.SalesPerson == thisCategory);

            if (thisCategoryValue == null)
            {
                thisCategoryValue = new ChartValues() {SalesPerson = thisCategory};
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

        public (MarginBySalesPersonChartData MaterialMargin, MarginBySalesPersonChartData SellingMargin) GetChartData()
        {
            var materialMargin = new MarginBySalesPersonChartData();
            var sellingMargin = new MarginBySalesPersonChartData();
            foreach (var chartValue in Values.OrderBy(x=>x.SalesPerson))
            {
                materialMargin.AddValue(chartValue.SalesPerson, "Won", chartValue.Material.Won.Margin.RoundAndNormalize(2));
                materialMargin.AddValue(chartValue.SalesPerson, "Active", chartValue.Material.Active.Margin.RoundAndNormalize(2));
                materialMargin.AddValue(chartValue.SalesPerson, "Expired", chartValue.Material.Expired.Margin.RoundAndNormalize(2));
                materialMargin.AddValue(chartValue.SalesPerson, "Lost", chartValue.Material.Lost.Margin.RoundAndNormalize(2));

                sellingMargin.AddValue(chartValue.SalesPerson, "Won", chartValue.Selling.Won.Margin.RoundAndNormalize(2));
                sellingMargin.AddValue(chartValue.SalesPerson, "Active", chartValue.Selling.Active.Margin.RoundAndNormalize(2));
                sellingMargin.AddValue(chartValue.SalesPerson, "Expired", chartValue.Selling.Expired.Margin.RoundAndNormalize(2));
                sellingMargin.AddValue(chartValue.SalesPerson, "Lost", chartValue.Selling.Lost.Margin.RoundAndNormalize(2));
            }

            return (materialMargin, sellingMargin);
        }
    }
}