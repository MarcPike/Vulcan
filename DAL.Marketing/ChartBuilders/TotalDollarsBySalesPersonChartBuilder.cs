using System.Collections.Generic;
using System.Linq;
using DAL.Marketing.ChartModels;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Extensions;
using DAL.Vulcan.Mongo.Models;
using Vulcan.IMetal.Helpers;

namespace DAL.Marketing.ChartBuilders
{
    public class TotalDollarsBySalesPersonChartBuilder 
    {
        private struct ChartValues
        {
            public string SalesPerson;
            public decimal Won;
            public decimal Total;
            public decimal Active;
            public decimal Expired;
            public decimal Lost;
        }

        private List<ChartValues> Values { get; set; } = new List<ChartValues>();

        public void Calculate(List<CrmQuote> quotes, string displayCurrency)
        {
            var salesPersons = quotes.Select(x => x.SalesPerson.FullName).Distinct().ToList();
            salesPersons = salesPersons.OrderBy(x => x).ToList();
            var includeLostInTotal = false;
            foreach (var salesPerson in salesPersons)
            {
                decimal won = 0;
                decimal active = 0;
                decimal lost = 0;
                decimal expired = 0;
                decimal total = 0;

                var quotesForSalesPerson = quotes.Where(x => x.SalesPerson.FullName == salesPerson).ToList();
                foreach (var quote in quotesForSalesPerson)
                {
                    var helperCurrency = new HelperCurrencyForIMetal();


                    var quoteTotal = new QuoteTotal(quote.Items.Select(x => x.AsQuoteItem()).ToList(), includeLostInTotal).TotalPrice;
                    quoteTotal = helperCurrency.ConvertValueFromCurrencyToCurrency(quoteTotal, quote.DisplayCurrency, displayCurrency);

                    total += quoteTotal;

                    switch (quote.Status)
                    {
                        case PipelineStatus.Draft:
                            break;
                        case PipelineStatus.Submitted:
                            active += quoteTotal;
                            break;
                        case PipelineStatus.Won:
                            won += quoteTotal;
                            break;
                        case PipelineStatus.Loss:
                            if (!quote.Bid) lost += quoteTotal;
                            break;
                        case PipelineStatus.Expired:
                            expired += quoteTotal;
                            break;
                    }
                }

                Values.Add(new ChartValues()
                {
                    SalesPerson = salesPerson,
                    Active = active,
                    Won = won,
                    Expired = expired,
                    Lost = lost,
                    Total = total
                });

            }

        }

        public TotalDollarsBySalesPersonChartData GetChartData()
        {
            var result = new TotalDollarsBySalesPersonChartData();
            foreach (var chartValue in Values.OrderByDescending(x=>x.Total))
            {
                //result.AddValue(chartValue.SalesPerson, "Total", chartValue.Total.RoundAndNormalize(2));
                result.AddValue(chartValue.SalesPerson, "Won", chartValue.Won.RoundAndNormalize(2));
                result.AddValue(chartValue.SalesPerson, "Active", chartValue.Active.RoundAndNormalize(2));
                result.AddValue(chartValue.SalesPerson, "Expired", chartValue.Expired.RoundAndNormalize(2));
                result.AddValue(chartValue.SalesPerson, "Lost", chartValue.Lost.RoundAndNormalize(2));
            }

            return result;
        }
    }
}