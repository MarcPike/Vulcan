using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;

namespace DAL.Common.Tests
{
    [TestFixture()]
    public class InitializeLocations
    {
        [Test]
        public void Execute()
        {
            InitializerEntity.Initialize();
            InitializerLocation.Initialize();
            var queryHelper = new CommonMongoRawQueryHelper<Entity>();
            var entities = queryHelper.Find(queryHelper.FilterBuilder.Empty).ToList();
            foreach (var entity in entities)
            {
                InitializerEntity.UpdateLocationList(entity);
            }

            //GetLocationDataFromVulcan();
            CreatePayrollRegion();
        }

        //private void GetLocationDataFromVulcan()
        //{
        //    var queryHelperLocation = new CommonMongoRawQueryHelper<Location>();
        //    var locations = queryHelperLocation.GetAll();
        //    foreach (var location in locations)
        //    {
        //        var vulcanLocation = GetVulcanLocation(location.Office);
        //        if (vulcanLocation == null) continue;
        //        if (vulcanLocation.DefaultCurrency != null)
        //        {
        //            location.DefaultCurrency = CurrencyType.GetCurrencyTypeRefFor(vulcanLocation.DefaultCurrency.Code);
        //        }

        //        if (vulcanLocation.Teams != null)
        //        {
        //            var docList = new ExternalDocumentList();
        //            docList.Name = "Teams";
        //            foreach (var vulcanLocationTeam in vulcanLocation.Teams)
        //            {
        //                docList.Documents.Add(vulcanLocationTeam.ToBsonDocument());
        //            }

        //            location.SaveExternalDocumentList(docList);
        //        }

        //        queryHelperLocation.Upsert(location);
        //    }
        //}

        private void CreatePayrollRegion()
        {
            PayrollRegion.PopulateValues();
            var payrollRegions = new CommonRepository<PayrollRegion>().AsQueryable().ToList();

            var repLocations = new CommonRepository<Location>();
            var locations = repLocations.AsQueryable().ToList();

            foreach (var location in locations.Where(x => x.Country == "China").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("China"));
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Region == "Europe").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Europe"));
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Country == "Malaysia").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Malaysia"));
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Country == "UAE").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Middle East"));
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Country == "Singapore").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Singapore"));
                repLocations.Upsert(location);
            }

            foreach (var location in locations.Where(x => x.Region == "Western Hemisphere").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Western Hemisphere"));
                repLocations.Upsert(location);
            }

            locations = repLocations.AsQueryable().ToList();
            foreach (var location in locations.Where(x => x.PayrollRegions.Count == 0).ToList())
            {
                Console.WriteLine(location.Office);
            }

            PayrollRegionRef GetPayrollRegionFor(string name)
            {
                return payrollRegions.First(x => x.Name == name).AsPayrollRegionRef();
            }
        }



        //private DAL.Vulcan.Mongo.DocClass.Locations.Location GetVulcanLocation(string locationOffice)
        //{
        //    EnvironmentSettings.CrmProduction();
        //    return new RepositoryBase<DAL.Vulcan.Mongo.DocClass.Locations.Location>().AsQueryable()
        //        .FirstOrDefault(x => x.Office == locationOffice);

        //}
    }
}
