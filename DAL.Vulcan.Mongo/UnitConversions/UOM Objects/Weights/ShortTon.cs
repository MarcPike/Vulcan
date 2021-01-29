namespace DAL.Vulcan.Mongo.UnitConversions.UOM_Objects.Weights
{
    public class ShortTon : BaseUnitOfMeasure
    {
        public override decimal GetBaseUnitFactor()
        {
            return (decimal)907184.74;
        }

        public ShortTon()
        {
            Name = "Ton";
            UomType = UomType.Weight;
        }
    }
}