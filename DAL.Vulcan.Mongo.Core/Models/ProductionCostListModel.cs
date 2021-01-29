using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class ProductionCostListModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Coid { get; set; }
        public LocationRef Location { get; set; }
        public ResourceType ResourceType { get; set; }
        public string ResourceTypeName => ResourceType.ToString();
        public List<CostValue> CostValues { get; set; } = new List<CostValue>();


        public ProductionCostListModel()
        {

        }
        public ProductionCostListModel(string application, string userId, ProductionCostList value, string teamCurrency)
        {
            Coid = value.Coid;
            ResourceType = value.ResourceType;
            var missingCurrency = false;
            foreach (var costValue in value.CostValues)
            {
                if (costValue.Currency == string.Empty)
                {
                    costValue.Currency = teamCurrency;
                    missingCurrency = true;
                }
            }

            if (missingCurrency)
            {
                value.SaveToDatabase();
            }

            CostValues = value.CostValues;
            Location = value.Location;
            Application = application;
            UserId = userId;
        }
    }
}
