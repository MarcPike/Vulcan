using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Common.DocClass
{
    public class LocationTimeZoneRef : ReferenceObject<LocationTimeZone>
    {
        public string Name { get; set; }

        public LocationTimeZoneRef()
        {
            
        }

        public LocationTimeZoneRef(LocationTimeZone l)
        {
            Id = l.Id.ToString();
            Name = l.Name;
        }

        public LocationTimeZone AsLocationTimeZone()
        {
            return ToBaseDocument();
        }
    }
}