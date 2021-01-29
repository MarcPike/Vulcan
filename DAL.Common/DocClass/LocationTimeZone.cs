using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Common.DocClass
{
    public class LocationTimeZone : BaseDocument, ICommonDatabaseObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal GmtOffset { get; set; }
        public int OldHrsId { get; set; }

        public LocationTimeZone()
        {
            
        }

        public LocationTimeZoneRef AsLocationTimeZoneRef()
        {
            return new LocationTimeZoneRef(this);
        }

        public static LocationTimeZoneRef Unspecified()
        {
            var filter = Helper.FilterBuilder.Where(x => x.Name == "Unspecified");
            return Helper.Find(filter).Single().AsLocationTimeZoneRef();
        }

        public static MongoRawQueryHelper<LocationTimeZone> Helper { get; set; } = new MongoRawQueryHelper<LocationTimeZone>();

        public static List<LocationTimeZone> Initialize()
        {
            var existingTimeZones = Helper.GetAll();

            var timeZones = new List<LocationTimeZone>()
            {
                new LocationTimeZone()
                    {Name = "Unspecified", Description = "Unspecified or Unknown", GmtOffset = 0, OldHrsId = 1},
                new LocationTimeZone()
                    {Name = "Eastern Time", Description = "U.S. Eastern Time Zone", GmtOffset = -5, OldHrsId = 2},
                new LocationTimeZone()
                    {Name = "Central Time", Description = "U.S. Central Time Zone", GmtOffset = -6, OldHrsId = 3},
                new LocationTimeZone()
                    {Name = "Mountain Time", Description = "U.S. Mountain Time Zone", GmtOffset = -7, OldHrsId = 4},
                new LocationTimeZone()
                    {Name = "Pacific Time", Description = "U.S. Pacific Time Zone", GmtOffset = -8, OldHrsId = 5},
                new LocationTimeZone()
                    {Name = "Kuala Lumpur Singapore", Description = "Singapore Malaysia", GmtOffset = 8, OldHrsId = 6},
                new LocationTimeZone() {Name = "Greenwich Mean Time", Description = "GMT", GmtOffset = 0, OldHrsId = 7},
                new LocationTimeZone() {Name = "Abu Dhabi", Description = "Dubai Time Zone", GmtOffset = 4, OldHrsId = 8},
                new LocationTimeZone() {Name = "Beijing", Description = "China Office", GmtOffset = 8, OldHrsId = 9},
                new LocationTimeZone() {Name = "Europe", Description = "Norway", GmtOffset = 1, OldHrsId = 14},
                new LocationTimeZone() {Name = "Central European Time", Description = "European Central Time (Standard Time)", GmtOffset = 1, OldHrsId = 0},
                new LocationTimeZone() {Name = "Christmas Island Time (Jakarta)", Description = "Christmas Island Time (Jakarta)", GmtOffset = 7, OldHrsId = 0},
                new LocationTimeZone() {Name = "Australian Eastern Standard Time", Description = "Australian Eastern Standard Time", GmtOffset = 10, OldHrsId = 0},
                new LocationTimeZone() {Name = "(No DST) EM Australia", Description = "Australian Western (No DST)", GmtOffset = 8, OldHrsId = 0},
            };

            foreach (var timeZone in timeZones)
            {
                if (existingTimeZones.All(x => x.Name != timeZone.Name))
                {
                    Helper.Upsert(timeZone);
                }
            }


            return Helper.GetAll();
        }
    }
}
