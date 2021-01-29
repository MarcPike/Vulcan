namespace DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Lengths
{
    public class Centimeter : BaseUnitOfMeasure
    {
        public override decimal GetBaseUnitFactor()
        {
            return (decimal)0.393701;
        }

        public Centimeter()
        {
            Name = "cm";
            UomType = UomType.Length;
        }
    }
}