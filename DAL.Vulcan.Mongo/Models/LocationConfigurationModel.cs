using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Config;
using DAL.Vulcan.Mongo.DocClass.Locations;

namespace DAL.Vulcan.Mongo.Models
{
    public class LocationConfigurationModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public LocationRef Location { get; set; }
        public string Name { get; set; }
        public Configuration Configuration { get; set; }
        public List<LocationResourceDefaultMargin> DefaultMargins { get; set; }

        public LocationConfigurationModel()
        {

        }

        public LocationConfigurationModel(string application, string userId, LocationConfiguration config)
        {
            Id = config.Id.ToString();
            Location = config.Location;
            Configuration = config.Configuration;
            DefaultMargins = config.DefaultMargins;
            Application = application;
            UserId = userId;
        }

        public static LocationConfigurationModel GetForLocation(string application, string userId, LocationRef location)
        {
            var rep = new RepositoryBase<LocationConfiguration>();
            var result = rep.AsQueryable().SingleOrDefault(x => x.Location.Id == location.Id);
            if (result == null)
            {
                result = new LocationConfiguration(location);
                rep.Upsert(result);
            }

            return new LocationConfigurationModel(application, userId, result);
        }
    }
}