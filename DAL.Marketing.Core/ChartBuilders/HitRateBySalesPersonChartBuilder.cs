using System.Collections.Generic;
using System.Linq;
using DAL.Marketing.Core.ChartModels;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Marketing.Core.ChartBuilders
{
    public class HitRateBySalesPersonChartBuilder 
    {
        private class ChartValues
        {
            public string SalesPerson;
            public int Won = 0;
            public int Total = 0;
            public int Active = 0;
            public int Lost = 0;
            public int Expired = 0;
        }

        private List<ChartValues> Values { get; } = new List<ChartValues>();

        public void Calculate(List<CrmQuote> quotes)
        {
            var salesPersons = quotes.Select(x => x.SalesPerson.FullName).Distinct().ToList();
            salesPersons = salesPersons.OrderBy(x => x).ToList();

            foreach (var salesPerson in salesPersons)
            {
                Values.Add(new ChartValues()
                {
                    SalesPerson = salesPerson,
                    Total = quotes.Count(x => x.SalesPerson.FullName == salesPerson && x.Status != PipelineStatus.Draft),
                    Won = quotes.Count(x => x.SalesPerson.FullName == salesPerson && x.Status == PipelineStatus.Won),
                    Active = quotes.Count(x => x.SalesPerson.FullName == salesPerson && x.Status == PipelineStatus.Submitted),
                    Lost = quotes.Count(x => x.SalesPerson.FullName == salesPerson && x.Status == PipelineStatus.Loss && x.Bid == false),
                    Expired = quotes.Count(x => x.SalesPerson.FullName == salesPerson && x.Status == PipelineStatus.Expired)
                });
            }

        }

        public HitRateBySalesPersonChartData GetChartData()
        {
            var result = new HitRateBySalesPersonChartData();
            foreach (var chartValue in Values.OrderByDescending(x=>x.Total))
            {
                result.AddValue(chartValue.SalesPerson, "Won", chartValue.Won);
                result.AddValue(chartValue.SalesPerson, "Active", chartValue.Active);
                result.AddValue(chartValue.SalesPerson, "Expired", chartValue.Expired);
                result.AddValue(chartValue.SalesPerson, "Lost", chartValue.Lost);
            }

            return result;
        }
    }

}
