namespace DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Weights
{
    public class Pounds : BaseUnitOfMeasure
    {
        public override decimal GetBaseUnitFactor()
        {
            return (decimal)453.59237;
        }

        public Pounds()
        {
            Name = "lb";
            UomType = UomType.Weight;
        }
    }
}