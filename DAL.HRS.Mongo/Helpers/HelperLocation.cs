using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.Common.Models;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using MapLocation = DAL.Common.DocClass.MapLocation;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperLocation : HelperBase, IHelperLocation
    {
        public Location GetLocation(string locationId)
        {
            var location = new RepositoryBase<Location>().AsQueryable()
                .FirstOrDefault(x => x.Id == ObjectId.Parse(locationId));
            if (location == null) throw new Exception("Invalid location");
            return location;
        }

        public LocationModel GetNewLocationModel()
        {
            return new LocationModel(new Location());
        }

        public MapLocation GetNewMapLocation(double x, double y)
        {
            return new MapLocation(x,y);
        }

        public LocationModel SaveLocation(LocationModel model)
        {
            var queryHelper = new MongoRawQueryHelper<Location>();

            var location = queryHelper.FindById(model.Id);
            if (location == null)
            {
                location = new Location()
                {
                    Id = ObjectId.Parse(model.Id)
                };
            }
                
            location.Branch = model.Branch;
            location.Region = model.Region;
            location.Country = model.Country;
            location.Office = model.Office;
            location.Phone = model.Phone;
            location.Fax = model.Fax;
            location.PhoneTollFree = model.PhoneTollFree;
            location.Addresses = model.Addresses;
            location.MapLocation = model.MapLocation;
            location.PayrollRegions = model.PayrollRegions;
            location.Entity = model.Entity;

            // NOTE: Validation code is within the Validate() method of Location

            location = queryHelper.Upsert(location);
            return new LocationModel(location);

        }

        public List<Location> GetAllLocations()
        {
            return new RepositoryBase<Location>().AsQueryable().ToList();
        }

        public HrRepresentativeModel SaveHrRepresentative(HrRepresentativeModel model)
        {
            var hrRep = HrRepresentative.Helper.Find(x => x.Location.Id == model.Location.Id).FirstOrDefault() ??
                        new HrRepresentative()
                        {
                            Location = model.Location
                        };
            hrRep.Representative = model.Representative;
            HrRepresentative.Helper.Upsert(hrRep);

            return new HrRepresentativeModel(hrRep);
        }
    }
}