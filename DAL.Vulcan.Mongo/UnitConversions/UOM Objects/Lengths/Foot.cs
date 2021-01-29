namespace DAL.Vulcan.Mongo.UnitConversions.UOM_Objects.Lengths
{
    public class Foot: BaseUnitOfMeasure
    {
        public override decimal GetBaseUnitFactor()
        {
            return (decimal) 12;
        }

        public Foot()
        {
            Name = "ft";
            UomType = UomType.Length;
        }
    }
}
