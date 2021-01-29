using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.ProductionCostList
{
    [TestFixture]
    public class FindHeatTreatQuoteWithFewestItems
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
        }

        [Test]
        public void Execute()
        {
            var rep = new RepositoryBase<CrmQuoteItem>();
            var quoteItemsWithHeatTreat = rep.AsQueryable().Where(x =>
                x.CalculateQuotePriceModel.ProductionCosts.Any(p => p.ResourceType == ResourceType.HeatTreat)).ToList();

            foreach (var crmQuoteItem in quoteItemsWithHeatTreat)
            {
                var heatTreatProductionCost =
                    crmQuoteItem.CalculateQuotePriceModel.ProductionCosts.Single(x =>
                        x.ResourceType == ResourceType.HeatTreat);
                if (heatTreatProductionCost.CostValues.Count() <= 10)
                {
                    var quote = crmQuoteItem.GetQuote();
                    Console.WriteLine($"{quote.QuoteId} has only {heatTreatProductionCost.CostValues.Count()} and has ReportDate == {quote.ReportDate}");
                    foreach (var costValue in heatTreatProductionCost.CostValues)
                    {
                        Console.WriteLine(costValue.TypeName);
                    }
                }

            }

        }
    }
}
