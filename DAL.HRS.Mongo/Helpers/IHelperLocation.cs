using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.Common.Models;
using DAL.HRS.Mongo.Models;
using MapLocation = DAL.Common.DocClass.MapLocation;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperLocation
    {
        List<Location> GetAllLocations();
        Location GetLocation(string locationId);

        LocationModel GetNewLocationModel();

        MapLocation GetNewMapLocation(double x, double y);

        LocationModel SaveLocation(LocationModel model);
        HrRepresentativeModel SaveHrRepresentative(HrRepresentativeModel model);
    }
}