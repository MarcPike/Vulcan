using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.Locations
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