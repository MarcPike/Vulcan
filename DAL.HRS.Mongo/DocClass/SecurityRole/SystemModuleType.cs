using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.SecurityRole
{
    public class SystemModuleType : BaseDocument
    {
        public static MongoRawQueryHelper<SystemModuleType> Helper { get; set; } = new MongoRawQueryHelper<SystemModuleType>();
        public string Name { get; set; }
        public bool IsHrsModule { get; set; }
        public bool IsHseModule { get; set; }


        public SystemModuleTypeRef AsSystemModuleTypeRef()
        {
            return new SystemModuleTypeRef(this);
        }
    }
}