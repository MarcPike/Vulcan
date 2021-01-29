namespace DAL.Vulcan.Mongo.UnitConversions.UOM_Objects.Weights
{
    public class KiloGram : BaseUnitOfMeasure
    {
        public override decimal GetBaseUnitFactor()
        {
            return 1000;
        }

        public KiloGram()
        {
            Name = "kg";
            UomType = UomType.Weight;
        }
    }
}