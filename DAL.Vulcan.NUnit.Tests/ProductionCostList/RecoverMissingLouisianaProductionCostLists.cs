using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.ProductionCostList
{
    [TestFixture]
    public class RecoverMissingLouisianaProductionCostLists
    {
        [Test]
        public void Execute()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var resourceTypes = Enum.GetNames(typeof(ResourceType));

            var repProductionCostList = new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>();

            var location = new RepositoryBase<Location>().AsQueryable().SingleOrDefault(x=>x.Office == "Gray").AsLocationRef();

            var team = new RepositoryBase<Team>().AsQueryable().Single(x=>x.Name == "Louisiana Ragin Cajuns").AsTeamRef();

            var quotes = new RepositoryBase<CrmQuote>().AsQueryable(new AggregateOptions { AllowDiskUse = true })
                .Where(x => x.Team.Id == team.Id).Where(x=>x.CreateDateTime <= DateTime.Now.AddDays(-3)).OrderByDescending(x => x.CreateDateTime).Take(500).ToList();
            foreach (var crmQuote in quotes)
            {
                foreach (var item in crmQuote.Items.Select(x=>x.AsQuoteItem()).Where(x=> x.CalculateQuotePriceModel != null ).ToList()) 
                {
                    foreach (var productionCost in item.QuotePrice.ProductionCosts)
                    {
                        if (!repProductionCostList.AsQueryable().Any(x =>
                            x.Coid == "XXX" &&
                            x.Location.Id == location.Id &&
                            x.ResourceType == productionCost.ResourceType))
                        {
                            var newProductionList = new Mongo.DocClass.Quotes.ProductionCostList()
                            {
                                Coid = "XXX",
                                CostValues = productionCost.CostValues,
                                Location = location,
                                ResourceType = productionCost.ResourceType,
                            };

                            repProductionCostList.Upsert(newProductionList);

                        }
                    }
                }
            }

            var removeProductionLists = repProductionCostList.AsQueryable()
                .Where(x => x.Location.Id == location.Id && x.Coid == "INC");

            foreach (var removeProductionList in removeProductionLists)
            {
                removeProductionList.Coid = "OLD";
                repProductionCostList.Upsert(removeProductionList);
            }

            var changeCoidToInc = repProductionCostList.AsQueryable()
                .Where(x => x.Location.Id == location.Id && x.Coid == "XXX");
            foreach (var productionCostList in changeCoidToInc)
            {
                productionCostList.Coid = "INC";
                repProductionCostList.Upsert(productionCostList);
            }
        }
    }
}
