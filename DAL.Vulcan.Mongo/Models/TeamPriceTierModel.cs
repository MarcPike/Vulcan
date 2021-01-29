using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.TeamSettings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Team_Settings;

namespace DAL.Vulcan.Mongo.Models
{
    public class TeamPriceTierModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public TeamRef Team { get; set; }
        public List<TierPriceDefinitionModel> PriceDefinitions { get; set; } = new List<TierPriceDefinitionModel>();
        public string Currency { get; set; }
        [JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)] // Mongo
        public WeightType WeightType { get; set; } = WeightType.Lbs;

        public TeamPriceTierModel()
        {
        }

        public TeamPriceTierModel(TeamPriceTier tier, string application, string userId)
        {
            Application = application;
            UserId = userId;
            Id = tier.Id.ToString();
            Team = tier.Team;

            if (string.IsNullOrEmpty(tier.Currency))
            {
                tier.Currency = tier.Team.AsTeam().DefaultCurrency;
                tier.SaveToDatabase();
            }

            Currency = tier.Currency;
            PriceDefinitions = tier.PriceDefinitions.Select(x=> new TierPriceDefinitionModel(x)).OrderBy(x=>x.StockGrade).ThenBy(x=>x.ProductCondition).ToList();
        }
    }
}
