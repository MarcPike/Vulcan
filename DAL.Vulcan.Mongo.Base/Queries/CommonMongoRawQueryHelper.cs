using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.Base.Queries
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