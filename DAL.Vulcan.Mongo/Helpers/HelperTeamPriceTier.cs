using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.TeamSettings;
using MongoDB.Bson;
using Vulcan.IMetal.Context.StockItems;
using Vulcan.IMetal.Helpers;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperTeamPriceTier : IHelperTeamPriceTier
    {
        public TeamPriceTier GetTeamPriceTier(string teamId)
        {
            var team = new RepositoryBase<Team>().Find(teamId);

            return TeamPriceTier.GetForTeam(team);

        }

        public TeamPriceTierModel GetTeamPriceTierModel(string teamId, string application, string userId)
        {
            return new TeamPriceTierModel(GetTeamPriceTier(teamId), application, userId);
        }

        public TierPriceDefinitionModel GetNewPriceDefinitionModel()
        {
            var newDefinition = new TierPriceDefinition()
            {
            };

            var model = new TierPriceDefinitionModel(newDefinition);
            model.IsDirty = true;

            return model;
        }

        public BasePriceDimensionModel GetNewBasePriceDimensionModel()
        {
            var newDim = new BasePriceDimension();
            return new BasePriceDimensionModel(newDim);
        }

        public WeightDiscountModel GetNewWeightDiscountModel()
        {
            var weightDiscount = new WeightDiscount();
            return new WeightDiscountModel(weightDiscount);
        }

        public TeamPriceTierModel SaveTeamPriceTier(TeamPriceTierModel model)
        {
            var teamPriceTier = new RepositoryBase<TeamPriceTier>().Find(model.Id) ?? new TeamPriceTier()
            {
                Id = ObjectId.Parse(model.Id),
                Team = model.Team,
                CreatedByUserId = model.UserId
            };

            teamPriceTier.Currency = model.Currency;
            teamPriceTier.WeightType = model.WeightType;

            teamPriceTier.PriceDefinitions.Clear();
            foreach (var priceDefinition in model.PriceDefinitions)
            {
                var thisId = Guid.Parse(priceDefinition.Id);
                var thisDef = new TierPriceDefinition()
                    {
                        Id = thisId,
                        StockGrade = priceDefinition.StockGrade,
                        ProductCondition = priceDefinition.ProductCondition
                    };

                foreach (var basePriceDimension in priceDefinition.BasePriceDimensions)
                {
                    var thisBasePrice = new BasePriceDimension()
                    {
                        Id = Guid.Parse(basePriceDimension.Id),
                        OuterDiameterMin = basePriceDimension.OuterDiameterMin,
                        OuterDiameterMax = basePriceDimension.OuterDiameterMax,
                        BasePrice = basePriceDimension.BasePrice
                    };
                    foreach (var weightDiscount in basePriceDimension.WeightDiscounts)
                    {
                        var thisWeightDiscount = new WeightDiscount()
                        {
                            Id = Guid.Parse(weightDiscount.Id),
                            MinWeight = weightDiscount.MinWeight,
                            MaxWeight = weightDiscount.MaxWeight,
                            BasePriceFactor = weightDiscount.BasePriceFactor
                        };
                        thisBasePrice.WeightDiscounts.Add(thisWeightDiscount);
                    }

                    thisDef.BasePriceDimensions.Add(thisBasePrice);
                }

                teamPriceTier.PriceDefinitions.Add(thisDef);
            }

            teamPriceTier.SaveToDatabase();

            return new TeamPriceTierModel(teamPriceTier, model.Application, model.UserId);

        }

        public decimal GetPrice(string teamId, ProductMaster productMaster, decimal weight, string displayCurrency)
        {
            var result = (decimal)0;

            var teamPriceTier = GetTeamPriceTier(teamId);

            var priceDefinition = teamPriceTier.PriceDefinitions.FirstOrDefault(x =>
                x.StockGrade == productMaster.StockGrade && x.ProductCondition == productMaster.ProductCondition);

            if (priceDefinition == null) return result;

            var basePriceDimension = priceDefinition.BasePriceDimensions.FirstOrDefault(x =>
                x.OuterDiameterMin <= productMaster.OuterDiameter && x.OuterDiameterMax >= productMaster.OuterDiameter);

            if (basePriceDimension == null) return result;

            result = basePriceDimension.BasePrice;

            var weightDiscount =
                basePriceDimension.WeightDiscounts.FirstOrDefault(x => x.MinWeight <= weight && x.MaxWeight >= weight);

            if (weightDiscount != null)
            {
                result = result + (result * weightDiscount.BasePriceFactor);
            }

            if ((teamPriceTier.Currency != displayCurrency) && (result != 0))
            {
                var helperCurrency = new HelperCurrencyForIMetal();
                result = helperCurrency.ConvertValueFromCurrencyToCurrency(result, teamPriceTier.Currency, displayCurrency);
            }

            return result;
        }

    }
}
