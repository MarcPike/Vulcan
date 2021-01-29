using System;
using DAL.Vulcan.Mongo.Extensions;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public static class QuoteCalculations
    {
        public static decimal GetMarkup(decimal totalCost, decimal salePrice)
        {
            // Markup = (SalePrice - InternalCost) / InternalCost

            //Round and normalize to prevent decimal overflow errors.
            totalCost = totalCost.RoundAndNormalize(6);
            salePrice = salePrice.RoundAndNormalize(6);

            return totalCost != 0M ? (salePrice - totalCost) / totalCost : 0M;
        }

        /// <summary>
        /// Returns the profit margin (1.0 = 100%)
        /// </summary>
        /// <param name="totalCost"></param>
        /// <param name="salePrice"></param>
        /// <returns></returns>
        public static decimal GetMargin(decimal totalCost, decimal salePrice)
        {
            // Margin = (SalePrice - InternalCost) / Abs(SalePrice)

            //Round and normalize to prevent decimal overflow errors.
            totalCost = totalCost.RoundAndNormalize(6);
            salePrice = salePrice.RoundAndNormalize(6);

            return salePrice != 0M ? (salePrice - totalCost) / Math.Abs(salePrice) : 0M;
        }

        /// <summary>
        /// Gets either margin or markup, depending on the value of getMargin. (1.0 = 100%)
        /// </summary>
        /// <param name="totalCost">The cost of the goods or service.</param>
        /// <param name="salePrice">The sale price of the goods or services.</param>
        /// <param name="getMargin">When true (default),gets profit margin. When false, gets markup.</param>
        /// <returns></returns>
        public static decimal GetMarginOrMarkup(decimal totalCost, decimal salePrice, bool getMargin = true)
        {
            return getMargin ? 
                NumericUtils.GetMargin(totalCost, salePrice) : 
                NumericUtils.GetMarkup(totalCost, salePrice);
        }



        /// <summary>
        /// Calculates the charge for the given cost and markup percentage. 
        /// </summary>
        /// <param name="totalCost"></param>
        /// <param name="markup">Markup percentage must be a decimal where 1.0 = 100%.</param>
        /// <returns></returns>
        public static decimal GetSalePriceFromMarkup(decimal totalCost, decimal markup)
        {
            // SalePrice = (InternalCost * Markup) + InternalCost
            return totalCost * markup + totalCost;
        }

        /// <summary>
        /// Calculates the charge for the given cost and profit margin. 
        /// </summary>
        /// <param name="totalCost"></param>
        /// <param name="margin">Profit margin must be a decimal where 1.0 = 100%.</param>
        /// <returns></returns>
        public static decimal GetSalePriceFromMargin(decimal totalCost, decimal margin)
        {
            // SalePrice = InternalCost / (1.0 - Margin) 
            // Margin can't be > 100%, but it can be < -100%.
            if (margin > 1M) margin = 1M;
            return margin != 1M ? totalCost / (1M - margin) : 0M;
        }

        /// <summary>
        /// Gets the sale price calculated from cost and either margin or markup, depending on the value of getMargin.
        /// </summary>
        /// <param name="cost">The cost of the goods or service.</param>
        /// <param name="marginOrMarkup">The percentage of margin or markup, where 1.0 = 100%</param>
        /// <param name="getMargin">When true (default), uses a profit margin calculation. When false, gets markup.</param>
        /// <returns></returns>
        public static decimal GetSalePriceFromMarginOrMarkup(decimal cost, decimal marginOrMarkup, bool getMargin = true)
        {
            return getMargin ? 
                NumericUtils.GetSalePriceFromMargin(cost, marginOrMarkup) : 
                NumericUtils.GetSalePriceFromMarkup(cost, marginOrMarkup);
        }


    }
}