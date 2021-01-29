namespace DAL.Vulcan.Mongo.UnitConversions.UOM_Objects.Weights
{
    public class Gram: BaseUnitOfMeasure
    {
        public override decimal GetBaseUnitFactor()
        {
            return 1;
        }

        public Gram()
        {
            Name = "g";
            UomType = UomType.Weight;
        }
    }
}
