using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.Quotes;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    //public class PiecesValue
    //{
    //    public LengthMeasure PieceLength { get; set; } = new LengthMeasure() {Centimeters = 0,Feet = 0,Inches = 0,Meters = 0,Millimeters = 0};
    //    public WeightMeasure PieceWeight { get; set; } = new WeightMeasure() {Kilograms = 0,Pounds = 0};
    //    public int Pieces { get; set; } = 0;

    //    public PiecesValue()
    //    {

    //    }

    //    public PiecesValue(RequiredQuantity requiredQuantity)
    //    {
    //        Pieces = requiredQuantity.Pieces;
    //        PieceLength = requiredQuantity.PieceLength;
    //        PieceWeight = requiredQuantity.PieceWeight;
    //    }

    //    public PiecesValue(OrderQuantity orderQuantity, string coid, decimal theoWeight)
    //    {
    //        var requiredQuantity = orderQuantity.GetRequiredQuantity(coid, theoWeight);

    //        Pieces = requiredQuantity.Pieces;
    //        PieceLength = requiredQuantity.PieceLength;
    //        PieceWeight = requiredQuantity.PieceWeight;
    //    }

    //    public static decimal GetTotalPounds(PiecesValue soldPieces, List<PiecesValue> testPieces)
    //    {
    //        return (soldPieces.Pieces * soldPieces.PieceWeight.Pounds) +
    //               (testPieces.Sum(x => x.Pieces * x.PieceWeight.Pounds));
    //    }
    //    public static decimal GetTotalKilograms(PiecesValue soldPieces, List<PiecesValue> testPieces)
    //    {
    //        return (soldPieces.Pieces * soldPieces.PieceWeight.Kilograms) +
    //               (testPieces.Sum(x => x.Pieces * x.PieceWeight.Kilograms));
    //    }

    //    public static decimal GetTotalFeet(PiecesValue pieces, List<PiecesValue> testPieces)
    //    {
    //        return (pieces.Pieces * pieces.PieceLength.Feet) +
    //               (testPieces.Sum(x => x.Pieces * x.PieceLength.Feet));
    //    }

    //    public static decimal GetTotalInches(PiecesValue soldPieces, List<PiecesValue> testPieces)
    //    {
    //        return (soldPieces.Pieces * soldPieces.PieceLength.Inches) +
    //               (testPieces.Sum(x => x.Pieces * x.PieceLength.Inches));
    //    }

    //    public static int GetTotalPieces(PiecesValue soldPieces, List<PiecesValue> testPieces)
    //    {
    //        return (soldPieces.Pieces) + (testPieces.Sum(x => x.Pieces));
    //    }

    //    public static decimal GetTotalPounds(PiecesValue soldPieces)
    //    {
    //        return (soldPieces.Pieces * soldPieces.PieceWeight.Pounds);
    //    }
    //    public static decimal GetTotalKilograms(PiecesValue soldPieces)
    //    {
    //        return (soldPieces.Pieces * soldPieces.PieceWeight.Kilograms);
    //    }

    //    public static decimal GetTotalFeet(PiecesValue pieces)
    //    {
    //        return (pieces.Pieces * pieces.PieceLength.Feet);
    //    }

    //    public static decimal GetTotalInches(PiecesValue soldPieces)
    //    {
    //        return (soldPieces.Pieces * soldPieces.PieceLength.Inches);
    //    }

    //    public static int GetTotalPieces(PiecesValue soldPieces)
    //    {
    //        return (soldPieces.Pieces);
    //    }

    //    public static decimal GetTotalPounds(List<PiecesValue> pieces)
    //    {
    //        return pieces.Sum(x=> x.Pieces * x.PieceWeight.Pounds);
    //    }
    //    public static decimal GetTotalKilograms(List<PiecesValue> pieces)
    //    {
    //        return pieces.Sum(x=> x.Pieces * x.PieceWeight.Kilograms);
    //    }

    //    public static decimal GetTotalFeet(List<PiecesValue> pieces)
    //    {
    //        return pieces.Sum(x=>x.Pieces * x.PieceLength.Feet);
    //    }

    //    public static decimal GetTotalInches(List<PiecesValue> pieces)
    //    {
    //        return pieces.Sum(x=> x.Pieces * x.PieceLength.Inches);
    //    }

    //    public static int GetTotalPieces(List<PiecesValue> pieces)
    //    {
    //        return pieces.Sum(x=> x.Pieces);
    //    }


    //    public decimal Pounds()
    //    {
    //        return Pieces * PieceWeight.Pounds;
    //    }
    //    public decimal Kilograms()
    //    {
    //        return Pieces * PieceWeight.Kilograms;
    //    }

    //    public decimal Feet()
    //    {
    //        return Pieces * PieceLength.Feet;
    //    }

    //    public decimal Inches()
    //    {
    //        return Pieces * PieceLength.Inches;
    //    }

    //    public void ChangeTheoWeight(decimal theoWeight)
    //    {
    //        PieceWeight.Pounds = PieceLength.Inches * theoWeight;
    //        PieceWeight.Kilograms = PieceWeight.Pounds * (decimal) 0.453592;
    //    }
    //}
}