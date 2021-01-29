using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsDepartment : BaseDocument
    {
        public static MongoRawQueryHelper<BbsDepartment> Helper = new MongoRawQueryHelper<BbsDepartment>();

        public string Name { get; set; }
        public List<BbsDepartmentSubCategoryRef> SubCategories { get; set; } = new List<BbsDepartmentSubCategoryRef>();

        public BbsDepartmentRef AsBbsDepartmentRef()
        {
            return new BbsDepartmentRef(this);
        }

        public static BbsDepartmentRef GetRefForName(string name)
        {
            return new RepositoryBase<BbsDepartment>().AsQueryable().FirstOrDefault(x => x.Name == name)
                ?.AsBbsDepartmentRef();
        }
    }
}
