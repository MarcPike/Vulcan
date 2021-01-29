using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsTask : BaseDocument
    {
        public static MongoRawQueryHelper<BbsTask> Helper = new MongoRawQueryHelper<BbsTask>();
        public string Name { get; set; }
        public List<BbsTaskSubCategoryRef> SubCategories { get; set; } = new List<BbsTaskSubCategoryRef>();

        public BbsTaskRef AsBbsTaskRef()
        {
            return new BbsTaskRef(this);
        }

        public static BbsTaskRef GetRefForName(string name)
        {
            return new RepositoryBase<BbsTask>().AsQueryable().FirstOrDefault(x => x.Name == name)
                ?.AsBbsTaskRef();
        }
    }
}