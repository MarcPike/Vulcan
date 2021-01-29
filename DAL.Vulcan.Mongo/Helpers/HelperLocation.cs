using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Locations;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Helpers
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

        public List<Location> GetAllLocations()
        {
            return new RepositoryBase<Location>().AsQueryable().ToList();
        }
    }
}