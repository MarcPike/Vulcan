using System.Collections.Generic;
using System.Linq;

namespace DAL.Marketing.Core.ChartBuilders
{
    public class MarginCalculator
    {
        public List<decimal> Values { get; set; } = new List<decimal>();

        public void Add(decimal value)
        {
            Values.Add(value);
        }

        public decimal Margin
        {
            get
            {
                if (!Values.Any()) return 0;
                return Values.Average();
            }
        }

    }
}