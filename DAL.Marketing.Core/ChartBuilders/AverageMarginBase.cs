using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Marketing.Core.ChartBuilders
{
    public class AverageMarginBase
    {
        public virtual void EvaluateData(CrmQuote quote, CrmQuoteItem quoteItem, PipelineStatus status,
            decimal materialMargin, decimal sellingMargin)
        {

        }

        public void Calculate(List<CrmQuote> quotes)
        {

            foreach (var quote in quotes)
            {
                var items = quote.Items.Select(x => x.AsQuoteItem()).Where(x => x.IsQuickQuoteItem == false).ToList();
                foreach (var quoteItem in items)
                {
                    var thisCategory = quote.Company.ShortName;

                    if (quoteItem.QuotePrice == null) continue;

                    var materialMargin = quoteItem.QuotePrice.MaterialPriceValue.Margin;
                    var sellingMargin = quoteItem.QuotePrice.Margin;

                    EvaluateData(quote, quoteItem, quote.Status, materialMargin, sellingMargin);

                }
            }


        }
    }
}