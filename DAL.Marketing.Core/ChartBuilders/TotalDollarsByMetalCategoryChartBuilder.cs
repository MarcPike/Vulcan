using DAL.Marketing.Core.ChartModels;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Extensions;
using DAL.Vulcan.Mongo.Core.Models;
using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Helpers;

namespace DAL.Marketing.Core.ChartBuilders
{
    public class TotalDollarsByMetalCategoryChartBuilder 
    {
        private class ChartValues
        {
            public string MetalCategory;
            public decimal Won;
            public decimal Total;
            public decimal Active;
            public decimal Expired;
            public decimal Lost;
        }

        private List<ChartValues> Values { get; } = new List<ChartValues>();

        public void Calculate(List<CrmQuote> quotes, string displayCurrency)
        {
            var helperCurrency = new HelperCurrencyForIMetal();

            foreach (var quote in quotes)
            {

                var items = quote.Items.Select(x => x.AsQuoteItem()).Where(x=>x.IsLost == false && x.IsQuickQuoteItem == false).ToList();
                foreach (var item in items)
                {
                    if (item.QuotePrice == null) continue;

                    var thisTotal = helperCurrency.ConvertValueFromCurrencyToCurrency(item.QuotePrice.FinalPrice, quote.DisplayCurrency, displayCurrency);
                    var thisCategory = item.QuotePrice.FinishedProduct.MetalCategory;

                    
                    var thisValue = Values.SingleOrDefault(x => x.MetalCategory == thisCategory);
                    if (thisValue == null)
                    {
                        thisValue = new ChartValues()
                        {
                            MetalCategory = thisCategory
                        };
                        Values.Add(thisValue);
                    }
                    thisValue.Total += thisTotal;
                    switch (quote.Status)
                    {
                        case PipelineStatus.Draft:
                            break;
                        case PipelineStatus.Submitted:
                            thisValue.Active += thisTotal;
                            break;
                        case PipelineStatus.Won:
                            thisValue.Won += thisTotal;
                            break;
                        case PipelineStatus.Loss:
                            if (!quote.Bid) thisValue.Lost += thisTotal;
                            break;
                        case PipelineStatus.Expired:
                            thisValue.Expired += thisTotal;
                            break;
                    }
                    
                }
            }
        }

        public TotalDollarsByMetalCategoryChartData GetChartData()
        {
            var result = new TotalDollarsByMetalCategoryChartData();
            foreach (var chartValue in Values.OrderByDescending(x=>x.Total))
            {
                //result.AddValue(chartValue.MetalCategory, "Total", chartValue.Total.RoundAndNormalize(2));
                result.AddValue(chartValue.MetalCategory, "Won", chartValue.Won.RoundAndNormalize(2));
                result.AddValue(chartValue.MetalCategory, "Active", chartValue.Active.RoundAndNormalize(2));
                result.AddValue(chartValue.MetalCategory, "Expired", chartValue.Expired.RoundAndNormalize(2));
                result.AddValue(chartValue.MetalCategory, "Lost", chartValue.Lost.RoundAndNormalize(2));
            }

            return result;
        }
    }

}