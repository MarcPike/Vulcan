using System.Collections.Generic;
using System.Linq;
using DAL.Marketing.ChartModels;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Marketing.ChartBuilders
{

    public class HitRateByCustomerChartBuilder 
    {
        private class ChartValues
        {
            public string Customer;
            public int Won = 0;
            public int Total = 0;
            public int Active = 0;
            public int Lost = 0;
            public int Expired = 0;
        }

        private List<ChartValues> Values { get;  } = new List<ChartValues>();

        public void Calculate(List<CrmQuote> quotes)
        {

            var customers = quotes.Select(x => x.Company.Code + " " + x.Company.ShortName).Distinct().ToList();
            customers = customers.OrderBy(x => x).ToList();
            foreach (var customer in customers)
            {
                int won = 0;
                int active = 0;
                int lost = 0;
                int expired = 0;
                int total = 0;

                var quotesForCompany = quotes.Where(x => x.Company.Code +" " + x.Company.ShortName == customer).ToList();
                foreach (var quote in quotesForCompany)
                {
                    switch (quote.Status)
                    {
                        case PipelineStatus.Draft:
                            break;
                        case PipelineStatus.Submitted:
                            active += 1;
                            break;
                        case PipelineStatus.Won:
                            won += 1;
                            break;
                        case PipelineStatus.Loss:
                            if (!quote.Bid) lost += 1;
                            break;
                        case PipelineStatus.Expired:
                            expired += 1;
                            break;
                    }

                    total += 1;
                }

                Values.Add(new ChartValues()
                {
                    Customer = customer,
                    Active = active,
                    Won = won,
                    Expired = expired,
                    Lost = lost,
                    Total = total
                });

            }

        }


        public HitRateByCustomerChartData GetChartData()
        {
            var result = new HitRateByCustomerChartData();
            foreach (var chartValue in Values.OrderByDescending(x=>x.Total))
            {
                //result.AddValue(chartValue.Customer, "Total", chartValue.Total);
                result.AddValue(chartValue.Customer, "Won", chartValue.Won);
                result.AddValue(chartValue.Customer, "Active", chartValue.Active);
                result.AddValue(chartValue.Customer, "Expired", chartValue.Expired);
                result.AddValue(chartValue.Customer, "Lost", chartValue.Lost);
            }

            return result;
        }
    }
}