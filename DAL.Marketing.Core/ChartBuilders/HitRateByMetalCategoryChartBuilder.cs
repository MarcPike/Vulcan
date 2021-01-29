using System.Collections.Generic;
using System.Linq;
using DAL.Marketing.Core.ChartModels;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Marketing.Core.ChartBuilders
{
    public class HitRateByMetalCategoryChartBuilder 
    {
        private class ChartValues
        {
            public string MetalCategory;
            public int Won = 0;
            public int Total = 0;
            public int Active = 0;
            public int Lost = 0;
            public int Expired = 0;
        }

        private List<ChartValues> Values { get; } = new List<ChartValues>();

        public void Calculate(List<CrmQuote> quotes)
        {

            foreach (var quote in quotes)
            {

                var items = quote.Items.Select(x => x.AsQuoteItem()).Where(x => x.IsLost == false && x.IsQuickQuoteItem == false).ToList();
                foreach (var quoteItem in items)
                {
                    if (quoteItem.QuotePrice == null) continue;
                    
                    var thisCategory = quoteItem.QuotePrice.FinishedProduct.MetalCategory;


                    var thisValue = Values.SingleOrDefault(x => x.MetalCategory == thisCategory);
                    if (thisValue == null)
                    {
                        thisValue = new ChartValues()
                        {
                            MetalCategory = thisCategory
                        };
                        Values.Add(thisValue);
                    }
                    thisValue.Total += 1;

                    switch (quote.Status)
                    {
                        case PipelineStatus.Draft:
                            break;
                        case PipelineStatus.Submitted:
                            thisValue.Active += 1;
                            break;
                        case PipelineStatus.Won:
                            thisValue.Won += 1;
                            break;
                        case PipelineStatus.Loss:
                            if (!quote.Bid) thisValue.Lost += 1;
                            break;
                        case PipelineStatus.Expired:
                            thisValue.Expired += 1;
                            break;
                    }

                }
            }
        }

        public HitRateByMetalCategoryChartData GetChartData()
        {
            var result = new HitRateByMetalCategoryChartData();
            foreach (var chartValue in Values.OrderByDescending(x=>x.Total))
            {
                result.AddValue(chartValue.MetalCategory, "Won", chartValue.Won);
                result.AddValue(chartValue.MetalCategory, "Active", chartValue.Active);
                result.AddValue(chartValue.MetalCategory, "Expired", chartValue.Expired);
                result.AddValue(chartValue.MetalCategory, "Lost", chartValue.Lost);
            }

            return result;
        }
    }

}