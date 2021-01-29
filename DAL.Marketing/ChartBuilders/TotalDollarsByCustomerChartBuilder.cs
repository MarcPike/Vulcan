using System.Collections.Generic;
using System.Linq;
using DAL.Marketing.ChartModels;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Extensions;
using DAL.Vulcan.Mongo.Models;
using Vulcan.IMetal.Helpers;

namespace DAL.Marketing.ChartBuilders
{
    public class TotalDollarsByCustomerChartBuilder 
    {
        private struct ChartValues
        {
            public string Customer;
            public decimal Won;
            public decimal Total;
            public decimal Active;
            public decimal Expired;
            public decimal Lost;
        }

        private List<ChartValues> Values { get;  } = new List<ChartValues>();

        public void Calculate(List<CrmQuote> quotes, string displayCurrency)
        {
            quotes = quotes.Where(x => x.Company != null).ToList();

            var customers = quotes.Select(x => x.Company.Code + " " + x.Company.ShortName).Distinct().ToList();
            customers = customers.OrderBy(x => x).ToList();
            var includeLostInTotal = false;
            foreach (var customer in customers)
            {
                decimal won = 0;
                decimal active = 0;
                decimal lost = 0;
                decimal expired = 0;
                decimal total = 0;

                var quotesForSalesPerson = quotes.Where(x => x.Company.Code + " " + x.Company.ShortName == customer).ToList();
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
                    Customer = customer,
                    Active = active,
                    Won = won,
                    Lost = lost,
                    Expired = expired,
                    Total = total
                });

            }

        }

        public TotalDollarsByCustomerChartData GetChartData()
        {
            var result = new TotalDollarsByCustomerChartData();
            foreach (var chartValue in Values.OrderByDescending(x=>x.Total))
            {
                //result.AddValue(chartValue.Customer, "Total", chartValue.Total.RoundAndNormalize(2));
                result.AddValue(chartValue.Customer, "Won", chartValue.Won.RoundAndNormalize(2));
                result.AddValue(chartValue.Customer, "Active", chartValue.Active.RoundAndNormalize(2));
                result.AddValue(chartValue.Customer, "Expired", chartValue.Expired.RoundAndNormalize(2));
                result.AddValue(chartValue.Customer, "Lost", chartValue.Lost.RoundAndNormalize(2));
            }

            return result;
        }
    }
}