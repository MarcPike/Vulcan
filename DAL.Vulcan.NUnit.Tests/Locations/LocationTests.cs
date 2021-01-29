using System.Diagnostics;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Test;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Locations
{
    [TestFixture]
    public class LocationTests
    {
        [Test]
        public void GetLocations()
        {
            Location.GenerateDefaults();
            var rep = new RepositoryBase<Location>();
            foreach (var location in IAsyncCursorSourceExtensions.ToList(rep.AsQueryable()))
            {
                Trace.WriteLine(ObjectDumper.Dump(location));
            }
        }

        [Test]
        public void GetUniqueOffices()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            //Location.GenerateDefaults();
            var rep = new RepositoryBase<Location>();
            foreach (var office in rep.AsQueryable().Select(x=>x.Office).Distinct().OrderBy(x=>x))
            {
                System.Console.WriteLine(office);
            }
        }

        //[TestMethod]
        //public void SynchronizeIdentityLocations()
        //{
        //    var identityLocationRep = new AspNetCore.Identity.MongoDB.Repository.RepositoryBase<LocationRef>();
        //    Location.GenerateDefaults();
        //    var rep = new RepositoryBase<Location>();
        //    foreach (var location in rep.AsQueryable().ToList())
        //    {
        //        var locationRef = identityLocationRep.AsQueryable().SingleOrDefault(x => x.LocationId == location.Id);
        //        if (locationRef == null)
        //        {
        //            locationRef = new LocationRef()
        //            {
        //                LocationId = location.Id,
        //                Region = location.Region,
        //                Office = location.Office,
        //                Branch = location.Branch,
        //                Country = location.Country,
        //                CreatedByUserId = "admin"
        //            };

        //            identityLocationRep.Upsert(locationRef);
        //        }
        //    }

        //}
    }
}
