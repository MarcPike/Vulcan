using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.TeamSettings;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperTeamPriceTier
    {
        TeamPriceTier GetTeamPriceTier(string teamId);
        TeamPriceTierModel GetTeamPriceTierModel(string teamId, string application, string userId);
        TierPriceDefinitionModel GetNewPriceDefinitionModel();
        BasePriceDimensionModel GetNewBasePriceDimensionModel();
        WeightDiscountModel GetNewWeightDiscountModel();
        TeamPriceTierModel SaveTeamPriceTier(TeamPriceTierModel model);
        decimal GetPrice(string teamId, ProductMaster productMaster, decimal weight, string displayCurrency);
    }
}