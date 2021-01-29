using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Models
{
    public class ProductionCostListModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Coid { get; set; }
        public LocationRef Location { get; set; }
        public ResourceType ResourceType { get; set; }
        public string ResourceTypeName => ResourceType.ToString();
        public List<DocClass.Quotes.CostValue> CostValues { get; set; } = new List<DocClass.Quotes.CostValue>();


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
