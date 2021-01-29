using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.DocClass.Croz.Models
{
    public class CrozProductionCostListModel
    {
        public string Region { get; set; } = string.Empty;
        public List<CrozProductionCost> ProductionCosts { get; set; } = new List<CrozProductionCost>();
        public string UserToken { get; set; } = string.Empty;

        public CrozProductionCostListModel()
        {
            
        }

        public CrozProductionCostListModel(string userToken, string region)
        {
            UserToken = userToken;
            Region = region;
            var value = CrozProductionCostList.Helper.
                Find(x => x.Region == region).FirstOrDefault();

            if (value != null)
            {
                ProductionCosts = value.ProductionCosts;
                UserToken = userToken;
            }
        }

        public CrozProductionCostListModel(CrozProductionCostList list)
        {
            Region = list.Region;
            ProductionCosts = list.ProductionCosts;
        }

        public static List<CrozProductionCostListModel> GetAll(string userToken)
        {
            var result = new List<CrozProductionCostListModel>();
            foreach (var crozProductionCostList in CrozProductionCostList.Helper.GetAll())
            {
                result.Add(new CrozProductionCostListModel(userToken, crozProductionCostList.Region));
            }
            return result;
        }
        
    }
}
