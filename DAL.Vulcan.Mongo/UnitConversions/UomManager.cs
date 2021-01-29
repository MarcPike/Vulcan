using System.Collections.Generic;
using DAL.Vulcan.Mongo.UnitConversions.Exceptions;

namespace DAL.Vulcan.Mongo.UnitConversions
{
    public class UomManager: IUomManager
    {
        private readonly UnitOfMeasures _unitOfMeasures = new UnitOfMeasures();
        public UnitOfMeasures GetUnitOfMeasures()
        {
            return _unitOfMeasures;
        }

        public BaseUnitOfMeasure GetBaseForUomType(UomType type)
        {
            return _unitOfMeasures.GetBaseUnitOfMeasure(type);
        }

        public decimal Convert(decimal value, BaseUnitOfMeasure from, BaseUnitOfMeasure to)
        {
            if (from.UomType != to.UomType) throw new WrongUomTypeConversionException();

            var convertedToBase = from.GetBaseUnitFactor()*value;
            return convertedToBase / to.GetBaseUnitFactor();
        }

        public decimal Convert(decimal value, UomType type, string from, string to)
        {
            var fromUnit = _unitOfMeasures.FindByName(type, from);
            var toUnit = _unitOfMeasures.FindByName(type, to);

            if (fromUnit == null) throw new UnknownUnitOfMeasureException();
            if (toUnit == null) throw new UnknownUnitOfMeasureException();

            return Convert(value, fromUnit, toUnit);

        }

        public bool IsWeightUnit(string unit)
        {
            var weightUnits = new List<string>() {"g", "kg", "lb", "Ton", "Tne"};
            return weightUnits.Contains(unit);
        }

        public bool IsLengthUnit(string unit)
        {
            var lengthUnits = new List<string>() { "cm", "ft", "in", "m", "yd", "mm" };
            return lengthUnits.Contains(unit);
        }

        public string GetBaseWeightUnitForCoid(string coid)
        {
            var coidForKg = new List<string> { "SIN", "MSA", "DUB", "EUR" };
            if (coidForKg.Contains(coid))
            {
                return "kg";
            }
            else
            {
                return "lb";
            }
        }

        public decimal GetWeightFromLength(string coid, decimal length, string fromLengthType, decimal theoWeight)
        {
            var lengthConverted = Convert(length, UomType.Length, fromLengthType, "in");

            var weight = lengthConverted * theoWeight;

            return weight;

        }

        public decimal GetLengthFromWeight(string coid, decimal weight, string fromWeightType, string toLengthType, decimal theoWeight)
        {
            decimal weightConverted;
            var coidForKg = new List<string> {"SIN", "MSA", "DUB", "EUR"};
            if (coidForKg.Contains(coid))
            {
                weightConverted = Convert(weight, UomType.Weight, fromWeightType, "kg");
            }
            else
            {
                weightConverted = Convert(weight, UomType.Weight, fromWeightType, "lb");
            }

            var inches = weightConverted / theoWeight;

            return Convert(inches,UomType.Length, "in", toLengthType);
        }

        public UomManager()
        {
            DefineWeights.AddWeights(_unitOfMeasures);
            DefineLengths.AddLengths(_unitOfMeasures);
        }
    }
}