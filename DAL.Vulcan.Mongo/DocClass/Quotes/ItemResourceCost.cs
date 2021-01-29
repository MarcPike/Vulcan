using System;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class ItemResourceCost
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public ResourceType ResourceType { get; set; } = ResourceType.Unknown;
        public PerType PerType { get; set; } = PerType.PerPiece;
        public decimal Price { get; set; } = 0;
        public decimal Cost { get; set; } = 0;
        public bool IsPriceBlended { get; set; } = false;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Fn: public decimal GetTotalPriceFromBasePrice(StartingBaseCost baseCost)
        ///
        /// Summary:    Gets total price from base price.
        ///
        /// Author: Mpike
        ///
        /// Date:   1/5/2018
        ///
        /// Exceptions:
        /// ArgumentOutOfRangeException -   Thrown when one or more arguments are outside the required
        ///                                 range. 
        ///
        /// Parameters:
        /// baseCost -     The base price. 
        ///
        /// Returns:    The total price from base price.
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public decimal GetTotalPriceFromBasePrice(BaseCost baseCost)
        {
            decimal result = 0;
            switch (PerType)
            {
                case PerType.PerPiece:
                    result = (Price * baseCost.TotalPieces);
                    break;
                case PerType.PerInch:
                    result = (Price * baseCost.TotalInches * baseCost.TotalPieces);
                    break;
                case PerType.PerFoot:
                    result = (Price * (baseCost.TotalFeet) * baseCost.TotalPieces);
                    break;
                case PerType.PerPound:
                    result = (Price * baseCost.TotalPounds * baseCost.TotalPieces);
                    break;
                case PerType.PerKg:
                    result = (Price * baseCost.TotalKilograms * baseCost.TotalPieces);
                    break;
                case PerType.PerLot:
                    result = Price;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        public decimal GetTotalCostFromBasePrice(BaseCost baseCost)
        {
            decimal result = 0;
            switch (PerType)
            {
                case PerType.PerPiece:
                    result = (Cost * baseCost.TotalPieces);
                    break;
                case PerType.PerInch:
                    result = (Cost * baseCost.TotalInches * baseCost.TotalPieces);
                    break;
                case PerType.PerFoot:
                    result = (Cost * (baseCost.TotalFeet) * baseCost.TotalPieces);
                    break;
                case PerType.PerPound:
                    result = (Cost * baseCost.TotalPounds * baseCost.TotalPieces);
                    break;
                case PerType.PerKg:
                    result = (Cost * baseCost.TotalKilograms * baseCost.TotalPieces);
                    break;
                case PerType.PerLot:
                    result = Cost;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

    }
}