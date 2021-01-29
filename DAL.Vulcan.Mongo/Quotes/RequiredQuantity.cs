using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Quotes
{
    public class RequiredQuantity : ICloneable
    {
        public int Pieces { get; set; }
        public WeightMeasure PieceWeight { get; set; }
        public LengthMeasure PieceLength { get; set; }

        public decimal TotalPieces()
        {
            return Pieces;
        }

        public decimal TotalPounds()
        {
            return Pieces * PieceWeight.Pounds;
        }
        public decimal TotalKilograms()
        {
            return Pieces * PieceWeight.Kilograms;
        }

        public decimal TotalFeet()
        {
            return Pieces * PieceLength.Feet;
        }

        public decimal TotalInches()
        {
            return Pieces * PieceLength.Inches;
        }
        public decimal TotalMeters()
        {
            return Pieces * PieceLength.Meters;
        }

        public void ChangeTheoWeight(decimal theoWeight)
        {
            PieceWeight.Pounds = PieceLength.Inches * theoWeight;
            PieceWeight.Kilograms = PieceWeight.Pounds * (decimal)0.453592;
        }

        public static RequiredQuantity CalculateNewQuantityForNewProduct(RequiredQuantity startingRequiredQuantity,
            ProductMaster finishedProduct)
        {
            if ((startingRequiredQuantity == null) || (finishedProduct == null)) return null;

            var result = new RequiredQuantity()
            {
                Pieces = startingRequiredQuantity.Pieces,
                PieceLength = startingRequiredQuantity.PieceLength,
                PieceWeight = new WeightMeasure()
                {
                    Pounds = startingRequiredQuantity.PieceLength.Inches * finishedProduct.TheoWeight * finishedProduct.FactorForLbs,
                    Kilograms = (startingRequiredQuantity.PieceLength.Inches * finishedProduct.TheoWeight) * finishedProduct.FactorForKilograms
                }
            };

            return result;
        }

        public RequiredQuantity(OrderQuantity orderQuantity, string coid, decimal theoWeight)
        {
            var requiredQuantity = orderQuantity.GetRequiredQuantity(coid, theoWeight);

            Pieces = requiredQuantity.Pieces;
            PieceLength = requiredQuantity.PieceLength;
            PieceWeight = requiredQuantity.PieceWeight;
        }

        public RequiredQuantity()
        {
        }

        public RequiredQuantity GetFinishedQuantityWithoutKerf(decimal kerfPoundsPerPiece, decimal kerfInchesPerCut)
        {
            var inches = PieceLength.Inches - kerfInchesPerCut;
            var pounds = PieceWeight.Pounds - kerfPoundsPerPiece;
            var result = new RequiredQuantity()
            {
                Pieces = Pieces,
                PieceLength = new LengthMeasure()
                {
                    Inches = inches,
                    Feet = inches / 12,
                    Yards = inches / 36,
                    Millimeters = inches * (decimal)25.4,
                    Centimeters = inches * (decimal)2.54,
                    Meters = inches * (decimal)0.0254
                },
                PieceWeight = new WeightMeasure()
                {
                    Pounds = pounds,
                    Kilograms = pounds * (decimal)0.453592
                }
            };

            return result;
        }

        public decimal TotalCentimeters()
        {
            return TotalInches() * (decimal)2.54;
        }

        public decimal TotalMillimeters()
        {
            return TotalInches() * (decimal) 25.4;
        }

        public decimal TotalYards()
        {
            return TotalInches() * (decimal)0.0277778;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
