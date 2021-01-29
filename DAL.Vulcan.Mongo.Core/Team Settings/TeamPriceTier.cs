using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.TeamSettings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.Team_Settings
{
    [BsonIgnoreExtraElements]
    public class TeamPriceTier: BaseDocument
    {
        public TeamRef Team { get; set; }
        public string Currency { get; set; }

        public List<TierPriceDefinition> PriceDefinitions { get; set; } = new List<TierPriceDefinition>();
        [JsonConverter(typeof(JsonStringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)] // Mongo
        public WeightType WeightType { get; set; } = WeightType.Lbs;

        public TeamPriceTier()
        {
        }

        public static TeamPriceTier GetForTeam(Team team)
        {
            var rep = new RepositoryBase<TeamPriceTier>();
            var result = rep.AsQueryable().FirstOrDefault(x => x.Team.Id == team.Id.ToString());
            
            if (result == null)
            {
                var weightType = WeightType.Kg;
                var teamLocation = team.Location.AsLocation();
                if ((teamLocation.Country == "United States") || (teamLocation.Country == "Canada"))
                {
                    weightType = WeightType.Lbs;
                }
                else
                {
                    weightType = WeightType.Kg;
                }
                result = new TeamPriceTier()
                {
                    Team = team.AsTeamRef(),
                    Currency = team.Location.AsLocation().DefaultCurrency.Code,
                    WeightType = weightType
                };
            }

            return result;
        }

    }
}
