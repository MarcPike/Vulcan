namespace DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Weights
{
    public class Tonnes : BaseUnitOfMeasure
    {
        public override decimal GetBaseUnitFactor()
        {
            return (decimal)1000000;
        }

        public Tonnes()
        {
            Name = "Tne";
            UomType = UomType.Weight;
        }
    }
}