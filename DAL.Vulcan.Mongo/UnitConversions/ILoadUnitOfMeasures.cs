

namespace DAL.Vulcan.Mongo.UnitConversions
{
    public interface IUomManager
    {
        UnitOfMeasures GetUnitOfMeasures();
        BaseUnitOfMeasure GetBaseForUomType(UomType type);
        decimal Convert(decimal value, BaseUnitOfMeasure from, BaseUnitOfMeasure to);
    }
}