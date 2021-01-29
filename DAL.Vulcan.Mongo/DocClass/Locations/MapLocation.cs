using MongoDB.Driver.GeoJsonObjectModel;

namespace DAL.Vulcan.Mongo.DocClass.Locations
{
    public class MapLocation
    {
        public GeoJson2DCoordinates Coordinates { get; set; }

        public MapLocation(double x, double y)
        {
            Coordinates = new GeoJson2DCoordinates(x,y);
        }
    }
}