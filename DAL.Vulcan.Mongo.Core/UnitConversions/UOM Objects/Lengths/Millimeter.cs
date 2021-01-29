namespace DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Lengths
{
    public class Millimeter : BaseUnitOfMeasure
    {
        public override decimal GetBaseUnitFactor()
        {
            return (decimal)0.0393701;
        }

        public Millimeter()
        {
            Name = "mm";
            UomType = UomType.Length;
        }
    }
}