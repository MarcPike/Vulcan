using System.Collections.Generic;

namespace DAL.Marketing.ChartModels
{
    public class ChartSeriesForStackedBar<TType>
    {
        public string Name { get; set; }
        public List<TType> Data { get; set; } = new List<TType>();

        
    }
}