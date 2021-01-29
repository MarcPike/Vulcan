using System;

namespace DAL.Vulcan.Mongo.Core.Extensions
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// Strips unnecessary trailing zeroes from the actual decimal struct rather than using string formatting tricks.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal Normalize(this decimal value)
        {
            return value / 1.000000000000000000000000000000000M;
        }

        /// <summary>
        /// Rounds the value to the specified decimal precision, and then removes the trailing zeroes from the value.
        /// (i.e. eliminates trailing zeroes from the decimal struct without using string conversions).
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal RoundAndNormalize(this decimal value, int decimals)
        {
            return Math.Round(value, decimals).Normalize();
        }

        /// <summary>
        /// Converts the decimal to a string using the round-trip format specifier, which is not normally available to a decimal.
        /// Use the usual decimal.Parse or decimal.TryParse methods to parse the string back into a decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToRoundTripString(this decimal value)
        {
            var dblValue = (double)value;
            return dblValue.ToString("R");
        }

        /// <summary>
        /// Creates a string representation of the number with all symbols removed, and padded left with the specified number of zeroes.
        /// For example, 2065.28 padded to 11 characters = 00000206528
        /// THE RIGHT TWO DIGITS ARE ALWAYS THE TWO DIGITS AFTER THE DECIMAL PLACE!
        /// </summary>
        /// <param name="value"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        public static string StripSymbolsAndPadLeft(this decimal value, int totalLength)
        {
            var floored = (int)Math.Floor(value);
            var decimalPart = (int)((value - floored) * 100);
            var baseString = floored.ToString() + decimalPart.ToString().PadLeft(2, '0');
            return baseString.PadLeft(totalLength, '0');
        }

    }
}