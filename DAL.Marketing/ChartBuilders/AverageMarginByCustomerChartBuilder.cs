using System.Collections.Generic;
using System.Linq;
using DAL.Marketing.ChartModels;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Extensions;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Marketing.ChartBuilders
{
    public class MarginCollection
    {
        public MarginCalculator Won = new MarginCalculator();
        public MarginCalculator Lost = new MarginCalculator();
        public MarginCalculator Expired = new MarginCalculator();
        public MarginCalculator Active = new MarginCalculator();
    }

    public class AverageMarginByCustomerChartBuilder: AverageMarginBase
    {
        private class ChartValuesMargin
        {
            public string Customer;

            public readonly MarginCollection Material = new MarginCollection();
            public readonly MarginCollection Selling = new MarginCollection();
            public string SortOn { get; set; }
        }

        private List<ChartValuesMargin> Values { get; set; } = new List<ChartValuesMargin>();

        public override void EvaluateData(CrmQuote quote, CrmQuoteItem quoteItem, PipelineStatus status, decimal materialMargin,
            decimal sellingMargin)
        {
            var thisCategory = quote.Company.Code + " " + quote.Company.ShortName;
            var sortOn = quote.Company.ShortName;

            var thisCategoryValue = Values.FirstOrDefault(x => x.Customer == thisCategory);

            if (thisCategoryValue == null)
            {
                thisCategoryValue = new ChartValuesMargin() { Customer = thisCategory, SortOn = sortOn };
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

        public (MarginByCustomerChartData MaterialMargins, MarginByCustomerChartData SellingMargins) GetChartData()
        {
            var materialMargin = new MarginByCustomerChartData();
            var sellingMargin = new MarginByCustomerChartData();
            foreach (var chartValue in Values.OrderBy(x=>x.SortOn))
            {
                materialMargin.AddValue(chartValue.Customer, "Won", chartValue.Material.Won.Margin.RoundAndNormalize(2));
                materialMargin.AddValue(chartValue.Customer, "Active", chartValue.Material.Active.Margin.RoundAndNormalize(2));
                materialMargin.AddValue(chartValue.Customer, "Expired", chartValue.Material.Expired.Margin.RoundAndNormalize(2));
                materialMargin.AddValue(chartValue.Customer, "Lost", chartValue.Material.Lost.Margin.RoundAndNormalize(2));

                sellingMargin.AddValue(chartValue.Customer, "Won", chartValue.Selling.Won.Margin.RoundAndNormalize(2));
                sellingMargin.AddValue(chartValue.Customer, "Active", chartValue.Selling.Active.Margin.RoundAndNormalize(2));
                sellingMargin.AddValue(chartValue.Customer, "Expired", chartValue.Selling.Expired.Margin.RoundAndNormalize(2));
                sellingMargin.AddValue(chartValue.Customer, "Lost", chartValue.Selling.Lost.Margin.RoundAndNormalize(2));
            }

            return (materialMargin,sellingMargin);
        }
    }
}