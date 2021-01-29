using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.ProductionCostList
{
    [TestFixture]
    public class CopyProductionCostList
    {
        [Test]
        public void Lafeyette()
        {
            var locations = new RepositoryBase<Location>().AsQueryable().ToList();
            //foreach (var location in locations)
            //{
            //    Console.WriteLine(ObjectDumper.Dump(location));
            //}

            var lafayette = locations.Single(x => x.Office == "Lafayette");
            var telge = locations.Single(x => x.Office == "Telge");

            var productionLists = new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>().AsQueryable().ToList();

            foreach (var productionCostList in productionLists.Where(x=>x.Location.Id == telge.Id.ToString()).ToList())
            {
                if (new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>().AsQueryable().Any(x =>
                    x.Coid == productionCostList.Coid && x.Location.Id == lafayette.Id.ToString() &&
                    x.ResourceType == productionCostList.ResourceType))
                {
                    continue;
                }

                var newProductionCostList = new Mongo.DocClass.Quotes.ProductionCostList()
                {
                    Coid = productionCostList.Coid,
                    ResourceType = productionCostList.ResourceType,
                    CostValues = productionCostList.CostValues,
                    CreatedByUserId = "mpike",
                    CreateDateTime = DateTime.Now,
                    Location = lafayette.AsLocationRef()
                };
                newProductionCostList.SaveToDatabase();
            }

        }
        [Test]
        public void Gray()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var locations = new RepositoryBase<Location>().AsQueryable().ToList();
            //foreach (var location in locations)
            //{
            //    Console.WriteLine(ObjectDumper.Dump(location));
            //}

            var gray = locations.Single(x => x.Office == "Gray");
            var telge = locations.Single(x => x.Office == "Telge");

            var productionLists = new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>().AsQueryable().ToList();

            foreach (var productionCostList in productionLists.Where(x => x.Location.Id == telge.Id.ToString()).ToList())
            {
                if (new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>().AsQueryable().Any(x =>
                    x.Coid == productionCostList.Coid && x.Location.Id == gray.Id.ToString() &&
                    x.ResourceType == productionCostList.ResourceType))
                {
                    continue;
                }

                var newProductionCostList = new Mongo.DocClass.Quotes.ProductionCostList()
                {
                    Coid = productionCostList.Coid,
                    ResourceType = productionCostList.ResourceType,
                    CostValues = productionCostList.CostValues,
                    CreatedByUserId = "mpike",
                    CreateDateTime = DateTime.Now,
                    Location = gray.AsLocationRef()
                };
                newProductionCostList.SaveToDatabase();
            }

        }

        [Test]
        public void Cumbernauld()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var locations = new RepositoryBase<Location>().AsQueryable().ToList();
            //foreach (var location in locations)
            //{
            //    Console.WriteLine(ObjectDumper.Dump(location));
            //}

            var telge = locations.Single(x => x.Office == "Telge");
            var cumbernauld = locations.Single(x => x.Office == "Cumbernauld");
            var destinationCoid = cumbernauld.GetCoid();
            var destinationOffice = cumbernauld.Office;

            var productionLists = new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>().AsQueryable().ToList();
            var rep = new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>();
            foreach (var productionCostList in productionLists.Where(x => x.Location.Id == telge.Id.ToString() && x.Coid == telge.GetCoid()).ToList())
            {
                if (rep.AsQueryable().Any(x=>x.Coid == destinationCoid && x.ResourceType == productionCostList.ResourceType && x.Location.Office == destinationOffice))
                { 
                    continue;
                }

                //if (new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>().AsQueryable().Any(x =>
                //    x.Coid == productionCostList.Coid && x.Location.Id == telge.Id.ToString() &&
                //    x.ResourceType == productionCostList.ResourceType))
                //{
                //    continue;
                //}

                var newProductionCostList = new Mongo.DocClass.Quotes.ProductionCostList()
                {
                    Coid = destinationCoid,
                    ResourceType = productionCostList.ResourceType,
                    CostValues = productionCostList.CostValues,
                    CreatedByUserId = "mpike",
                    CreateDateTime = DateTime.Now,
                    Location = cumbernauld.AsLocationRef()
                };
                newProductionCostList.SaveToDatabase();
            }

        }

        [Test]
        public void Sheffield()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var locations = new RepositoryBase<Location>().AsQueryable().ToList();
            //foreach (var location in locations)
            //{
            //    Console.WriteLine(ObjectDumper.Dump(location));
            //}

            var telge = locations.Single(x => x.Office == "Telge");
            var sheffield = locations.Single(x => x.Office == "Sheffield");
            var destinationCoid = sheffield.GetCoid();
            var destinationOffice = sheffield.Office;

            var productionLists = new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>().AsQueryable().ToList();
            var rep = new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>();
            foreach (var productionCostList in productionLists.Where(x => x.Location.Id == telge.Id.ToString() && x.Coid == telge.GetCoid()).ToList())
            {
                if (rep.AsQueryable().Any(x => x.Coid == destinationCoid && x.ResourceType == productionCostList.ResourceType && x.Location.Office == destinationOffice))
                {
                    continue;
                }

                //if (new RepositoryBase<Mongo.DocClass.Quotes.ProductionCostList>().AsQueryable().Any(x =>
                //    x.Coid == productionCostList.Coid && x.Location.Id == telge.Id.ToString() &&
                //    x.ResourceType == productionCostList.ResourceType))
                //{
                //    continue;
                //}

                var newProductionCostList = new Mongo.DocClass.Quotes.ProductionCostList()
                {
                    Coid = destinationCoid,
                    ResourceType = productionCostList.ResourceType,
                    CostValues = productionCostList.CostValues,
                    CreatedByUserId = "mpike",
                    CreateDateTime = DateTime.Now,
                    Location = sheffield.AsLocationRef()
                };
                newProductionCostList.SaveToDatabase();
            }


        }


    }
}
