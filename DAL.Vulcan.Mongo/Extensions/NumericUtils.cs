using System;
using System.Globalization;

namespace DAL.Vulcan.Mongo.Extensions
{
    public static class NumericUtils
    {
        #region Misc

        /// <summary>
        ///     Returns a value indicating whether the provided value is numeric.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(object value)
        {
            return double.TryParse(Convert.ToString(value), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out _);
        }

        #endregion

        #region Conversion Utils

        /// <summary>
        ///     Returns a decimal from the provided string.  Returns defaultValue (0 if not specified) if string could not be
        ///     parsed.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetDecimal(object input, decimal defaultValue = 0M)
        {
            if (input is decimal @decimal) return @decimal;
            
            return decimal.TryParse((string) input, out var output) ? output : defaultValue;
        }

        /// <summary>
        ///     Returns a nullable of decimal for the provided object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static decimal? GetDecimal_Nullable(object input)
        {
            var strInput = (string) input;

            if (string.IsNullOrEmpty(strInput))
            {
                return null;
            }

            return GetDecimal(strInput);
        }

        /// <summary>
        ///     Returns a double from the provided string.  Returns defaultValue (0 if not specified) if string could not be
        ///     parsed.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double GetDouble(object input, double defaultValue = 0D)
        {
            if (input is double d) return d;

            return double.TryParse((string) input, out var output) ? output : defaultValue;
        }

        /// <summary>
        ///     Returns a nullable of double for the provided object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double? GetDouble_Nullable(object input)
        {
            var strInput = (string) input;

            if (string.IsNullOrEmpty(strInput))
            {
                return null;
            }

            return GetDouble(strInput);
        }

        /// <summary>
        ///     Returns an int from the provided string.  Returns defaultValue (0 if not specified) if string could not be parsed.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt32(object input, int defaultValue = 0)
        {
            switch (input)
            {
                case null:
                    return 0;
                case int i:
                    return i;
            }

            return int.TryParse((string) input, out var output) ? output : defaultValue;
        }

        /// <summary>
        ///     Returns a nullable of int for the provided object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int? GetInt32_Nullable(object input)
        {
            var strInput = (string) input;

            if (string.IsNullOrEmpty(strInput))
            {
                return null;
            }

            return GetInt32(strInput);
        }

        /// <summary>
        ///     Returns a long from the provided string.  Returns defaultValue (0 if not specified) if string could not be parsed.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long GetInt64(object input, long defaultValue = 0L)
        {
            switch (input)
            {
                case null:
                    return 0L;
                case int _:
                    return (long) input;
                case long _:
                    return (long) input;
            }

            return long.TryParse((string) input, out var output) ? output : defaultValue;
        }

        /// <summary>
        ///     Returns a nullable of int for the provided object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long? GetInt64_Nullable(object input)
        {
            var strInput = (string) input;

            if (string.IsNullOrEmpty(strInput))
            {
                return null;
            }

            return GetInt64(strInput);
        }

        #endregion


        #region Margin and Markup

        /// <summary>
        ///     Returns the markup percentage (1.0 = 100%)
        /// </summary>
        /// <param name="totalCost"></param>
        /// <param name="salePrice"></param>
        /// <returns></returns>
        public static decimal GetMarkup(decimal totalCost, decimal salePrice)
        {
            // Markup = (SalePrice - InternalCost) / InternalCost

            //Round and normalize to prevent decimal overflow errors.
            totalCost = totalCost.RoundAndNormalize(6);
            salePrice = salePrice.RoundAndNormalize(6);

            return (totalCost != 0M) ? (salePrice - totalCost) / totalCost : 0M;
        }

        /// <summary>
        ///     Returns the profit margin (1.0 = 100%)
        /// </summary>
        /// <param name="totalCost"></param>
        /// <param name="salePrice"></param>
        /// <returns></returns>
        public static decimal GetMargin(decimal totalCost, decimal salePrice)
        {
            // Margin = (SalePrice - InternalCost) / SalePrice

            //Round and normalize to prevent decimal overflow errors.
            totalCost = totalCost.RoundAndNormalize(6);
            salePrice = salePrice.RoundAndNormalize(6);

            return (salePrice != 0M) ? (salePrice - totalCost) / salePrice : 0M;
        }

        /// <summary>
        ///     Gets either margin or markup, depending on the value of getMargin. (1.0 = 100%)
        /// </summary>
        /// <param name="totalCost">The cost of the goods or service.</param>
        /// <param name="salePrice">The sale price of the goods or services.</param>
        /// <param name="getMargin">When true (default),gets profit margin. When false, gets markup.</param>
        /// <returns></returns>
        public static decimal GetMarginOrMarkup(decimal totalCost, decimal salePrice, bool getMargin = true)
        {
            return getMargin ? GetMargin(totalCost, salePrice) : GetMarkup(totalCost, salePrice);
        }


        /// <summary>
        ///     Calculates the charge for the given cost and markup percentage.
        /// </summary>
        /// <param name="totalCost"></param>
        /// <param name="markup">Markup percentage must be a decimal where 1.0 = 100%.</param>
        /// <returns></returns>
        public static decimal GetSalePriceFromMarkup(decimal totalCost, decimal markup)
        {
            // SalePrice = (InternalCost * Markup) + InternalCost
            return (totalCost * markup) + totalCost;
        }

        /// <summary>
        ///     Calculates the charge for the given cost and profit margin.
        /// </summary>
        /// <param name="totalCost"></param>
        /// <param name="margin">Profit margin must be a decimal where 1.0 = 100%.</param>
        /// <returns></returns>
        public static decimal GetSalePriceFromMargin(decimal totalCost, decimal margin)
        {
            // SalePrice = InternalCost / (1.0 - Margin)

            return (margin != 1M) ? totalCost / (1M - margin) : 0M;
        }

        /// <summary>
        ///     Gets the sale price calculated from cost and either margin or markup, depending on the value of getMargin.
        /// </summary>
        /// <param name="cost">The cost of the goods or service.</param>
        /// <param name="marginOrMarkup">The percentage of margin or markup, where 1.0 = 100%</param>
        /// <param name="getMargin">When true (default), uses a profit margin calculation. When false, gets markup.</param>
        /// <returns></returns>
        public static decimal GetSalePriceFromMarginOrMarkup(decimal cost, decimal marginOrMarkup,
            bool getMargin = true)
        {
            return getMargin ? 
                GetSalePriceFromMargin(cost, marginOrMarkup) : 
                GetSalePriceFromMarkup(cost, marginOrMarkup);
        }

        #endregion
    }
}