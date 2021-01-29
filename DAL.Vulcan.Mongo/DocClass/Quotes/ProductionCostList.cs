using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Locations;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class ProductionCostList: BaseDocument
    {
        public string Coid { get; set; }
        public LocationRef Location { get; set; } = null;
        public ResourceType ResourceType { get; set; }
        public string ResourceTypeName => ResourceType.ToString();
        public List<CostValue> CostValues { get; set; } = new List<CostValue>();

        public static ProductionCostList GetFor(string coid, ResourceType resourceType, string office)
        {
            var rep = new RepositoryBase<ProductionCostList>();
            var rowFound = rep.AsQueryable().SingleOrDefault(x => x.Coid == coid && x.ResourceType == resourceType && x.Location.Office == office);
            return rowFound ?? new ProductionCostList() {Coid = coid, ResourceType = resourceType};
        }

        public static ProductionCostList GetFor(string coid, string resourceTypeName, string office)
        {
            var location = new RepositoryBase<Location>().AsQueryable().SingleOrDefault(x=>x.Office == office);
            if (location == null) throw new Exception($"No Location found for Office == {office}");

            var resourceType = (ResourceType) Enum.Parse(typeof(ResourceType), resourceTypeName);

            var rep = new RepositoryBase<ProductionCostList>();
            var rowFound = rep.AsQueryable().SingleOrDefault(x => x.Coid == coid && x.ResourceType == resourceType && x.Location.Office == office);
            return rowFound ?? new ProductionCostList() { Coid = coid, ResourceType = resourceType, Location = location.AsLocationRef()};
        }

    }
}