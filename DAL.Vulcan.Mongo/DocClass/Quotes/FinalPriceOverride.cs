using System;
using DAL.Vulcan.Mongo.Extensions;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Quotes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class FinalPriceOverride
    {
        public decimal TotalPrice { get; set; }
        public CustomerUom CustomerUom { get; set; }
        public RequiredQuantity FinalQuantity { get; set; }
        public FinalPriceOverrideType OverrideType { get; set; } = FinalPriceOverrideType.PerInch;
        public decimal OverrideValue { get; set; } = 0;

        public decimal StartingPricePerInch => (FinalQuantity.TotalInches() == 0) ?  0 : TotalPrice / FinalQuantity.TotalInches();
        public decimal StartingPricePerFoot => (FinalQuantity.TotalFeet() == 0) ? 0 : TotalPrice / FinalQuantity.TotalFeet();



        public decimal StartingPricePerKilogram =>
            (FinalQuantity.TotalKilograms() == 0) ? 0 : TotalPrice / FinalQuantity.TotalKilograms();

        public decimal StartingPricePerPiece => (FinalQuantity.Pieces == 0) ? 0 : TotalPrice / FinalQuantity.Pieces;

        public decimal FinalPrice { get; set; }
        public decimal FinalPricePerInch { get; set; }
        public decimal FinalPricePerPound { get; set; }
        public decimal FinalPricePerFoot { get; set; }
        public decimal FinalPricePerKilogram { get; set; }
        public decimal FinalPricePerEach { get; set; }
        public decimal FinalPricePerMillimeter => 
            (FinalQuantity.TotalMillimeters() == 0) ? 0 : TotalPrice / FinalQuantity.TotalMillimeters();

        public decimal FinalPricePerCentimeter =>
            (FinalQuantity.TotalCentimeters() == 0) ? 0 : TotalPrice / FinalQuantity.TotalCentimeters();

        public decimal FinalPricePerMeter =>
            (FinalQuantity.TotalMeters() == 0) ? 0 : TotalPrice / FinalQuantity.TotalMeters();

        public decimal FinalPricePerYard => FinalPricePerFoot * 3;


        //public decimal FinalMargin => QuoteCalculations.GetMargin(TotalPrice, FinalPrice);

        private bool InPriceRecalcMode = false;

        public FinalPriceOverride(
            RequiredQuantity finalQuantity, 
            decimal totalPrice, 
            CustomerUom customerUom, 
            FinalPriceOverrideType overrideType, 
            decimal overrideValue)
        {
            FinalQuantity = finalQuantity;
            TotalPrice = totalPrice;
            CustomerUom = customerUom;
            OverrideType = overrideType;
            OverrideValue = overrideValue;
            CalculateFinalTotals();
        }

        public void CalculateFinalTotals()
        {

            FinalPrice = TotalPrice;
            if (OverrideValue > 0)
            {
                switch (OverrideType)
                {
                    case FinalPriceOverrideType.PerInch:
                        FinalPrice = FinalQuantity.TotalInches() * OverrideValue;
                        break;
                    case FinalPriceOverrideType.PerFoot:
                        FinalPrice = FinalQuantity.TotalFeet() * OverrideValue;
                        break;
                    case FinalPriceOverrideType.PerPound:
                        FinalPrice = FinalQuantity.TotalPounds() * OverrideValue;
                        break;
                    case FinalPriceOverrideType.PerKilogram:
                        FinalPrice = FinalQuantity.TotalKilograms() * OverrideValue;
                        break;
                    case FinalPriceOverrideType.PerPiece:
                        FinalPrice = FinalQuantity.Pieces * OverrideValue;
                        break;
                    case FinalPriceOverrideType.PerLot:
                        FinalPrice = OverrideValue;
                        break;
                    default:
                        FinalPrice = 0;
                        break;
                }

            }

            FinalPricePerInch = (FinalQuantity.TotalInches() == 0) ? 0 : FinalPrice / FinalQuantity.TotalInches();
            FinalPricePerPound = (FinalQuantity.TotalPounds() == 0) ? 0 : FinalPrice / FinalQuantity.TotalPounds();
            FinalPricePerFoot = (FinalQuantity.TotalFeet() == 0) ? 0 : FinalPrice / FinalQuantity.TotalFeet();
            FinalPricePerKilogram = (FinalQuantity.TotalKilograms() == 0) ? 0 : FinalPrice / FinalQuantity.TotalKilograms();
            FinalPricePerEach = (FinalQuantity.Pieces == 0) ? 0 : FinalPrice / FinalQuantity.Pieces;

            if (InPriceRecalcMode == false)
            {
                var originalOverrideType = OverrideType;
                var originalOverrideValue = OverrideValue;
                switch (CustomerUom)
                {
                    case CustomerUom.Inches:
                        OverrideType = FinalPriceOverrideType.PerInch;
                        OverrideValue = FinalPricePerInch;
                        break;
                    case CustomerUom.Feet:
                        OverrideType = FinalPriceOverrideType.PerFoot;
                        OverrideValue = FinalPricePerFoot;
                        break;
                    case CustomerUom.Pounds:
                        OverrideType = FinalPriceOverrideType.PerPound;
                        OverrideValue = FinalPricePerPound;
                        break;
                    case CustomerUom.Kilograms:
                        OverrideType = FinalPriceOverrideType.PerKilogram;
                        OverrideValue = FinalPricePerKilogram;
                        break;
                    case CustomerUom.PerPiece:
                        OverrideType = FinalPriceOverrideType.PerPiece;
                        OverrideValue = FinalPricePerEach;
                        break;
                }
                InPriceRecalcMode = true;
                CalculateFinalTotals();
                OverrideType = originalOverrideType;
                OverrideValue = originalOverrideValue;
                InPriceRecalcMode = false;
            }
        }

        public decimal FinalUnitCost
        {
            get
            {
                switch (CustomerUom)
                {
                    case CustomerUom.Inches:
                        return FinalPricePerInch;
                    case CustomerUom.Feet:
                        return FinalPricePerFoot;
                    case CustomerUom.Pounds:
                        return FinalPricePerPound;
                    case CustomerUom.Kilograms:
                        return FinalPricePerKilogram;
                    case CustomerUom.PerPiece:
                        return FinalPricePerEach;

                    case CustomerUom.Millimeters:
                        return FinalPricePerMillimeter;
                    case CustomerUom.Centimeters:
                        return FinalPricePerCentimeter;
                    case CustomerUom.Meters:
                        return FinalPricePerMeter;
                    case CustomerUom.Yards:
                        return FinalPricePerYard;

                    default:
                        return 0;
                }
            }
        }

        public string FinalUnitCostSuffix
        {
            get
            {
                switch (CustomerUom)
                {
                    case CustomerUom.Inches:
                        return " / in";
                    case CustomerUom.Feet:
                        return " / ft";
                    case CustomerUom.Pounds:
                        return " / lb";
                    case CustomerUom.Kilograms:
                        return " / kg";
                    case CustomerUom.PerPiece:
                        return " / each";
                    case CustomerUom.Millimeters:
                        return " / mm";
                    case CustomerUom.Centimeters:
                        return " / cm";
                    case CustomerUom.Meters:
                        return " / m";
                    case CustomerUom.Yards:
                        return " / yd";
                    default:
                        return "";
                }
            }
        }

        public string FinalUom
        {
            get
            {
                switch (CustomerUom)
                {
                    case CustomerUom.Inches:
                        return "in";
                    case CustomerUom.Feet:
                        return "ft";
                    case CustomerUom.Pounds:
                        return "lbs";
                    case CustomerUom.Kilograms:
                        return "kg";
                    case CustomerUom.PerPiece:
                        return "in";
                    case CustomerUom.Millimeters:
                        return "mm";
                    case CustomerUom.Centimeters:
                        return "cm";
                    case CustomerUom.Meters:
                        return "m";
                    case CustomerUom.Yards:
                        return "yd";
                    default:
                        return "";
                }
            }
        }
        public decimal FinalQuantityValue
        {
            get
            {
                

                switch (CustomerUom)
                {
                    case CustomerUom.Inches:
                        return FinalQuantity.PieceLength.Inches;
                    case CustomerUom.Feet:
                        return FinalQuantity.PieceLength.Feet;
                    case CustomerUom.Pounds:
                        return FinalQuantity.PieceWeight.Pounds;
                    case CustomerUom.Kilograms:
                        return FinalQuantity.PieceWeight.Kilograms;
                    case CustomerUom.PerPiece:
                        return FinalQuantity.PieceLength.Inches;
                    case CustomerUom.Millimeters:
                        return FinalQuantity.PieceLength.Millimeters;
                    case CustomerUom.Centimeters:
                        return FinalQuantity.PieceLength.Centimeters;
                    case CustomerUom.Meters:
                        return FinalQuantity.PieceLength.Meters;
                    case CustomerUom.Yards:
                        return FinalQuantity.PieceLength.Yards;

                    default:
                        return (decimal) 0;
                }
            }
        }
    }

}