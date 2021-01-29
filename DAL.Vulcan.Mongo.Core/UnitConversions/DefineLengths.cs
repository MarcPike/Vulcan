using DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Lengths;
using Centimeter = DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Lengths.Centimeter;
using Foot = DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Lengths.Foot;
using Inch = DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Lengths.Inch;
using Meter = DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Lengths.Meter;
using Yard = DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Lengths.Yard;

namespace DAL.Vulcan.Mongo.Core.UnitConversions
{
    public static class DefineLengths
    {
        public static void AddLengths(UnitOfMeasures measures)
        {
            measures.RegisterUom(new Foot());
            measures.RegisterUom(new Inch());
            measures.RegisterUom(new Yard());
            measures.RegisterUom(new Meter());
            measures.RegisterUom(new Centimeter());
            measures.RegisterUom(new Millimeter());
        }
    }
}