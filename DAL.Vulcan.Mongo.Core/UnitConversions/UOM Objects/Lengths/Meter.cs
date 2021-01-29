namespace DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Lengths
{
    public class Meter : BaseUnitOfMeasure
    {
        public override decimal GetBaseUnitFactor()
        {
            return (decimal)39.3701;
        }

        public Meter()
        {
            Name = "m";
            UomType = UomType.Length;
        }
    }
}