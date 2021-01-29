using Gram = DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Weights.Gram;
using KiloGram = DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Weights.KiloGram;
using Pounds = DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Weights.Pounds;
using ShortTon = DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Weights.ShortTon;
using Tonnes = DAL.Vulcan.Mongo.Core.UnitConversions.UOM_Objects.Weights.Tonnes;

namespace DAL.Vulcan.Mongo.Core.UnitConversions
{
    public static class DefineWeights
    {
        public static void AddWeights(UnitOfMeasures measures)
        {
            measures.RegisterUom(new Gram());
            measures.RegisterUom(new KiloGram());
            measures.RegisterUom(new Pounds());
            measures.RegisterUom(new Tonnes());
            measures.RegisterUom(new ShortTon());
        }
    }
}