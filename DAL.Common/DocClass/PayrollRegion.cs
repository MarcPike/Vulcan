using System.Linq;
using System.Net;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.Common.DocClass
{
    public class PayrollRegion : BaseDocument, ICommonDatabaseObject
    {
        public static MongoRawQueryHelper<PayrollRegion> Helper = new MongoRawQueryHelper<PayrollRegion>();
        public string Name { get; set; }

        public static void PopulateValues()
        {
            var rep = new CommonRepository<PayrollRegion>();

            AddIfRequired("Asia Pacific");
            AddIfRequired("Asia Pacific Executive");
            AddIfRequired("China");
            AddIfRequired("Europe");
            AddIfRequired("Malaysia");
            AddIfRequired("Middle East");
            AddIfRequired("Singapore");
            AddIfRequired("Western Hemisphere");

            void AddIfRequired(string name)
            {
                if (rep.AsQueryable().ToList().All(x => x.Name != name))
                {
                    rep.Upsert(new PayrollRegion()
                    {
                        Name = name
                    });
                }
            }
        }

        public PayrollRegionRef AsPayrollRegionRef()
        {
            return new PayrollRegionRef(this);
        }
    }
}