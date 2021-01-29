

namespace DAL.Vulcan.Mongo.Core.UnitConversions
{
    public class BaseUnitOfMeasure
    {
        
        public UomType UomType { get; set; }
        public string Name { get; set; }

        public virtual decimal GetBaseUnitFactor()
        {
            return 0;
        }

        public BaseUnitOfMeasure(UomType type, string name)
        {
            UomType = type;
            Name = name;
        }

        public BaseUnitOfMeasure()
        {
        }
    }
}