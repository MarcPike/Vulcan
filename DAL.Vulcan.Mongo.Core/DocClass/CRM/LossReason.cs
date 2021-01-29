using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Queries;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class LostReason: BaseDocument
    {
        public static MongoRawQueryHelper<LostReason> Helper = new MongoRawQueryHelper<LostReason>();
        public string Reason { get; set; }

        public LostReason()
        {
        }


    }
}
