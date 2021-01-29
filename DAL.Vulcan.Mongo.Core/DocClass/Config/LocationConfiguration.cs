using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.DocClass.Config
{
    public class LocationConfiguration : BaseConfiguration
    {
        public LocationRef Location { get; set; }

        public List<LocationResourceDefaultMargin> DefaultMargins { get; set; } = new List<LocationResourceDefaultMargin>();

        public LocationConfiguration()
        {

        }

        public LocationConfiguration(LocationRef location)
        {
            Location = location;
            Configuration = new Configuration();
        }

        public LocationConfiguration(LocationRef location, Configuration configuration)
        {
            Location = location;
            Configuration = Configuration;
        }


        public static LocationConfiguration GetConfigurationFor(LocationRef locationRef)
        {
            return new RepositoryBase<LocationConfiguration>().AsQueryable().SingleOrDefault(x => x.Location.Id == locationRef.Id);
        }
        
    }
}