using DAL.Vulcan.Mongo.Core.Quotes;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class ProductionStepHeatTreat : ProductionStepCostBase
    {
        public ProductionStepHeatTreat(string coid, string startingProductCode, string finishedProductCode,
            RequiredQuantity requiredQuantity, bool isPriceBlended) :
            base(coid, startingProductCode, finishedProductCode, requiredQuantity, ResourceType.HeatTreat, isPriceBlended)
        {

        }
    }

    //public class CostValueTemplate
    //{
    //    public string 
    //}
}