using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhProductUsageRef : ReferenceObject<CoshhProductUsage>
    {
        public string UsageType { get; set; }

        public CoshhProductUsageRef()
        {
        }

        public CoshhProductUsageRef(CoshhProductUsage usage) : base(usage)
        {
            UsageType = usage.UsageType;
        }

        public CoshhProductUsage AsCoshhProductUsage()
        {
            return ToBaseDocument();
        }
    }
}