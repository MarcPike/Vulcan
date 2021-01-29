using System.Collections.Generic;
using System.Linq;

namespace DAL.Marketing.ChartModels
{
    public class HitRateBySalesPersonChartData : ChartData
    {
        public List<string> Categories { get; set; } = new List<string>();

        public List<ChartSeriesForStackedBar<int>> Series { get; set; } = new List<ChartSeriesForStackedBar<int>>();

        public void AddValue(string salesPersonName, string seriesName, int value)
        {
            if (Categories.All(x=>x != salesPersonName)) Categories.Add(salesPersonName);
            var seriesValue = Series.SingleOrDefault(x => x.Name == seriesName);
            if (seriesValue == null)
            {
                seriesValue = new ChartSeriesForStackedBar<int>()
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