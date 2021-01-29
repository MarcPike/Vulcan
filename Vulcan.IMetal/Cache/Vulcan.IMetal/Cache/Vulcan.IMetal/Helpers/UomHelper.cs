using System;
using System.Collections.Generic;
using System.Linq;

namespace Vulcan.IMetal.Helpers
{
    public static class UomHelper
    {
        private static List<string> _iMetalLengthUoMs;
        public static List<string> MetalLengthUoMs
        {
            get
            {
                if (_iMetalLengthUoMs == null)
                {
                    _iMetalLengthUoMs = new List<string>
                    {
                        "FT",
                        "IN",
                        "cm",
                        "m",
                        "mm",
                    };
                }

                return _iMetalLengthUoMs;
            }
        }

        public static bool IsValidIMetalLengthUom(string uom)
        {
            return MetalLengthUoMs.Contains(uom, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Since iMetal stores length in Quantity fields, and Quantity might be something other than length, when we are displaying a length field,
        /// we need to check if quantity is actually length. Otherwise, display 0 in a length field that is bound to a non-length quantity field.
        /// </summary>
        /// <param name="quantityUnit"></param>
        /// <returns></returns>
        public static bool QuantityIsLength(string quantityUnit)
        {
            return IsValidIMetalLengthUom(quantityUnit);
        }

        public static decimal GetFactorForPounds(string coid)
        {
            return coid == "CAN" || coid == "INC" ? (decimal)1 : (decimal)2.20462;
        }

        public static decimal GetFactorForKilograms(string coid)
        {
            return coid == "CAN" || coid == "INC" ? (decimal)0.45359 : (decimal)1;
        }

        public static decimal FactorToPounds(string coid, decimal baseWeight)
        {
            return coid == "CAN" || coid == "INC" ? baseWeight : baseWeight * (decimal)2.20462;
        }

        public static decimal FactorToKilograms(string coid, decimal baseWeight)
        {
            return coid == "CAN" || coid == "INC" ? baseWeight * (decimal).453592 : baseWeight;
        }
    }
}
