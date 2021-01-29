using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;
using DAL.Vulcan.Mongo.Team_Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vulcan.IMetal.Context.StockItems;
using Vulcan.IMetal.Helpers;

namespace DAL.Vulcan.Mongo.TeamSettings
{
    [BsonIgnoreExtraElements]
    public class TeamPriceTier: BaseDocument
    {
        public TeamRef Team { get; set; }
        public string Currency { get; set; }

        public List<TierPriceDefinition> PriceDefinitions { get; set; } = new List<TierPriceDefinition>();
        [JsonConverter(typeof(StringEnumConverter))] // JSON.Net
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
