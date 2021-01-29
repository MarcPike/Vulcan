using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Test;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.ProductionCostList
{
    [TestFixture()]
    public class GrabProductionListFromQuote
    {
        [SetUp]
        public void Setup()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
        }
        //[Test]
        //public void Refresh()
        //{

        //    var quote = new RepositoryBase<CrmQuote>().AsQueryable().SingleOrDefault(x => x.QuoteId == 8736);
        //    Assert.IsNotNull(quote);

        //    var quoteItem = quote.Items.FirstOrDefault();
        //    Assert.IsNotNull(quoteItem);

        //    var productionCostsForHeatTreat =
        //        quoteItem.ProductionCosts.FirstOrDefault(x => x.ResourceType == ResourceType.HeatTreat);
        //    Assert.IsNotNull(productionCostsForHeatTreat);

        //    var costValues = productionCostsForHeatTreat.CostValues;

        //    var rep = new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>();
        //    var productCostListForHeatTreatForTelge = rep.AsQueryable().
        //        SingleOrDefault(x=>x.Coid == quote.Coid && x.Location.Office == "Telge" && x.ResourceType == ResourceType.HeatTreat);
        //    Assert.IsNotNull(productCostListForHeatTreatForTelge);

        //    //Console.WriteLine("ProductionCostList on Quote");
        //    //Console.WriteLine(ObjectDumper.Dump(costValues));
        //    //Console.WriteLine("ProductionCostList tEmplate");
        //    //Console.WriteLine(ObjectDumper.Dump(productCostListForHeatTreatForTelge));

        //    productCostListForHeatTreatForTelge.CostValues = costValues;

        //    Console.WriteLine("ProductionCostList tEmplate after correction");
        //    Console.WriteLine(ObjectDumper.Dump(productCostListForHeatTreatForTelge));

        //    rep.Upsert(productCostListForHeatTreatForTelge);

        //}

    }
}
