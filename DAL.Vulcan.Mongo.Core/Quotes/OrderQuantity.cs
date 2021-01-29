using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.UnitConversions;

namespace DAL.Vulcan.Mongo.Core.Quotes
{
    public class OrderQuantity
    {
        public List<string> Lengths { get; } = new List<string>()
        {
            "in",
            "ft",
            "cm",
            "yd",
            "mm",
            "m"
        };

        public  List<string> Weights { get; } = new List<string>()
        {
            "lb",
            "kg"
        };

        public  List<string> Each { get; } = new List<string>()
        {
            "ea"
        };

        public int Pieces { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityType { get; set; }

        public bool WeightBased => Weights.Contains(QuantityType);
        public bool LengthBased => Lengths.Contains(QuantityType);
        public bool EachBased => Each.Contains(QuantityType);


        public decimal GetTotalInches()
        {
            if (QuantityType == "in") return Quantity;
            if (QuantityType == "ft") return Quantity * 12;
            if (QuantityType == "cm") return Quantity * (decimal)0.393701;
            if (QuantityType == "yd") return Quantity * 36;
            if (QuantityType == "mm") return Quantity * (decimal) 0.0393701;
            if (QuantityType == "m") return Quantity * (decimal) 39.3701;

            return 0;
        }

        public OrderQuantity()
        {
        }

        public OrderQuantity(int pieces, decimal quantity, string quantityType)
        {
            Pieces = pieces;
            Quantity = quantity;
            QuantityType = quantityType;

            if ((!WeightBased) && (!LengthBased) && (!EachBased))
                throw new Exception("Invalid Quantity Type: " + QuantityType);
        }

        public RequiredQuantity GetRequiredQuantityBasedOnPoundsPerInch(string coid, decimal poundsPerInch)
        {
            return GetRequiredQuantity(coid, poundsPerInch);
        }

        public RequiredQuantity GetRequiredQuantity(string coid, decimal theoWeight)
        {
            decimal inches = 0;
            decimal pounds = 0;
            //decimal kilos = 0;
            var result = new RequiredQuantity()
            {
                Pieces = Pieces
            };
            var uomManager = new UomManager();
            if (WeightBased)
            {
                inches = uomManager.GetLengthFromWeight(coid, Quantity, QuantityType, "in", theoWeight);
                result.PieceLength = LengthMeasure.FromInches(inches);
                pounds = uomManager.Convert(Quantity, UomType.Weight, QuantityType, "lb");
                result.PieceWeight = WeightMeasure.FromPounds(pounds);
            }
            else if (LengthBased)
            {
                if (uomManager.GetBaseWeightUnitForCoid(coid) == "kg")
                {
                    pounds = uomManager.GetWeightFromLength(coid, Quantity, QuantityType, theoWeight) * (decimal)2.20462;
                    //kilos = uomManager.Convert(pounds, UomType.Weight, "lb", "kg");
                }
                else
                {
                    pounds = uomManager.GetWeightFromLength(coid, Quantity, QuantityType, theoWeight);
                }
                result.PieceWeight = WeightMeasure.FromPounds(pounds);

                inches = uomManager.Convert(Quantity, UomType.Length, QuantityType, "in");
                result.PieceLength = LengthMeasure.FromInches(inches);
            }

            return result;
        }

        public decimal GetTotalPounds()
        {
            if (LengthBased) return 0;
            var factor = (QuantityType == "lb") ? (decimal) 1 : (decimal) 0.453592;
            return Quantity * Pieces * factor;
        }

        public decimal GetTotalKilos()
        {
            if (LengthBased) return 0;
            var factor = (QuantityType == "kg") ? (decimal)1 : (decimal)2.20462;
            return Quantity * Pieces * factor;
        }
    }
}
