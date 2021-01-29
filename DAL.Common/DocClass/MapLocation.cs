using MongoDB.Driver.GeoJsonObjectModel;

namespace DAL.Common.DocClass
{
    public class MapLocation
    {
        public GeoJson2DCoordinates Coordinates { get; set; }

        public MapLocation(double x, double y)
        {
            Coordinates = new GeoJson2DCoordinates(x,y);
        }

        public MapLocation()
        {
            Coordinates = new GeoJson2DCoordinates(0,0);
        }
    }
}