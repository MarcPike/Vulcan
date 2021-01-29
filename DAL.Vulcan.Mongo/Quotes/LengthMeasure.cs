using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Quotes
{
    public class LengthMeasure
    {
        public decimal Inches { get; set; }
        public decimal Feet { get; set; }
        public decimal Yards { get; set; }
        public decimal Millimeters { get; set; }
        public decimal Centimeters { get; set; }
        public decimal Meters { get; set; }

        public static LengthMeasure FromMillimeters(decimal millimeters)
        {
            var inches = millimeters * (decimal) 0.0393701;
            var result = new LengthMeasure()
            {
                Inches = inches,
                Feet = inches / 12,
                Yards = inches / 36,
                Millimeters = inches * (decimal) 25.4,
                Centimeters = inches * (decimal) 2.54,
                Meters = inches * (decimal) 0.0254
            };
            return result;
        }


        public static LengthMeasure FromCentimeters(decimal centimeters)
        {
            var inches = centimeters * (decimal) 0.393701;
            var result = new LengthMeasure()
            {
                Inches = inches,
                Feet = inches / 12,
                Yards = inches / 36,
                Millimeters = inches * (decimal) 25.4,
                Centimeters = inches * (decimal) 2.54,
                Meters = inches * (decimal) 0.0254
            };
            return result;
        }


        public static LengthMeasure FromMeters(decimal meters)
        {
            var inches = meters * (decimal) 39.3701;
            var result = new LengthMeasure()
            {
                Inches = inches,
                Feet = inches / 12,
                Yards = inches / 36,
                Millimeters = inches * (decimal) 25.4,
                Centimeters = inches * (decimal) 2.54,
                Meters = inches * (decimal) 0.0254
            };
            return result;
        }

        public static LengthMeasure FromYards(decimal yards)
        {
            var result = new LengthMeasure()
            {
                Inches = yards * 36,
                Feet = yards / 3,
                Yards = yards,
                Millimeters = (yards * 36) * (decimal) 25.4,
                Centimeters = (yards * 36) * (decimal) 2.54,
                Meters = (yards * 36) * (decimal) 0.0254
            };
            return result;
        }


        public static LengthMeasure FromInches(decimal inches)
        {
            var result = new LengthMeasure()
            {
                Inches = inches,
                Feet = inches / 12,
                Yards = inches / 36,
                Millimeters = inches * (decimal) 25.4,
                Centimeters = inches * (decimal) 2.54,
                Meters = inches * (decimal) 0.0254
            };
            return result;
        }

        public static LengthMeasure FromFeet(decimal feet)
        {
            var inches = feet * 12;
            var result = new LengthMeasure()
            {
                Inches = inches,
                Feet = inches / 12,
                Yards = inches / 36,
                Millimeters = inches * (decimal) 25.4,
                Centimeters = inches * (decimal) 2.54,
                Meters = inches * (decimal) 0.0254
            };
            return result;
        }
    }
}
