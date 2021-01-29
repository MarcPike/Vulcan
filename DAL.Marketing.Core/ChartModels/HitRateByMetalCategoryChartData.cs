using System.Collections.Generic;
using System.Linq;

namespace DAL.Marketing.Core.ChartModels
{
    public class HitRateByMetalCategoryChartData : ChartData
    {
        public List<string> Categories { get; set; } = new List<string>();

        public List<ChartSeriesForStackedBar<int>> Series { get; set; } = new List<ChartSeriesForStackedBar<int>>();

        public void AddValue(string metalCategory, string seriesName, int value)
        {
            if (Categories.All(x => x != metalCategory)) Categories.Add(metalCategory);
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