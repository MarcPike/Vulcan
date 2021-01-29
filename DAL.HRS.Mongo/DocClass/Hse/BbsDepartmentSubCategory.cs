using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Driver;


namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsDepartmentSubCategory : BaseDocument
    {
        public static MongoRawQueryHelper<BbsDepartmentSubCategory> Helper = new MongoRawQueryHelper<BbsDepartmentSubCategory>();
        public string Name { get; set; }

        public BbsDepartmentSubCategoryRef AsBbsDepartmentSubCategoryRef()
        {
            return new BbsDepartmentSubCategoryRef(this);
        }

        public static BbsDepartmentSubCategoryRef GetRefForName(string name)
        {
            var sub = Helper.Find(x=>x.Name == name).FirstOrDefault();
            if (sub != null) return sub.AsBbsDepartmentSubCategoryRef();
            sub = new BbsDepartmentSubCategory()
            {
                Name = name
            };
            Helper.Upsert(sub);

            return sub.AsBbsDepartmentSubCategoryRef();
        }

    }
}