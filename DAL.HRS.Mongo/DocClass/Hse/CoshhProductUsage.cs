using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhProductUsage : BaseDocument
    {
        public string UsageType { get; set; }

        public CoshhProductUsageRef AsCoshhProductUsageRef()
        {
            return new CoshhProductUsageRef(this);
        }

        public static CoshhProductUsageRef CreateAndReturnRef(RepositoryBase<CoshhProductUsage> rep, string usageType)
        {
            var newUsageType = rep.AsQueryable().FirstOrDefault(x => x.UsageType == usageType);
            if (newUsageType == null)
            {
                newUsageType = new CoshhProductUsage()
                {
                    UsageType = usageType
                };
                rep.Upsert(newUsageType);
            }

            return newUsageType.AsCoshhProductUsageRef();
        }

    }
}