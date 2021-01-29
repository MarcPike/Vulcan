using System;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.Quotes;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    [BsonIgnoreExtraElements]
    public class QuickQuoteData
    {
        public string Coid { get; set; }
        public string Label { get; set; } = string.Empty;
        public decimal Cost { get; set; } = 0;
        public decimal UnitCost { get; set; } = 0;
        public decimal UnitPrice { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public bool Regret { get; set; } = false;
        public ProductMaster StartingProduct { get; set; }
        public ProductMaster FinishedProduct { get; set; }
        public OrderQuantity OrderQuantity { get; set; }

        //public CrozCalcData CalcData { get; set; } = new CrozCalcData();

        public RequiredQuantity RequiredQuantity
        {
            get
            {
                if ((FinishedProduct != null) && !OrderQuantity.EachBased)
                {
                    return OrderQuantity.GetRequiredQuantity(Coid, FinishedProduct.TheoWeight);
                }
                else
                {
                    return null;
                }
            }
        }

        public decimal QuantityValueForPdf()
        {
            if (RequiredQuantity == null) return OrderQuantity.Quantity;


            if (OrderQuantity.QuantityType == "in")
                    return RequiredQuantity.PieceLength.Inches;

            if (OrderQuantity.QuantityType == "ft")
                return RequiredQuantity.PieceLength.Feet;

            if (OrderQuantity.QuantityType == "lb")
                    return RequiredQuantity.PieceWeight.Pounds;

            if (OrderQuantity.QuantityType == "kg")
                    return RequiredQuantity.PieceWeight.Kilograms;

            if (OrderQuantity.QuantityType == "cm")
                    return RequiredQuantity.PieceLength.Centimeters;
            if (OrderQuantity.QuantityType == "mm")
                return RequiredQuantity.PieceLength.Millimeters;
            if (OrderQuantity.QuantityType == "yd")
                return RequiredQuantity.PieceLength.Yards;

            return (decimal) 0;
            
        }

        public string UomValueForPdf()
        {

            return OrderQuantity.QuantityType;
           
        }

        public decimal UnitPriceValueForPdf()
        {
            if (Price == 0) return 0;

            if (RequiredQuantity == null)
            {
                if (OrderQuantity.Pieces == 0) return 0;
                if (OrderQuantity.Quantity == 0) return Price / OrderQuantity.Pieces;
                return Price / (OrderQuantity.Quantity * OrderQuantity.Pieces);
            }
            var totalQuantity = (decimal) 0;

            if (OrderQuantity.QuantityType == "in")
                totalQuantity = RequiredQuantity.TotalInches();

            if (OrderQuantity.QuantityType == "ft")
                totalQuantity = RequiredQuantity.TotalFeet();

            if (OrderQuantity.QuantityType == "lb")
                totalQuantity = RequiredQuantity.TotalPounds();

            if (OrderQuantity.QuantityType == "kg")
                totalQuantity = RequiredQuantity.TotalKilograms();

            if (OrderQuantity.QuantityType == "cm")
                totalQuantity = RequiredQuantity.TotalCentimeters();
            if (OrderQuantity.QuantityType == "mm")
                totalQuantity = RequiredQuantity.TotalMillimeters();
            if (OrderQuantity.QuantityType == "yd")
                totalQuantity = RequiredQuantity.TotalYards();

            return Price / totalQuantity;
        }

        public decimal GetWeightInPounds()
        {
            var result = (decimal) 0;
            if (RequiredQuantity != null)
            {
                result = RequiredQuantity.TotalPounds();
            }
            else
            {
                result = OrderQuantity.GetTotalPounds();
            }

            return result;
        }

        public decimal GetWeightInKilos()
        {
            var result = (decimal)0;
            if (RequiredQuantity != null)
            {
                result = RequiredQuantity.TotalKilograms();
            }
            else
            {
                result = OrderQuantity.GetTotalKilos();
            }

            return result;
        }


        public QuickQuoteData()
        {

        }

        public QuickQuoteData(string coid)
        {
            Coid = coid;
            OrderQuantity = new OrderQuantity(1,0,"in");
        }

    }
}