using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.Locations;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperLocation
    {
        List<Location> GetAllLocations();
        Location GetLocation(string locationId);
    }
}