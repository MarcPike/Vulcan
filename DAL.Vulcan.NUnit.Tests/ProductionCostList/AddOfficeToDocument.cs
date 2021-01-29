using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Locations;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.ProductionCostList
{
    [TestFixture]
    public class AddOfficeToDocument
    {
        [Test]
        public void Execute()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var telge = new RepositoryBase<Location>().AsQueryable().Single(x => x.Office == "Telge").AsLocationRef();

            var rep = new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>();
            var allProductionCostLists = rep.AsQueryable().ToList();
            foreach (var productionCostList in allProductionCostLists)
            {
                productionCostList.Location = telge;
                rep.Upsert(productionCostList);
            }

        }
    }
}
