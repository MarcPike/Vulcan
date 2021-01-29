using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Common.DocClass
{
    [BsonIgnoreExtraElements]
    public class LocationRef: ReferenceObject<Location>, ISupportLocationNameChangesNested
    {
        public string Office { get; set; }
        public string Coid { get; set; }
        public string Country { get; set; }
        public string Branch { get; set; }
        public string Region { get; set; } = string.Empty;
        public EntityRef Entity { get; set; }


        public LocationRef(Location document) : base(document)
        {
            Coid = document.Coid;
            Country = document.Country;
            Branch = document.Branch;
            Office = document.Office;
            Entity = document.Entity;
            Region = document.Region;
        }

        public LocationRef()
        {
            
        }
        public Location AsLocation()
        {
            var result = ToBaseDocument();
            if (result == null)
            {
                var queryHelper = new MongoRawQueryHelper<Location>();
                var filter = queryHelper.FilterBuilder.Where(x => x.Office == Office);
                var filterUnknown = queryHelper.FilterBuilder.Where(x => x.Office == "<unknown>");
                result = queryHelper.Find(filter).FirstOrDefault() ?? queryHelper.Find(filterUnknown).FirstOrDefault();
                if (result != null)
                {
                    Id = result.Id.ToString();
                }
            }

            return result;
        }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            if ((Id == locationId) && (Office != newName))
            {
                Office = newName;
                modified = true;
            }

            var realLocation = Location.Helper.FindById(Id);
            if (realLocation.Office != Office)
            {
                Office = realLocation.Office;
                modified = true;
            }
            return modified;
        }
    }
}