using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.Helpers
{
    public static class EmployeeLocationsFilterGenerator
    {
        public static FilterDefinition<Employee> GetLocationsFilter(MongoRawQueryHelper<Employee> queryHelper, List<LocationRef> locations,
            FilterDefinition<Employee> filter)
        {
            if (locations.Count > 0)
            {
                var locationIds = locations.Select(x => x.Id).ToList();
                    
                    if (filter == FilterDefinition<Employee>.Empty)
                    {
                        filter = queryHelper.FilterBuilder.In(x => x.Location.Id, locationIds);
                    }
                    else
                    {
                        filter = filter | queryHelper.FilterBuilder.In(x => x.Location.Id, locationIds);
                    }
            }
            else
            {
                filter = filter & queryHelper.FilterBuilder.Where(x => x.Id == null);
            }

            return filter;
        }

    }
}