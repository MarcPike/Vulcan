using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperLocation
    {
        List<Location> GetAllLocations();
        Location GetLocation(string locationId);
    }
}