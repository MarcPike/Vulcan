namespace DAL.Vulcan.Mongo.UnitConversions.UOM_Objects.Lengths
{
    public class Inch : BaseUnitOfMeasure
    {
        public override decimal GetBaseUnitFactor()
        {
            return (decimal)1;
        }

        public Inch()
        {
            Name = "in";
            UomType = UomType.Length;
        }
    }
}