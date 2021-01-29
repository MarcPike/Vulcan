using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Core.Quotes
{
    public class WeightMeasure
    {
        public decimal Pounds { get; set; }
        public decimal Kilograms { get; set; }

        public static WeightMeasure FromKilos(decimal kilos)
        {
            var result = new WeightMeasure
            {
                Kilograms = kilos,
                Pounds = kilos / (decimal) 2.204620823516057
            };
            return result;

        }

        public static WeightMeasure FromPounds(decimal pounds)
        {
            var result = new WeightMeasure
            {
                Pounds = pounds,
                Kilograms = pounds * (decimal) 0.453592
            };
            return result;
        }
    }
}
