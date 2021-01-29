using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Team_Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class TeamPriceTierModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public TeamRef Team { get; set; }
        public List<TierPriceDefinitionModel> PriceDefinitions { get; set; } = new List<TierPriceDefinitionModel>();
        public string Currency { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))] // JSON.Net
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
