namespace DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Lengths
{
    public class Yard : BaseUnitOfMeasure
    {
        public override decimal GetBaseUnitFactor()
        {
            return (decimal)36;
        }

        public Yard()
        {
            Name = "yd";
            UomType = UomType.Length;
        }
    }
}