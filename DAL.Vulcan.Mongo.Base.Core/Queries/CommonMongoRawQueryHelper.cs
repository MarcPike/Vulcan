using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Base.Core.DocClass;

namespace DAL.Vulcan.Mongo.Base.Core.Queries
{
    public class CommonMongoRawQueryHelper<TBaseDocument> : MongoRawQueryHelper<TBaseDocument>
        where TBaseDocument : BaseDocument
    {
        public CommonMongoRawQueryHelper()
        {
            Context = new CommonContext();
        }

        public static TBaseDocument SaveToDatabase(TBaseDocument doc)
        {
            return new CommonMongoRawQueryHelper<TBaseDocument>().Upsert(doc);
        }
    }
}