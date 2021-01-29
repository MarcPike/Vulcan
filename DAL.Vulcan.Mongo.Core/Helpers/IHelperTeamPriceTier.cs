using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.Team_Settings;
using DAL.Vulcan.Mongo.Core.TeamSettings;

namespace DAL.Vulcan.Mongo.Core.Helpers
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