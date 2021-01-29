using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    //public class ProductionTestList : BaseDocument
    //{
    //    public string Coid { get; set; }
    //    public ResourceType ResourceType { get; set; }
    //    public string ResourceTypeName => ResourceType.ToString();
    //    public List<ProductionStepTest> ProductionStepTests { get; set; } = new List<ProductionStepTest>();

    //    public static ProductionTestList GetFor(string coid, string resourceTypeName)
    //    {
    //        var resourceType = (Quotes.ResourceType) Enum.Parse(typeof(ResourceType), resourceTypeName);
    //        return ProductionTestList.GetFor(coid, resourceType);
    //    }
    //    public static ProductionTestList GetFor(string coid, ResourceType resourceType)
    //    {
    //        var rep = new RepositoryBase<ProductionTestList>();
    //        var result = rep.AsQueryable().SingleOrDefault(x => x.Coid == coid && x.ResourceType == resourceType) ??
    //                     new ProductionTestList() {Coid = coid, ResourceType = resourceType};
    //        return result;
    //    }
    //}
}