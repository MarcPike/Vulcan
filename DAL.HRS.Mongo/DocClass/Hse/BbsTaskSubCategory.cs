using System;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsTaskSubCategory : BaseDocument
    {
        public static MongoRawQueryHelper<BbsTaskSubCategory> Helper = new MongoRawQueryHelper<BbsTaskSubCategory>();
        public string Name { get; set; }

        public BbsTaskSubCategoryRef AsBbsTaskSubCategoryRef()
        {
            return new BbsTaskSubCategoryRef(this);
        }

        public static BbsTaskSubCategoryRef GetRefForName(string name)
        {
            var sub = Helper.Find(x => x.Name == name).FirstOrDefault();
            if (sub != null) return sub.AsBbsTaskSubCategoryRef();
            sub = new BbsTaskSubCategory()
            {
                Name = name
            };
            Helper.Upsert(sub);

            return sub.AsBbsTaskSubCategoryRef();
        }

    }
}