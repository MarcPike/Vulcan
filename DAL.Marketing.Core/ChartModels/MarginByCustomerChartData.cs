using System.Collections.Generic;
using System.Linq;

namespace DAL.Marketing.Core.ChartModels
{
    public class MarginByCustomerChartData : ChartData
    {
        public List<string> Categories { get; set; } = new List<string>();

        public List<ChartSeriesForStackedBar<decimal>> Series { get; set; } = new List<ChartSeriesForStackedBar<decimal>>();

        public void AddValue(string customer, string seriesName, decimal value)
        {
            if (Categories.All(x => x != customer)) Categories.Add(customer);
            var seriesValue = Series.SingleOrDefault(x => x.Name == seriesName);
            if (seriesValue == null)
            {
                seriesValue = new ChartSeriesForStackedBar<decimal>()
                {
                    Name = seriesName,
                };
                seriesValue.Data.Add(value);
                Series.Add(seriesValue);
            }
            else
            {
                seriesValue.Data.Add(value);
            }
        }
    }
}