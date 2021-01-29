using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.Locations
{
    [BsonIgnoreExtraElements]
    public class LocationRef: ReferenceObject<Location>
    {
        public string Office { get; set; }
        public LocationRef(Location document) : base(document)
        {
        }

        public LocationRef()
        {
            
        }
        public Location AsLocation()
        {
            return ToBaseDocument();
        }

    }
}