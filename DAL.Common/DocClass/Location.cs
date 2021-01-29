using System.Collections.Generic;
using System.Linq;
using DAL.Common.Helper;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;

namespace DAL.Common.DocClass
{
    public class Location : BaseDocument, ICommonDatabaseObject
    {
        public string Coid { get; set; }
        public string Branch { get; set; } // eur usa
        public string Region { get; set; } // meap, Europe, Western
        public string Country { get; set; } // country UAE, Scotland, USA, Norway
        public string Office { get; set; } // Telge
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string PhoneTollFree { get; set; }
        public string KronosLaborLevel { get; set; } = string.Empty;

        public List<Address> Addresses { get; set; } = new List<Address>();
        public MapLocation MapLocation { get; set; }
        public EntityRef Entity { get; set; }

        public List<PayrollRegionRef> PayrollRegions { get; set; } = new List<PayrollRegionRef>();

        public string Locale { get; set; } = string.Empty;
        public CurrencyTypeRef DefaultCurrency { get; set; }

        public LocationTimeZoneRef TimeZone { get; set; }

        public static Location Unknown
        {
            get
            {
                var filter = Helper.FilterBuilder.Where(x => x.Office == "<unknown>");
                return Helper.Find(filter).FirstOrDefault();
            }
        }



        public LocationRef AsLocationRef()
        {
            return new LocationRef(this);
        }

        public static LocationRef GetReferencesForOffice(string office)
        {
            var queryHelper = new MongoRawQueryHelper<Location>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Office == office);
            var location = queryHelper.Find(filter).FirstOrDefault();
            return location?.AsLocationRef();
        }

        public static LocationRef GetReferencesForEntityId(string id)
        {
            var queryHelper = new MongoRawQueryHelper<Location>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Entity.Id == id);
            var location = queryHelper.Find(filter).FirstOrDefault();
            return location?.AsLocationRef();
        }

        public static LocationRef GetReferencesForEntityName(string name)
        {
            var queryHelper = new MongoRawQueryHelper<Location>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Entity.Name == name);
            var location = queryHelper.Find(filter).FirstOrDefault();
            return location?.AsLocationRef();
        }

        public static LocationRef GetLocationRefForId(string id)
        {
            return id == null ? null : Helper.FindById(id)?.AsLocationRef();
        }

        public static LocationRef GetLocationRefForId(ObjectId id)
        {
            return Helper.FindById(id)?.AsLocationRef();
        }

        public static MongoRawQueryHelper<Location> Helper = new MongoRawQueryHelper<Location>();


    }
}
